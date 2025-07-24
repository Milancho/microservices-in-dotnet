using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Shared.Infrastructure.Outbox;
internal class OutboxContextFactory : IDesignTimeDbContextFactory<OutboxContext>
{
    public OutboxContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<OutboxContext>();
        optionsBuilder.UseSqlServer();

        return new OutboxContext(optionsBuilder.Options);
    }
}
