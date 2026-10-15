using WarehouseManagement.Domain.Entities;
namespace WarehouseManagement.Tests.Domain;

public class ProductTests
{
    [Fact]
    public void Constructor_WithValidData_ShouldCreateProduct()
    {
        // Arrange
        string sku = "LAP-001";
        string name = "Dell Laptop";

        // Act
        var product = new Product(sku, name);

        // Assert
        Assert.NotEqual(Guid.Empty, product.Id);
        Assert.Equal("LAP-001", product.Sku);
        Assert.Equal("Dell Laptop", product.Name);
        Assert.Equal("Piece", product.UnitOfMeasure);
        Assert.True(product.IsActive);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_WithInvalidSku_ShouldThrowArgumentException(
        string? sku)
    {
        Assert.Throws<ArgumentException>(
            () => new Product(sku!, "Dell Laptop"));
    }

    [Fact]
    public void Constructor_ShouldNormalizeSkuAndName()
    {
        var product = new Product(" lap-001 ", " Dell Laptop ");

        Assert.Equal("LAP-001", product.Sku);
        Assert.Equal("Dell Laptop", product.Name);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_WithInvalidName_ShouldThrowArgumentException(
        string? name)
    {
        Assert.Throws<ArgumentException>(
            () => new Product("LAP-001", name!));
    }

    [Fact]
    public void Constructor_WithSkuLongerThan50Characters_ShouldThrowArgumentException()
    {
        string sku = new string('A', 51);

        Assert.Throws<ArgumentException>(
            () => new Product(sku, "Dell Laptop"));
    }

    [Fact]
    public void Constructor_WithNameLongerThan200Characters_ShouldThrowArgumentException()
    {
        string name = new string('A', 201);

        Assert.Throws<ArgumentException>(
            () => new Product("LAP-001", name));
    }

    [Fact]
    public void Constructor_WithMaximumAllowedLengths_ShouldCreateProduct()
    {
        string sku = new string('A', 50);
        string name = new string('B', 200);

        var product = new Product(sku, name);

        Assert.Equal(50, product.Sku.Length);
        Assert.Equal(200, product.Name.Length);
    }
}

