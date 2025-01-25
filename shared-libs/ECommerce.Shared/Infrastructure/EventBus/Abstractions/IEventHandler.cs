namespace ECommerce.Shared.Infrastructure.EventBus.Abstractions;
public interface IEventHandler<in TEvent> : IEventHandler
    where TEvent : Event
{
    Task Handle(TEvent @event);
    Task IEventHandler.Handle(Event @event) => Handle((TEvent)@event);
}

public interface IEventHandler
{
    Task Handle(Event @event);
}