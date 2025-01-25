using ECommerce.Shared.Infrastructure.EventBus.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Shared.Infrastructure.EventBus;
public static class EventBusHandlerExtensions
{
    public static IServiceCollection AddEventHandler<TEvent, THandler>(this IServiceCollection serviceCollection)
    where TEvent : Event
    where THandler : class, IEventHandler<TEvent>
    {
        serviceCollection.AddKeyedTransient<IEventHandler, THandler>(typeof(TEvent));

        serviceCollection.Configure<EventHandlerRegistration>(o =>
        {
            o.EventTypes[typeof(TEvent).Name] = typeof(TEvent);
        });

        return serviceCollection;
    }
}