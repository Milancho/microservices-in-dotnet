using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace ECommerce.Shared.Observability;

public static class OpenTelemetryStartupExtensions
{
    public static OpenTelemetryBuilder AddOpenTelemetryTracing(this IServiceCollection services, 
        string serviceName,
        Action<TracerProviderBuilder>? customTracing = null)
    {
        return services.AddOpenTelemetry()
            .ConfigureResource(r => r
                .AddService(serviceName))
            .WithTracing(builder =>
            {
                builder
                    .AddConsoleExporter()
                    .AddAspNetCoreInstrumentation();

                customTracing?.Invoke(builder);
            });
    }

    public static TracerProviderBuilder WithSqlInstrumentation(this TracerProviderBuilder builder) => 
        builder.AddSqlClientInstrumentation();
}