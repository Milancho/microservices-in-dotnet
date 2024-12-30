using System.Text.Json;
using Order.Service.Infrastructure.EventBus;
using Order.Service.Infrastructure.EventBus.Abstractions;

namespace Order.Service.Infrastructure.RabbitMq;

public class RabbitMqEventBus : IEventBus
{
    private readonly IRabbitMqConnection _rabbitMqConnection;

    public RabbitMqEventBus(IRabbitMqConnection rabbitMqConnection)
    {
        _rabbitMqConnection = rabbitMqConnection;
    }

    public Task PublishAsync(Event @event)
    {
        var routingKey = @event.GetType().Name;

        using var channel = _rabbitMqConnection.Connection.CreateModel();

        channel.QueueDeclare(queue: routingKey, false, false, false, null);

        var body = JsonSerializer.SerializeToUtf8Bytes(@event, @event.GetType());

        channel.BasicPublish(
            exchange: string.Empty,
            routingKey: routingKey,
            mandatory: false,
            basicProperties: null,
            body: body);

        return Task.CompletedTask;
    }
}