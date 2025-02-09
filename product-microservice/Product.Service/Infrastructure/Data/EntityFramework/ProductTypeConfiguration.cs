using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Product.Service.Models;

namespace Product.Service.Infrastructure.Data.EntityFramework;

internal class ProductTypeConfiguration : IEntityTypeConfiguration<ProductType>
{
    public void Configure(EntityTypeBuilder<ProductType> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(p => p.Type)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasData(
       new ProductType
       {
           Id = 1,
           Type = "Shoes"
       },
       new ProductType
       {
           Id = 2,
           Type = "Shorts"
       });

    }
}
