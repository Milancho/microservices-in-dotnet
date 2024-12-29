using Microsoft.AspNetCore.Mvc;
using Order.Service.ApiModels;
using Order.Service.Infrastructure.Data;
using Order.Service.IntegrationEvents.Events;
using Order.Service.Infrastructure.EventBus.Abstractions;

namespace Order.Service.Endpoints;

public static class OrderApiEndpoint
{
    public static void RegisterEndpoints(this IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder.MapPost("/{customerId}", (
            [FromServices] IEventBus eventBus,
            [FromServices] IOrderStore orderStore,
            string customerId, CreateOrderRequest request) =>
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

            eventBus.PublishAsync(new OrderCreatedEvent(customerId));

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