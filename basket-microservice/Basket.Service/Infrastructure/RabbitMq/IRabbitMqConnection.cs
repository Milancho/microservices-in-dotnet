using RabbitMQ.Client;

namespace Basket.Service.Infrastructure.RabbitMq;

public interface IRabbitMqConnection
{
    IConnection Connection { get; }
}
