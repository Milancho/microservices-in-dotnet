namespace Product.Service.ApiModels;

public record GetProductResponse(int Id, string Name, decimal Price, string ProductType, string? Description = null);
