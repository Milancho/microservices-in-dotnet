using Order.Service.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

builder.Services.AddScoped<IOrderStore, InMemoryOrderStore>();

app.MapGet("/", () => "Hello World!");

app.Run();
