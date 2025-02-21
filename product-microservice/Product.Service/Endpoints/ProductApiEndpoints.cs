using Microsoft.AspNetCore.Mvc;
using Product.Service.ApiModels;
using Product.Service.Infrastructure.Data;

namespace Product.Service.Endpoints;

public static class ProductApiEndpoints
{
    public static void RegisterEndpoints(this IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder.MapGet("/{productId}", async Task<IResult> ([FromServices] IProductStore productStore, int productId) =>
        {
            var product = await productStore.GetById(productId);

            return product is null
                ? TypedResults.NotFound("Product not found")
                : TypedResults.Ok(product);
        });

        routeBuilder.MapPost("/", async ([FromServices] IProductStore productStore, CreateProductRequest request) =>
        {
            var product = new Models.Product
            {
                Name = request.Name,
                Price = request.Price,
                Description = request.Description,
                ProductTypeId = request.ProductTypeId
            };
            await productStore.CreateProduct(product);
            return TypedResults.Created(product.Id.ToString());
        });
    }    
}