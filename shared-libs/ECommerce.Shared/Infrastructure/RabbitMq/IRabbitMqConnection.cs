using RabbitMQ.Client;

namespace ECommerce.Shared.Infrastructure.RabbitMq;
public interface IRabbitMqConnection
{
    IConnection Connection { get; }
}
