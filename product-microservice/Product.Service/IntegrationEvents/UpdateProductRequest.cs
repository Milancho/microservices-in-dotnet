namespace Product.Service.ApiModels;

public record UpdateProductRequest(string Name, decimal Price, int ProductTypeId, string? Description = null);
