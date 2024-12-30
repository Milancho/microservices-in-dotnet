using RabbitMQ.Client;

namespace Order.Service.Infrastructure.RabbitMq;

public interface IRabbitMqConnection
{
    IConnection Connection { get; }
}
