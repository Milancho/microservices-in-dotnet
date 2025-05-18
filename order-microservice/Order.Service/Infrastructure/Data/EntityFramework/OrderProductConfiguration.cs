using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Order.Service.Models;

namespace Order.Service.Infrastructure.Data.EntityFramework;

internal class OrderProductConfiguration : IEntityTypeConfiguration<OrderProduct>
{
    public void Configure(EntityTypeBuilder<OrderProduct> builder)
    {
        builder.HasKey(p => new { p.OrderId, p.ProductId });

        builder.Property(p => p.ProductId)
            .IsRequired();

        builder.Property(p => p.Quantity)
            .IsRequired();
    }
}
