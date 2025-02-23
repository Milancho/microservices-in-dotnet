namespace Basket.Service.Models;

internal record BasketProduct(string ProductId, string ProductName, decimal ProductPrice, int Quantity = 1);