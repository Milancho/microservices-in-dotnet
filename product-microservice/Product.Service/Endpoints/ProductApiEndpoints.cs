using ECommerce.Shared.Infrastructure.EventBus.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Product.Service.ApiModels;
using Product.Service.Infrastructure.Data;
using Product.Service.IntegrationEvents;

namespace Product.Service.Endpoints;

public static class ProductApiEndpoints
{
    public static void RegisterEndpoints(this IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder.MapGet("/{productId}", async Task<IResult> ([FromServices] IProductStore productStore, int productId) =>
        {
            var product = await productStore.GetById(productId);

            if (product is null || product.ProductType is null)
                {
                    return TypedResults.NotFound("Product not found");
                }
            
            return product is null
                ? TypedResults.NotFound("Product not found")
                : TypedResults.Ok(new GetProductResponse(product.Id, product.Name, product.Price, product.ProductType.Type, product.Description));
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

        routeBuilder.MapPut("/{productId}", async Task<IResult> ([FromServices] IProductStore productStore, [FromServices] IEventBus eventBus,
            int productId, UpdateProductRequest request) =>
        {
            var product = await productStore.GetById(productId);

            if (product is null)
            {
                return TypedResults.NotFound($"Product with id {productId} does not exist");
            }

            var existingPrice = product.Price;

            product.Name = request.Name;
            product.Price = request.Price;
            product.ProductTypeId = request.ProductTypeId;
            product.Description = request.Description;

            await productStore.UpdateProduct(product);

            if (!decimal.Equals(existingPrice, request.Price))
            {
                await eventBus.PublishAsync(new ProductPriceUpdatedEvent(productId, request.Price));
            }

            return TypedResults.NoContent();
        });
    }
}