using ECommerce.Shared.Infrastructure.Outbox.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Shared.Infrastructure.Outbox;
public class OutboxEventConfiguration : IEntityTypeConfiguration<OutboxEvent>
{
    public void Configure(EntityTypeBuilder<OutboxEvent> builder)
    {
        builder.HasKey(o => o.Id);

        builder.Property(o => o.EventType)
            .IsRequired();

        builder.Property(o => o.Data)
            .IsRequired();
    }
}
