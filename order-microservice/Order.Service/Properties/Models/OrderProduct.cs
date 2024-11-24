namespace Order.Service.Models;
internal class OrderProduct
{
    public required string ProductId { get; init; }
    public int Quantity { get; private set; }

    public void AddQuantity(int quantity)
    {
        Quantity += quantity;
    }
}