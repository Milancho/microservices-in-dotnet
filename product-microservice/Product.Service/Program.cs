using Product.Service.Infrastructure.Data.EntityFramework;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSqlServerDatastore(builder.Configuration);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.Run();
