using Basket.Service.ApiModels;
using Basket.Service.Infrastructure.Data;
using Basket.Service.Models;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.AspNetCore.Mvc;

namespace Basket.Service.Endpoints;

public static class BasketApiEndpoints
{
    public static void RegisterEndpoints(this IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder.MapGet("/{customerId}", GetBasket);

        routeBuilder.MapPost("/{customerId}", CreateBasket);

        routeBuilder.MapPut("/{customerId}", AddBasketProduct);

        routeBuilder.MapDelete("/{customerId}/{productId}", DeleteBasketProduct);

        routeBuilder.MapDelete("/{customerId}", DeleteBasket);       

        routeBuilder.MapPost("/", ([FromServices] IBasketStore basketStore, CreateBasketRequest createBasketRequest) =>
        {
            return TypedResults.Created(createBasketRequest.ProductId.ToString());
        }).RequireAuthorization();
    }

    internal static async Task<CustomerBasket> GetBasket(IBasketStore basketStore, string customerId)
        => await basketStore.GetBasketByCustomerId(customerId);

    internal static async Task<IResult> CreateBasket(IBasketStore basketStore, IDistributedCache cache, string customerId, CreateBasketRequest createBasketRequest)
    {
        var customerBasket = new CustomerBasket { CustomerId = customerId };

        var cachedProductPrice = decimal.Parse(await cache.GetStringAsync(createBasketRequest.ProductId));

        customerBasket.AddBasketProduct(new BasketProduct(createBasketRequest.ProductId, createBasketRequest.ProductName, cachedProductPrice));

        await basketStore.CreateCustomerBasket(customerBasket);

        return TypedResults.Created();
    }

    internal static async Task<IResult> AddBasketProduct(IBasketStore basketStore, IDistributedCache cache, string customerId, AddBasketProductRequest addProductRequest)
    {
        var customerBasket = await basketStore.GetBasketByCustomerId(customerId);

        var cachedProductPrice = decimal.Parse(await cache.GetStringAsync(addProductRequest.ProductId));

        customerBasket.AddBasketProduct(new BasketProduct(addProductRequest.ProductId,
            addProductRequest.ProductName, cachedProductPrice, addProductRequest.Quantity));

        await basketStore.UpdateCustomerBasket(customerBasket);

        return TypedResults.NoContent();
    }

    internal static async Task<IResult> DeleteBasketProduct(IBasketStore basketStore, string customerId, string productId)
    {
        var customerBasket = await basketStore.GetBasketByCustomerId(customerId);

        customerBasket.RemoveBasketProduct(productId);

        await basketStore.UpdateCustomerBasket(customerBasket);

        return TypedResults.NoContent();
    }

    internal static async Task<IResult> DeleteBasket(IBasketStore basketStore, string customerId)
    {
        await basketStore.DeleteCustomerBasket(customerId);

        return TypedResults.NoContent();
    }
}