using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WarehouseManagement.Infrastructure.Persistence;

namespace WarehouseManagement.Tests.Integration;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private const string TestDatabaseName = "WarehouseManagementTestDb";

    private const string TestConnectionString =
        "Server=localhost;Database=WarehouseManagementTestDb;" +
        "Integrated Security=True;TrustServerCertificate=True";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureAppConfiguration((context, configuration) =>
        {
            configuration.AddInMemoryCollection(
                new Dictionary<string, string?>
                {
                    ["ConnectionStrings:WarehouseDb"] = TestConnectionString
                });
        });
    }

    public async Task InitializeDatabaseAsync()
    {
        using var scope = Services.CreateScope();

        var dbContext = scope.ServiceProvider
            .GetRequiredService<WarehouseDbContext>();

        var connectionString = dbContext.Database
            .GetDbConnection()
            .ConnectionString;

        var databaseName = new SqlConnectionStringBuilder(
            connectionString).InitialCatalog;

        if (!string.Equals(
                databaseName,
                TestDatabaseName,
                StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException(
                "Integration tests must use the test database.");
        }

        await dbContext.Database.MigrateAsync();
    }
}