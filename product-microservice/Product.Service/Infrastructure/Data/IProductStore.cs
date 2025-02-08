namespace Product.Service.Infrastructure.Data;

internal interface IProductStore
{
    Task<Models.Product?> GetById(int id);

    Task CreateProduct(Models.Product product);

    Task UpdateProduct(Models.Product product);
}
