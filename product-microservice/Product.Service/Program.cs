using ECommerce.Shared.Infrastructure.RabbitMq;
using Product.Service.Endpoints;
using Product.Service.Infrastructure.Data.EntityFramework;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSqlServerDatastore(builder.Configuration);

builder.Services.AddRabbitMqEventBus(builder.Configuration)
    .AddRabbitMqEventPublisher();
    
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MigrateDatabase();
}

app.MapGet("/", () => "Hello World!");
app.Run();

app.RegisterEndpoints();