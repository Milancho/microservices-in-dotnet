namespace Product.Service.Models;

internal class Product
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public string? Description { get; set; }

    public required decimal Price { get; set; }

    public required int ProductTypeId { get; set; }
    public ProductType? ProductType { get; set; }
}
