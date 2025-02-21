namespace Product.Service.ApiModels;

public record CreateProductRequest(string Name, decimal Price, int ProductTypeId, string? Description = null);
