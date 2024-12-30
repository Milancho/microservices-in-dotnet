using RabbitMQ.Client;

namespace Order.Service.Infrastructure.RabbitMq;
public class RabbitMqConnection : IDisposable, IRabbitMqConnection
{
    private IConnection? _connection;

    public IConnection Connection => _connection!;

    public RabbitMqConnection()
    {
        InitializeConnection();
    }

    private void InitializeConnection()
    {
        var factory = new ConnectionFactory
        {
            HostName = "localhost"
        };

        _connection = factory.CreateConnection();
    }

    public void Dispose()
    {
        _connection?.Dispose();
    }
}