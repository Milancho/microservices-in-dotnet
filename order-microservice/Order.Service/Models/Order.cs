namespace Order.Service.Models;

internal class Order
{
    private readonly List<OrderProduct> _orderProducts = [];

    public IReadOnlyCollection<OrderProduct> OrderProducts => _orderProducts.AsReadOnly();

    public required string CustomerId { get; init; }

    public Guid OrderId { get; init; }
    public DateTime OrderDate { get; private set; }

    public Order()
    {
        OrderId = Guid.NewGuid();
        OrderDate = DateTime.UtcNow;
    }

    public void AddOrderProduct(string productId, int quantity)
    {
        var existingOrderForProduct = _orderProducts.SingleOrDefault(o => o.ProductId == productId);

        if (existingOrderForProduct is not null)
        {
            existingOrderForProduct.AddQuantity(quantity);
        }
        else
        {
            var orderProduct = new OrderProduct { ProductId = productId, OrderId = OrderId };
            orderProduct.AddQuantity(quantity);

            _orderProducts.Add(orderProduct);
        }
    }
}
