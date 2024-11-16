using Basket.Service.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace Basket.Service.Endpoints;

public static class BasketApiEndpoints
{
    public static void RegisterEndpoints(this IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder.MapGet("/{customerId}", (
          [FromServices] IBasketStore basketStore, string customerId)
              => basketStore.GetBasketByCustomerId(customerId));
    }
}