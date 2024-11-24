using Microsoft.AspNetCore.Mvc;
using Order.Service.Infrastructure.Data;

namespace Order.Service.Endpoints;

public static class OrderApiEndpoints
{
    public static void RegisterEndpoints(this IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder.MapPost("/{customerId}", ([FromServices] IOrderStore orderStore, string customerId, CreateOrderRequest request) =>
        {
            var order = new Models.Order
            {
                CustomerId = customerId
            };

            foreach (var product in request.OrderProducts)
            {
                order.AddOrderProduct(product.ProductId, product.Quantity);
            }

            orderStore.CreateOrder(order);

            return TypedResults.Created($"{order.CustomerId}/{order.OrderId}");
        });

        routeBuilder.MapGet("/{customerId}/{orderId}", IResult ([FromServices] IOrderStore orderStore, string customerId, string orderId) =>
        {
            var order = orderStore.GetCustomerOrderById(customerId, orderId);
            return order is null
            ? TypedResults.NotFound("Order not found for customer")
            : TypedResults.Ok(order);
        });
        
    }
}