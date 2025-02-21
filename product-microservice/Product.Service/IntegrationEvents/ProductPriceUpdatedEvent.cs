using ECommerce.Shared.Infrastructure.EventBus;

namespace Product.Service.IntegrationEvents;

public record ProductPriceUpdatedEvent(int ProductId, decimal NewPrice) : Event;
