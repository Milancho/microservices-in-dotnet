using Basket.Service.ApiModels;
using Basket.Service.Endpoints;
using Basket.Service.Infrastructure.Data;
using Basket.Service.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Caching.Distributed;
using NSubstitute;
using System.Text;

namespace Basket.Tests.Endpoints;
public class BasketApiEndpointsTests
{
    private readonly IBasketStore _basketStore = Substitute.For<IBasketStore>();
    private readonly IDistributedCache _cache = Substitute.For<IDistributedCache>();

    [Fact]
    public async Task GivenExistingBasket_WhenCallingGetBasket_ThenReturnsBasket()
    {
        // Arrange
        const string customerId = "1";
        var customerBasket = new CustomerBasket { CustomerId = customerId };

        _basketStore.GetBasketByCustomerId(customerId)
            .Returns(customerBasket);

        // Act
        var result = await BasketApiEndpoints.GetBasket(_basketStore, customerId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(customerId, result.CustomerId);
    }

    [Fact]
    public async Task GivenNewBasketRequest_WhenCallingCreateBasket_ThenReturnsCreatedResult()
    {
        // Arrange
        const string customerId = "1";
        const string productId = "1";
        var createBasketRequest = new CreateBasketRequest(productId, "Test Name");

        _cache.GetAsync(productId)
            .Returns(Encoding.UTF8.GetBytes("1.00"));

        // Act
        var result = await BasketApiEndpoints.CreateBasket(_basketStore, _cache, customerId, createBasketRequest);

        // Assert
        Assert.NotNull(result);

        var createdResult = (Created)result;
        Assert.NotNull(createdResult);
    }

    [Fact]
    public async Task GivenExistingBasket_WhenCallingAddBasketProduct_ThenReturnsNoContentResult()
    {
        // Arrange
        const string customerId = "1";
        var customerBasket = new CustomerBasket { CustomerId = customerId };

        _basketStore.GetBasketByCustomerId(customerId).Returns(customerBasket);

        const string productId = "1";
        const int quantity = 2;
        _cache.GetAsync(productId)
            .Returns(Encoding.UTF8.GetBytes("1.00"));

        var addBasketProductRequest = new AddBasketProductRequest(productId, "Test Name", quantity);

        // Act
        var result = await BasketApiEndpoints.AddBasketProduct(_basketStore, _cache, customerId, addBasketProductRequest);

        // Assert
        Assert.NotNull(result);

        var noContentResult = (NoContent)result;
        Assert.NotNull(noContentResult);
    }

    [Fact]
    public async Task GivenExistingBasketWithProducts_WhenCallingDeleteBasketProduct_ThenReturnsNoContentResult()
    {
        // Arrange
        const string customerId = "1";
        const string productId = "1";
        var customerBasket = new CustomerBasket { CustomerId = customerId };

        customerBasket.AddBasketProduct(new BasketProduct(productId, "Test Name", 9.99m));

        _basketStore.GetBasketByCustomerId(customerId).Returns(customerBasket);

        // Act
        var result = await BasketApiEndpoints.DeleteBasketProduct(_basketStore, customerId, productId);

        // Assert
        Assert.NotNull(result);

        var noContentResult = (NoContent)result;
        Assert.NotNull(noContentResult);
    }

    [Fact]
    public async Task GivenExistingBasket_WhenCallingDeleteBasket_ThenReturnsNoContentResult()
    {
        // Arrange
        const string customerId = "1";
        var customerBasket = new CustomerBasket { CustomerId = customerId };

        _basketStore.GetBasketByCustomerId(customerId).Returns(customerBasket);

        // Act
        var result = await BasketApiEndpoints.DeleteBasket(_basketStore, customerId);

        // Assert
        Assert.NotNull(result);

        var noContentResult = (NoContent)result;
        Assert.NotNull(noContentResult);
    }
}
