using ECommerce.Shared.Infrastructure.EventBus;
using ECommerce.Shared.Infrastructure.RabbitMq;
using Microsoft.Extensions.DependencyInjection;
using Order.Service.ApiModels;
using Order.Service.Infrastructure.Data.EntityFramework;
using Order.Tests;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace Order.Tests;

public class IntegrationTestBase : IClassFixture<OrderWebApplicationFactory>
{
    internal readonly OrderContext OrderContext;
    internal readonly HttpClient HttpClient;

    public IntegrationTestBase(OrderWebApplicationFactory webApplicationFactory)
    {
        var scope = webApplicationFactory.Services.CreateScope();

        OrderContext = scope.ServiceProvider.GetRequiredService<OrderContext>();
        HttpClient = webApplicationFactory.CreateClient();
    }

    [Fact]
    public async Task GetOrder_WhenNoOrderExists_ThenReturnsNotFound()
    {
        // Act
        var response = await HttpClient.GetAsync($"/1/{Guid.NewGuid()}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetOrder_WhenOrderExists_ThenReturnsOrder()
    {
        // Arrange
        var order = new Service.Models.Order { CustomerId = "1" };
        await OrderContext.CreateOrder(order);

        // Act
        var response = await HttpClient.GetAsync($"/{order.CustomerId}/{order.OrderId}");

        // Assert
        response.EnsureSuccessStatusCode();

        var getOrderResponse = await response.Content.ReadFromJsonAsync<GetOrderResponse>();

        Assert.NotNull(getOrderResponse);
        Assert.Equal(order.OrderId, getOrderResponse.OrderId);
    }

    [Fact]
    public async Task CreateOrder_WhenCalled_ThenCreatesOrder()
    {
        // Arrange
        var createOrderRequest = new CreateOrderRequest([]);

        // Act
        var response = await HttpClient.PostAsJsonAsync("/1", createOrderRequest);

        // Assert
        response.EnsureSuccessStatusCode();

        var locationHeader = response.Headers.FirstOrDefault(h =>
            string.Equals(h.Key, "Location")).Value.FirstOrDefault();

        Assert.NotNull(locationHeader);
        var split = locationHeader.Split('/');
        var customerId = split[0];
        var orderId = split[1];

        var order = OrderContext.Orders.FirstOrDefault(o =>
            o.OrderId == Guid.Parse(orderId) && o.CustomerId == customerId);

        Assert.NotNull(order);
    }

}