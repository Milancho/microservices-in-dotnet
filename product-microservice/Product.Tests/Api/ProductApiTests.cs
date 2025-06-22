using Product.Service.ApiModels;
using Product.Service.Infrastructure.Data.EntityFramework;
using Product.Service.IntegrationEvents;
using System.Net;
using System.Net.Http.Json;

namespace Product.Tests.Api;
public class ProductApiTests : IntegrationTestBase
{
    public ProductApiTests(ProductWebApplicationFactory webApplicationFactory)
        : base(webApplicationFactory)
    {
    }

    [Fact]
    public async Task GetProduct_WhenNoProductExists_ThenReturnsNotFound()
    {
        // Act
        var response = await HttpClient.GetAsync("/1");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetProduct_WhenProductExists_ThenReturnsProduct()
    {
        // Arrange
        var product = new Service.Models.Product { Name = "Test", Price = 9.99m, ProductTypeId = 1 };
        await ProductContext.CreateProduct(product);

        // Act
        var response = await HttpClient.GetAsync(product.Id.ToString());

        // Assert
        response.EnsureSuccessStatusCode();

        var getProductResponse = await response.Content.ReadFromJsonAsync<GetProductResponse>();

        Assert.NotNull(getProductResponse);
        Assert.Equal(product.Id, getProductResponse.Id);
    }

    [Fact]
    public async Task CreateProduct_WhenCalled_ThenCreatesProduct()
    {
        // Arrange
        var createProductRequest = new CreateProductRequest("Test name", 9.99m, 1);

        // Act
        var response = await HttpClient.PostAsJsonAsync("/", createProductRequest);

        // Assert
        response.EnsureSuccessStatusCode();

        var locationHeader = response.Headers.FirstOrDefault(h => string.Equals(h.Key, "Location")).Value.FirstOrDefault();

        Assert.NotNull(locationHeader);

        var product = await ProductContext.Products.FindAsync(int.Parse(locationHeader));
        Assert.NotNull(product);
    }

    [Fact]
    public async Task UpdateProduct_WhenCalledWithExistingProductAndNewPrice_ThenProductPriceUpdatedEventPublished()
    {
        // Arrange
        var product = new Service.Models.Product { Name = "Test name", Price = 9.99m, ProductTypeId = 1 };
        await ProductContext.CreateProduct(product);
        var updateProductRequest = new UpdateProductRequest("Test name", 19.99m, 1);
        Subscribe<ProductPriceUpdatedEvent>();

        // Act
        var response = await HttpClient.PutAsJsonAsync($"/{product.Id}", updateProductRequest);

        // Assert
        response.EnsureSuccessStatusCode();

        Assert.NotEmpty(ReceivedEvents);
        var receivedEvent = ReceivedEvents.First();

        Assert.IsType<ProductPriceUpdatedEvent>(receivedEvent);
        Assert.Equal((receivedEvent as ProductPriceUpdatedEvent).NewPrice, updateProductRequest.Price);
    }

}