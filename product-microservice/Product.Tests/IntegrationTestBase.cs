using ECommerce.Shared.Infrastructure.EventBus;
using ECommerce.Shared.Infrastructure.RabbitMq;
using Microsoft.Extensions.DependencyInjection;
using Product.Service.Infrastructure.Data.EntityFramework;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Product.Tests;
public class IntegrationTestBase : IClassFixture<ProductWebApplicationFactory>, IDisposable
{
    private const string QueueName = "product-integration-tests";
    private const string ExchangeName = "ecommerce-exchange";

    private IModel? _model;

    internal readonly ProductContext ProductContext;
    internal readonly HttpClient HttpClient;
    internal readonly IRabbitMqConnection RabbitMqConnection;
    internal List<Event> ReceivedEvents = [];

    public IntegrationTestBase(ProductWebApplicationFactory webApplicationFactory)
    {
        var scope = webApplicationFactory.Services.CreateScope();

        ProductContext = scope.ServiceProvider.GetRequiredService<ProductContext>();
        HttpClient = webApplicationFactory.CreateClient();
        RabbitMqConnection = scope.ServiceProvider.GetRequiredService<IRabbitMqConnection>();
    }

    public void Subscribe<TEvent>() where TEvent : Event
    {
        _model = RabbitMqConnection.Connection.CreateModel();
        _model.ExchangeDeclare(ExchangeName, "fanout", durable: false, autoDelete: false, null);
        _model.QueueDeclare(QueueName, durable: false, exclusive: false, autoDelete: false, null);

        EventingBasicConsumer eventingBasicConsumer = new(_model);
        eventingBasicConsumer.Received += (sender, eventArgs) =>
        {
            var body = Encoding.UTF8.GetString(eventArgs.Body.Span);
            var @event = JsonSerializer.Deserialize<TEvent>(body);

            if (@event is not null)
            {
                ReceivedEvents.Add(@event);
            }
        };

        _model.BasicConsume(QueueName, true, eventingBasicConsumer);
        _model.QueueBind(QueueName, ExchangeName, typeof(TEvent).Name);
    }

    public void Dispose()
    {
        if (_model is not null)
        {
            _model.QueueDelete(QueueName);
            _model.ExchangeDelete(ExchangeName);
        }
        RabbitMqConnection.Connection.Dispose();
    }
}
