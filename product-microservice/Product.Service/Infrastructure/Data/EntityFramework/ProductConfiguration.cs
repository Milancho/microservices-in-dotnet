using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Product.Service.Infrastructure.Data.EntityFramework;

internal class ProductConfiguration : IEntityTypeConfiguration<Models.Product>
{
    public void Configure(EntityTypeBuilder<Models.Product> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.Price)
            .IsRequired()
            .HasColumnType("decimal(18,4)");

        builder.HasOne(p => p.ProductType)
            .WithMany();
    }
}
