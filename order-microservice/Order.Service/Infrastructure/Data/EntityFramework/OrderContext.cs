using Microsoft.EntityFrameworkCore;
using Order.Service.Models;

namespace Order.Service.Infrastructure.Data.EntityFramework;

internal class OrderContext : DbContext, IOrderStore
{
    public OrderContext(DbContextOptions<OrderContext> options)
    : base(options)
    {
    }

    public DbSet<Models.Order> Orders { get; set; }
    public DbSet<OrderProduct> OrderProducts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new OrderConfiguration());
        modelBuilder.ApplyConfiguration(new OrderProductConfiguration());
    }

    public async Task CreateOrder(Models.Order order)
    {
        Orders.Add(order);

        await SaveChangesAsync();
    }

    public async Task<Models.Order?> GetCustomerOrderById(string customerId, string orderId)
    {
        return await Orders
            .Include(o => o.OrderProducts)
            .FirstOrDefaultAsync(o => o.CustomerId == customerId && o.OrderId.ToString() == orderId);
    }

    void IOrderStore.CreateOrder(Models.Order order)
    {
        throw new NotImplementedException();
    }

    Models.Order? IOrderStore.GetCustomerOrderById(string customerId, string orderId)
    {
        throw new NotImplementedException();
    }
}
