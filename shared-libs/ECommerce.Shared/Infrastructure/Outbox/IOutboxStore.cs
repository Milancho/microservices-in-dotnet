using ECommerce.Shared.Infrastructure.EventBus;
using ECommerce.Shared.Infrastructure.Outbox.Models;
using Microsoft.EntityFrameworkCore.Storage;

namespace ECommerce.Shared.Infrastructure.Outbox;
public interface IOutboxStore
{
    Task AddOutboxEvent<T>(T @event) where T : Event;
    Task<List<OutboxEvent>> GetUnpublishedOutboxEvents();
    Task MarkOutboxEventAsPublished(Guid outboxEventId);
    IExecutionStrategy CreateExecutionStrategy();
}