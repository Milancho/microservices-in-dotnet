using Order.Service.Infrastructure.EventBus.Abstractions;

namespace Order.Service.Infrastructure.RabbitMq;
public static class RabbitMqExtensions
{
    public static void AddRabbitMqEventBus(this IServiceCollection services)
    {
        services.AddSingleton<IRabbitMqConnection>(new RabbitMqConnection());

        services.AddScoped<IEventBus, RabbitMqEventBus>();
    }
}