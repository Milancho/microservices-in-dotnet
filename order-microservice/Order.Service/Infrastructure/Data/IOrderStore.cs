namespace Order.Service.Infrastructure.Data;

internal interface IOrderStore
{
    void CreateOrder(Models.Order order);
    Models.Order? GetCustomerOrderById(string customerId, string orderId);
}
