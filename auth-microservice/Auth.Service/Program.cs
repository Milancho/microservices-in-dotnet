using Auth.Service.Infrastructure.Data.EntityFramework;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSqlServerDatastore(builder.Configuration);



var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MigrateDatabase();
}

// app.RegisterEndpoints();

app.UseHttpsRedirection();

app.Run();
