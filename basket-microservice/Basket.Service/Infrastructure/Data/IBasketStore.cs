using Basket.Service.Models;

namespace Basket.Service.Infrastructure.Data;
internal interface IBasketStore
{
   CustomerBasket GetBasketByCustomerId(string customerId);
   void CreateCustomerBasket(CustomerBasket customerBasket);
}