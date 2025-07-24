using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Shared.Infrastructure.Outbox;
public static class OutboxStartupExtensions
{
    public static void AddOutbox(this IServiceCollection services, IConfigurationManager configuration)
    {
        var outboxOptions = new OutboxOptions();
        configuration.GetSection(OutboxOptions.OutboxSectionName).Bind(outboxOptions);
        services.AddSingleton(outboxOptions);

        services.AddDbContext<OutboxContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("Default"),
            sqlServerOptionsAction: sqlOptions =>
            {
                sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(40),
                    errorNumbersToAdd: [0]);
            }));

        services.AddScoped<IOutboxStore, OutboxContext>();
        services.AddHostedService<OutboxBackgroundService>();
    }

    public static void ApplyOutboxMigrations(this WebApplication webApp)
    {
        using var scope = webApp.Services.CreateScope();
        using var productContext = scope.ServiceProvider.GetRequiredService<OutboxContext>();
        productContext.Database.Migrate();
    }
}
