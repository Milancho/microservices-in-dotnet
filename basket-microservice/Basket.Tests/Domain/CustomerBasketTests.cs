using Basket.Service.Models;
namespace Basket.Tests.Domain;

public class CustomerBasketTests
{
    [Fact]
    public void GivenAnEmptyCustomerBasket_WhenCallingAddBasketProduct_ThenProductAddedToBasket()
    {
        // Arrange
        var product = new BasketProduct("1", "Test Name", 9.99M);
        var customerBasket = new CustomerBasket { CustomerId = "1" };
        // Act
        customerBasket.AddBasketProduct(product);
        // Assert
        Assert.Contains(product, customerBasket.Products);
    }

    [Fact]
    public void GivenCustomerBasketWithProduct_WhenCallingAddBasketProductWithExistingProduct_ThenBasketUpdated()
    {
        // Arrange
        var product = new BasketProduct("1", "Test Name", 9.99M);
        var customerBasket = new CustomerBasket { CustomerId = "1" };
        customerBasket.AddBasketProduct(product);
        var updatedProduct = product with
        {
            Quantity = 2
        };
        // Act
        customerBasket.AddBasketProduct(updatedProduct);
        // Assert
        Assert.Contains(updatedProduct, customerBasket.Products);
        Assert.Equal(updatedProduct.Quantity, customerBasket.Products.First().Quantity);
    }

    [Fact]
    public void GivenCustomerBasketWithProduct_WhenCallingRemoveBasketProduct_ThenProductSuccessfullyRemoved()
    {
        // Arrange
        var product = new BasketProduct("1", "Test Name", 9.99M);
        var customerBasket = new CustomerBasket { CustomerId = "1" };
        customerBasket.AddBasketProduct(product);
        // Act
        customerBasket.RemoveBasketProduct("1");
        // Assert
        Assert.Empty(customerBasket.Products);
    }

    [Fact]
    public void GivenCustomerBasket_WhenAddingProduct_ThenBasketTotalCalculatedCorrectly()
    {
        // Arrange
        const decimal basketTotal = 19.98M;
        // Act
        var product = new BasketProduct("1", "Test Name", 9.99M, 2);
        var customerBasket = new CustomerBasket { CustomerId = "1" };
        customerBasket.AddBasketProduct(product);
        // Assert
        Assert.Equal(basketTotal, customerBasket.BasketTotal);
    }

}
