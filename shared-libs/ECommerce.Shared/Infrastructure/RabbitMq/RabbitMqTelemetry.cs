using System.Diagnostics;

namespace ECommerce.Shared.Infrastructure.RabbitMq;
public class RabbitMqTelemetry
{
    public const string ActivitySourceName = "RabbitMQEventBus";

    public ActivitySource ActivitySource { get; } = new(ActivitySourceName);
}
