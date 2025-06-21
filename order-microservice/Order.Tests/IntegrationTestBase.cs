using ECommerce.Shared.Infrastructure.EventBus;
using ECommerce.Shared.Infrastructure.RabbitMq;
using Microsoft.Extensions.DependencyInjection;
using Order.Service.ApiModels;
using Order.Service.Infrastructure.Data.EntityFramework;
using Order.Tests;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace Order.Tests;

public class IntegrationTestBase : IClassFixture<OrderWebApplicationFactory>
{

    private const string QueueName = "order-integration-tests";
    private const string ExchangeName = "ecommerce-exchange";
    private IModel? _model;

    internal readonly OrderContext OrderContext;
    internal readonly HttpClient HttpClient;
    internal readonly IRabbitMqConnection RabbitMqConnection;
    internal List<Event> ReceivedEvents = [];
    public IntegrationTestBase(OrderWebApplicationFactory webApplicationFactory)
    {
        var scope = webApplicationFactory.Services.CreateScope();

        OrderContext = scope.ServiceProvider.GetRequiredService<OrderContext>();
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