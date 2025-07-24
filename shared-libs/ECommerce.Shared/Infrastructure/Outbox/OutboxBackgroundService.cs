using ECommerce.Shared.Infrastructure.EventBus.Abstractions;
using ECommerce.Shared.Infrastructure.EventBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text.Json;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

namespace ECommerce.Shared.Infrastructure.Outbox;
public class OutboxBackgroundService : BackgroundService
{
    private readonly TimeSpan _period;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<OutboxBackgroundService> _logger;

    public OutboxBackgroundService(IServiceScopeFactory serviceScopeFactory,
        IOptions<OutboxOptions> outboxOptions,
        ILogger<OutboxBackgroundService> logger)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
        _period = TimeSpan.FromSeconds(outboxOptions.Value.PublishIntervalInSeconds);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using PeriodicTimer timer = new(_period);

        while (!stoppingToken.IsCancellationRequested && await timer.WaitForNextTickAsync(stoppingToken))
        {
            _logger.LogInformation("Retrieving unpublished outbox events");

            using var serviceScope = _serviceScopeFactory.CreateScope();
            var outboxStore = serviceScope.ServiceProvider.GetRequiredService<IOutboxStore>();
            var eventBus = serviceScope.ServiceProvider.GetRequiredService<IEventBus>();

            var unpublishedEvents = await outboxStore.GetUnpublishedOutboxEvents();

            foreach (var unpublishedEvent in unpublishedEvents)
            {
                var @event = JsonSerializer.Deserialize(unpublishedEvent.Data, Type.GetType(unpublishedEvent.EventType)) as Event;

                await eventBus.PublishAsync(@event);

                await outboxStore.MarkOutboxEventAsPublished(unpublishedEvent.Id);
            }

            if (unpublishedEvents.Any()) 
            {
                _logger.LogInformation("Unpublished outbox events sent");
            }
            else
            {
                _logger.LogInformation("No unpublished events to send");
            }

        }
    }
}
