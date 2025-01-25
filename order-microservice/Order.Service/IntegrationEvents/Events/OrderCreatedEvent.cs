using ECommerce.Shared.Infrastructure.EventBus;
// using Order.Service.Infrastructure.EventBus;

namespace Order.Service.IntegrationEvents.Events;

public record OrderCreatedEvent(string CustomerId) : Event;
