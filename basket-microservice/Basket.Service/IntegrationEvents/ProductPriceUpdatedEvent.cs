using ECommerce.Shared.Infrastructure.EventBus;

namespace Basket.Service.IntegrationEvents;

public record ProductPriceUpdatedEvent(int ProductId, decimal NewPrice) : Event;
