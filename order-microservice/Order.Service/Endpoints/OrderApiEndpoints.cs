using Microsoft.AspNetCore.Mvc;
using Order.Service.ApiModels;
using Order.Service.Infrastructure.Data;
using Order.Service.IntegrationEvents.Events;
using ECommerce.Shared.Infrastructure.EventBus.Abstractions;

namespace Order.Service.Endpoints;

public static class OrderApiEndpoint
{
    public static void RegisterEndpoints(this IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder.MapPost("/{customerId}", async ([FromServices] IEventBus eventBus,
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

            await orderStore.CreateOrder(order);

            await eventBus.PublishAsync(new OrderCreatedEvent(customerId));

            return TypedResults.Created($"{order.CustomerId}/{order.OrderId}");
        });

        routeBuilder.MapGet("/{customerId}/{orderId}", async Task<IResult> ([FromServices] IOrderStore orderStore, string customerId, string orderId) =>
        {
            var order = await orderStore.GetCustomerOrderById(customerId, orderId);

            return order is null
                ? TypedResults.NotFound("Order not found for customer")
                : TypedResults.Ok(new GetOrderResponse(order.CustomerId, order.OrderId, order.OrderDate,
                order.OrderProducts.Select(op => new GetOrderProductResponse(op.ProductId, op.Quantity)).ToList()));
        });
        
    }
}