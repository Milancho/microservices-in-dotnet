using Basket.Service.Models;

namespace Basket.Service.Infrastructure.Data;

internal class InMemoryBasketStore : IBasketStore
{
   private static readonly Dictionary<string, CustomerBasket> Baskets = [];
  
   public CustomerBasket GetBasketByCustomerId(string customerId) =>
       Baskets.TryGetValue(customerId, out var value) ? value 
       : new CustomerBasket { CustomerId = customerId };
}