using Microsoft.AspNetCore.Mvc;
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
    }
}