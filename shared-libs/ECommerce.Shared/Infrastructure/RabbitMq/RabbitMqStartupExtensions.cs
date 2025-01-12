using ECommerce.Shared.Infrastructure.EventBus.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Shared.Infrastructure.RabbitMq;
public static class RabbitMqStartupExtensions
{
    public static IServiceCollection AddRabbitMqEventBus(this IServiceCollection services, IConfigurationManager configuration)
    {
        var rabbitMqOptions = new RabbitMqOptions();
        configuration.GetSection(RabbitMqOptions.RabbitMqSectionName).Bind(rabbitMqOptions);

        services.AddSingleton<IRabbitMqConnection>(new RabbitMqConnection(rabbitMqOptions));

        return services;
    }

    public static IServiceCollection AddRabbitMqEventPublisher(this IServiceCollection services)
    {
        services.AddScoped<IEventBus, RabbitMqEventBus>();

        return services;
    }

    // public static IServiceCollection AddRabbitMqSubscriberService(this IServiceCollection services, IConfigurationManager configuration)
    // {
    //     services.Configure<EventBusOptions>(configuration.GetSection(EventBusOptions.EventBusSectionName));

    //     services.AddHostedService<RabbitMqHostedService>();

    //     return services;
    // }
}
