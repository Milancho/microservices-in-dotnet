using ECommerce.Shared.Infrastructure.EventBus.Abstractions;
using ECommerce.Shared.Infrastructure.EventBus;
using System.Text.Json;
using System.Diagnostics;
using OpenTelemetry.Context.Propagation;
using OpenTelemetry;
using ECommerce.Shared.Observability;
using Polly.Retry;
using Polly;
using RabbitMQ.Client.Exceptions;
using System.Net.Sockets;
using Microsoft.Extensions.Options;

namespace ECommerce.Shared.Infrastructure.RabbitMq;
public class RabbitMqEventBus : IEventBus
{
    private const string ExchangeName = "ecommerce-exchange";

    private readonly IRabbitMqConnection _rabbitMqConnection;
    private readonly ActivitySource _activitySource;
    private readonly TextMapPropagator _propagator = Propagators.DefaultTextMapPropagator;
    private readonly ResiliencePipeline _pipeline;

    public RabbitMqEventBus(IRabbitMqConnection rabbitMqConnection, 
        RabbitMqTelemetry rabbitMqTelemetry,
        IOptions<EventBusOptions> options)
    {
        _rabbitMqConnection = rabbitMqConnection;
        _activitySource = rabbitMqTelemetry.ActivitySource;
        _pipeline = CreateResiliencePipeline(options.Value.RetryCount);
    }

    public Task PublishAsync(Event @event)
    {
        var routingKey = @event.GetType().Name;

        return _pipeline.Execute(() =>
        {
            using var channel = _rabbitMqConnection.Connection.CreateModel();

            var activityName = $"{OpenTelemetryMessagingConventions.PublishOperation} {routingKey}";

            using var activity = _activitySource.StartActivity(activityName, ActivityKind.Client);

            ActivityContext activityContextToInject = default;

            if (activity is not null)
            {
                activityContextToInject = activity.Context;
            }

            var properties = channel.CreateBasicProperties();

            _propagator.Inject(new PropagationContext(activityContextToInject, Baggage.Current), properties, (properties, key, value) =>
            {
                properties.Headers ??= new Dictionary<string, object>();
                properties.Headers[key] = value;
            });

            SetActivityContext(activity, routingKey, OpenTelemetryMessagingConventions.PublishOperation);

            channel.ExchangeDeclare(
                exchange: ExchangeName,
                type: "fanout",
                durable: false,
                autoDelete: false,
                null);

            var body = JsonSerializer.SerializeToUtf8Bytes(@event, @event.GetType());

            channel.BasicPublish(
                exchange: ExchangeName,
                routingKey: routingKey,
                mandatory: false,
                basicProperties: properties,
                body: body);

            return Task.CompletedTask;
        });
    }

    private static void SetActivityContext(Activity? activity, string routingKey, string operation)
    {
        if (activity is not null)
        {
            activity.SetTag(OpenTelemetryMessagingConventions.System, "rabbitmq");
            activity.SetTag(OpenTelemetryMessagingConventions.OperationName, operation);
            activity.SetTag(OpenTelemetryMessagingConventions.DestinationName, routingKey);
        }
    }

    private static ResiliencePipeline CreateResiliencePipeline(int retryCount)
    {
        var retryOptions = new RetryStrategyOptions
        {
            ShouldHandle = new PredicateBuilder().Handle<BrokerUnreachableException>().Handle<SocketException>().Handle<AlreadyClosedException>(),
            BackoffType = DelayBackoffType.Exponential,
            MaxRetryAttempts = retryCount
        };

        return new ResiliencePipelineBuilder()
            .AddRetry(retryOptions)
            .Build();
    }
}
