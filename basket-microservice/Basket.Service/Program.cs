using Basket.Service.Endpoints;
using Basket.Service.Infrastructure.Data;
using Basket.Service.Infrastructure.RabbitMq;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IBasketStore, InMemoryBasketStore>();
builder.Services.AddRabbitMqEventBus(builder.Configuration);
builder.Services.AddHostedService<RabbitMqHostedService>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.MapGet("/test", () => "Test !");

app.RegisterEndpoints();

app.Run();



