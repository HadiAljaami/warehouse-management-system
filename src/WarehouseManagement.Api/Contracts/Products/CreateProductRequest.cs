namespace WarehouseManagement.Api.Contracts.Products;

public sealed record CreateProductRequest(
    string? Sku,
    string? Name);