namespace WarehouseManagement.Api.Contracts.Products;

public sealed record ProductResponse(
    Guid Id,
    string Sku,
    string Name,
    string UnitOfMeasure,
    bool IsActive);