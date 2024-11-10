internal class CustomerBasket
{
    private readonly HashSet<BasketProduct> _products = [];

    public IEnumerable<BasketProduct> Products => _products;
    public required string CustomerId { get; init; }

    public void AddBasketProduct(BasketProduct basketProduct)
    {
        var existingProduct = _products.FirstOrDefault(i =>
            i.ProductId.Equals(basketProduct.ProductId));
        if (existingProduct is null)
        {
            _products.Add(basketProduct);
        }
        else
        {
            _products.Remove(existingProduct);

            _products.Add(basketProduct);
        }
    }

    public void RemoveBasketProduct(string productId) =>
            _products.RemoveWhere(r => r.ProductId.Equals(productId));

}