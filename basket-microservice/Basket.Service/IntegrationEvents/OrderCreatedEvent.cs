using Basket.Service.Infrastructure.EventBus;

namespace Basket.Service.IntegrationEvents;

public record OrderCreatedEvent(string CustomerId) : Event;

