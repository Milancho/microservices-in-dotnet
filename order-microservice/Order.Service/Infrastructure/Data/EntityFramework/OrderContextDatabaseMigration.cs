using Microsoft.EntityFrameworkCore;

namespace Order.Service.Infrastructure.Data.EntityFramework;

internal static class OrderContextDatabaseMigration
{
    public static void MigrateDatabase(this WebApplication webApp)
    {
        using var scope = webApp.Services.CreateScope();
        using var orderContext = scope.ServiceProvider.GetRequiredService<OrderContext>();
        orderContext.Database.Migrate();
    }
}
