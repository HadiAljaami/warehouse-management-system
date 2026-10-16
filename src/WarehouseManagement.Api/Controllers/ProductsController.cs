using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using WarehouseManagement.Api.Contracts.Products;
using WarehouseManagement.Domain.Entities;
using WarehouseManagement.Infrastructure.Persistence;

namespace WarehouseManagement.Api.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly WarehouseDbContext _dbContext;

    public ProductsController(WarehouseDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpPost]
    public async Task<ActionResult<ProductResponse>> Create(
        CreateProductRequest request,
        CancellationToken cancellationToken)
    {
        Product product;

        try
        {
            product = new Product(request.Sku!, request.Name!);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Invalid product data",
                Detail = ex.Message
            });
        }

        _dbContext.Products.Add(product);

        try
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex)
            when (IsDuplicateSkuViolation(ex))
        {
            return Conflict(new ProblemDetails
            {
                Status = StatusCodes.Status409Conflict,
                Title = "Duplicate SKU",
                Detail = "A product with this SKU already exists."
            });
        }

        var response = new ProductResponse(
            product.Id,
            product.Sku,
            product.Name,
            product.UnitOfMeasure,
            product.IsActive);

        return StatusCode(
            StatusCodes.Status201Created,
            response);
    }

    private static bool IsDuplicateSkuViolation(
        DbUpdateException exception)
    {
        if (exception.InnerException is not SqlException sqlException)
        {
            return false;
        }

        if (sqlException.Number is not (2601 or 2627))
        {
            return false;
        }

        return sqlException.Message.Contains(
            "IX_Products_Sku",
            StringComparison.OrdinalIgnoreCase);
    }
}