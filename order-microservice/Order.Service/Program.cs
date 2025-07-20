using Order.Service.Endpoints;
using Order.Service.Infrastructure.Data;
using ECommerce.Shared.Infrastructure.RabbitMq;
using Order.Service.Infrastructure.Data.EntityFramework;
using ECommerce.Shared.Observability;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddRabbitMqEventBus(builder.Configuration)
    .AddRabbitMqEventPublisher();

builder.Services.AddSqlServerDatastore(builder.Configuration);

const string serviceName = "Order";
builder.Services.AddOpenTelemetryTracing(serviceName, builder.Configuration, (traceBuilder) => traceBuilder.WithSqlInstrumentation())
  .AddOpenTelemetryMetrics(serviceName, builder.Services);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MigrateDatabase();
}

app.MapGet("/", () => "Hello World!");

app.RegisterEndpoints();

app.Run();

public partial class Program { }