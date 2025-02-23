using Basket.Service.Endpoints;
using Basket.Service.Infrastructure.Data;
using Basket.Service.Infrastructure.Data.Redis;
using Basket.Service.IntegrationEvents;
using Basket.Service.IntegrationEvents.EventHandlers;
using ECommerce.Shared.Infrastructure.EventBus;
using ECommerce.Shared.Infrastructure.RabbitMq;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IBasketStore, RedisBasketStore>();
builder.Services.AddRabbitMqEventBus(builder.Configuration)
    .AddRabbitMqSubscriberService(builder.Configuration)
    .AddEventHandler<OrderCreatedEvent, OrderCreatedEventHandler>()
    .AddEventHandler<ProductPriceUpdatedEvent, ProductPriceUpdatedEventHandler>();
    
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddRedisCache(builder.Configuration);

var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.MapGet("/test", () => "Test !");

app.RegisterEndpoints();

app.Run();
