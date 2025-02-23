using Basket.Service.Models;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Basket.Service.Infrastructure.Data.Redis;

internal class RedisBasketStore : IBasketStore
{
    private readonly IDistributedCache _cache;
    private readonly DistributedCacheEntryOptions _cacheEntryOptions;

    public RedisBasketStore(IDistributedCache cache)
    {
        _cache = cache;
        _cacheEntryOptions = new DistributedCacheEntryOptions
        {
            SlidingExpiration = TimeSpan.FromHours(24)
        };
    }

    public async Task CreateCustomerBasket(CustomerBasket customerBasket)
    {
        var serializedBasketProducts = JsonSerializer.Serialize(
            new CustomerBasketCacheModel(customerBasket.Products.ToList()));
        await _cache.SetStringAsync(customerBasket.CustomerId,
            serializedBasketProducts, _cacheEntryOptions);
    }

    public async Task DeleteCustomerBasket(string customerId) => await _cache.RemoveAsync(customerId);

    public async Task<CustomerBasket> GetBasketByCustomerId(string customerId)
    {
        var cachedBasketProducts = await _cache.GetStringAsync(customerId);

        if (cachedBasketProducts is null)
        {
            return new CustomerBasket { CustomerId = customerId };
        }

        var deserializedProducts =
            JsonSerializer.Deserialize<CustomerBasketCacheModel>(cachedBasketProducts);

        var customerBasket = new CustomerBasket { CustomerId = customerId };
        foreach (var product in deserializedProducts.Products)
        {
            customerBasket.AddBasketProduct(product);
        }

        return customerBasket;
    }

    public async Task UpdateCustomerBasket(CustomerBasket customerBasket)
    {
        var cachedBasketProducts = await _cache.GetStringAsync(customerBasket.CustomerId);
        if (cachedBasketProducts is not null)
        {
            var serializedBasketProducts = JsonSerializer.Serialize(
                new CustomerBasketCacheModel(customerBasket.Products.ToList()));
            await _cache.SetStringAsync(customerBasket.CustomerId, serializedBasketProducts, _cacheEntryOptions);
        }
    }
}