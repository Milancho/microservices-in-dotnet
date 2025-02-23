using Basket.Service.Models;

namespace Basket.Service.Infrastructure.Data;

internal record CustomerBasketCacheModel(List<BasketProduct> Products);
