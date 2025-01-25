
using ECommerce.Shared.Infrastructure.EventBus;

namespace Basket.Service.IntegrationEvents;

public record OrderCreatedEvent(string CustomerId) : Event;

