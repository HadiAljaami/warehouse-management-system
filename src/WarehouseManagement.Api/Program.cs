using Microsoft.EntityFrameworkCore;
using WarehouseManagement.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<WarehouseDbContext>((serviceProvider, options) =>
{
    var configuration = serviceProvider
        .GetRequiredService<IConfiguration>();

    var connectionString = configuration.GetConnectionString("WarehouseDb")
        ?? throw new InvalidOperationException(
            "Connection string 'WarehouseDb' is missing.");

    options.UseSqlServer(connectionString);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }
