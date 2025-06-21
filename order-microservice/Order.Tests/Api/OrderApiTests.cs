using Order.Service.ApiModels;
using Order.Tests;
using System.Net;
using System.Net.Http.Json;

public class OrderApiTests : IntegrationTestBase
{
    public OrderApiTests(OrderWebApplicationFactory webApplicationFactory)
        : base(webApplicationFactory)
    {
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
        var order = new Order.Service.Models.Order { CustomerId = "1" };
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