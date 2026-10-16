using System.Net;
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WarehouseManagement.Infrastructure.Persistence;

namespace WarehouseManagement.Tests.Integration.Products;

public class CreateProductTests
{
    [Fact]
    public async Task CreateProduct_WithValidData_Returns201AndPersistsProduct()
    {
        // Arrange
        await using var factory = new CustomWebApplicationFactory();

        await factory.InitializeDatabaseAsync();

        using var client = factory.CreateClient();

        var sku = $"TEST-{Guid.NewGuid():N}";

        var request = new
        {
            Sku = sku,
            Name = "Integration Test Product"
        };

        // Act
        var response = await client.PostAsJsonAsync(
            "/api/products",
            request);

        // Assert: HTTP response
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        // Assert: Database persistence
        using var verificationScope = factory.Services.CreateScope();

        var verificationDb = verificationScope.ServiceProvider
            .GetRequiredService<WarehouseDbContext>();

        var product = await verificationDb.Products
            .AsNoTracking()
            .SingleOrDefaultAsync(p => p.Sku == sku);

        Assert.NotNull(product);
        Assert.Equal("Integration Test Product", product.Name);
    }

    [Fact]
    public async Task CreateProduct_WithEmptySku_Returns400AndDoesNotPersist()
    {
        // Arrange
        await using var factory = new CustomWebApplicationFactory();

        await factory.InitializeDatabaseAsync();

        using var client = factory.CreateClient();

        var productName = $"Invalid Product {Guid.NewGuid():N}";

        var request = new
        {
            Sku = "",
            Name = productName
        };

        // Act
        var response = await client.PostAsJsonAsync(
            "/api/products",
            request);

        // Assert: HTTP response
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        // Assert: No product was persisted
        using var scope2 = factory.Services.CreateScope();

        var dbContext2 = scope2.ServiceProvider
            .GetRequiredService<WarehouseDbContext>();

        var exists = await dbContext2.Products
            .AnyAsync(p => p.Name == productName);

        Assert.False(exists);
    }

    [Fact]
    public async Task CreateProduct_WithDuplicateSku_Returns409AndDoesNotDuplicate()
    {
        // Arrange
        await using var factory = new CustomWebApplicationFactory();

        await factory.InitializeDatabaseAsync();

        using var client = factory.CreateClient();

        var sku = $"TEST-{Guid.NewGuid():N}";

        var firstRequest = new
        {
            Sku = sku,
            Name = "Original Product"
        };

        var duplicateRequest = new
        {
            Sku = sku,
            Name = "Duplicate Product"
        };

        // Create the original product
        var firstResponse = await client.PostAsJsonAsync(
            "/api/products",
            firstRequest);

        Assert.Equal(HttpStatusCode.Created, firstResponse.StatusCode);

        // Act: Attempt to create another product with the same SKU
        var duplicateResponse = await client.PostAsJsonAsync(
            "/api/products",
            duplicateRequest);

        // Assert: HTTP response
        Assert.Equal(HttpStatusCode.Conflict, duplicateResponse.StatusCode);

        // Assert: Only the original product exists
        using var verificationScope = factory.Services.CreateScope();

        var verificationDbContext = verificationScope.ServiceProvider
            .GetRequiredService<WarehouseDbContext>();

        var products = await verificationDbContext.Products
            .AsNoTracking()
            .Where(p => p.Sku == sku)
            .ToListAsync();

        Assert.Single(products);
        Assert.Equal("Original Product", products[0].Name);
    }



}