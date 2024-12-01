using Order.Service.Endpoints;
using Order.Service.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<IOrderStore, InMemoryOrderStore>();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.RegisterEndpoints();

app.Run();

