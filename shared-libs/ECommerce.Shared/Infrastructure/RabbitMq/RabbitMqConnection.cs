using RabbitMQ.Client;

namespace ECommerce.Shared.Infrastructure.RabbitMq;

public class RabbitMqConnection : IDisposable, IRabbitMqConnection
{
    private IConnection? _connection;
    private readonly RabbitMqOptions _options;

    public IConnection Connection => _connection!;

    public RabbitMqConnection(RabbitMqOptions options)
    {
        _options = options;

        InitializeConnection();
    }

    private void InitializeConnection()
    {
        var factory = new ConnectionFactory
        {
            HostName = _options.HostName
        };

        _connection = factory.CreateConnection();
    }

    public void Dispose()
    {
        _connection?.Dispose();
    }
}
