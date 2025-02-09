using Microsoft.EntityFrameworkCore;
using Product.Service.Models;

namespace Product.Service.Infrastructure.Data.EntityFramework;

internal static class ProductContextSeed
{
    public static void MigrateDatabase(this WebApplication webApp)
    {
        using var scope = webApp.Services.CreateScope();
        using var productContext = scope.ServiceProvider.GetRequiredService<ProductContext>();
        productContext.Database.Migrate();
    }
}