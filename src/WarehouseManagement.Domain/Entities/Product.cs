namespace WarehouseManagement.Domain.Entities;

public class Product
{
    public Guid Id { get; private set; }
    public string Sku { get; private set; } = null!;
    public string Name { get; private set; } = null!;
    public string UnitOfMeasure { get; private set; } = "Piece";
    public bool IsActive { get; private set; } = true;

    private Product()
    {
        // Required by EF Core.
    }

    public Product(string sku, string name)
    {
        if (string.IsNullOrWhiteSpace(sku))
            throw new ArgumentException(
                "SKU is required.", nameof(sku));

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException(
                "Product name is required.", nameof(name));

        sku = sku.Trim().ToUpperInvariant();
        name = name.Trim();

        if (sku.Length > 50)
            throw new ArgumentException(
                "SKU cannot exceed 50 characters.", nameof(sku));

        if (name.Length > 200)
            throw new ArgumentException(
                "Product name cannot exceed 200 characters.", nameof(name));

        Id = Guid.NewGuid();
        Sku = sku;
        Name = name;
    }
}