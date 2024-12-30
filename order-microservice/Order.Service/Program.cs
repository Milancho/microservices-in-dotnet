using Order.Service.Endpoints;
using Order.Service.Infrastructure.Data;
using Order.Service.Infrastructure.RabbitMq;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<IOrderStore, InMemoryOrderStore>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddRabbitMqEventBus();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.RegisterEndpoints();

app.Run();

