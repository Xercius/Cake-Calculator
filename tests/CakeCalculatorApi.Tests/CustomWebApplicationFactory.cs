using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using CakeCalculatorApi.Data;

namespace CakeCalculatorApi.Tests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>, IDisposable
{
    private SqliteConnection? _connection;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Remove the existing DbContext registration
            services.RemoveAll(typeof(DbContextOptions<CakeDbContext>));

            // Create a persistent in-memory SQLite connection
            // The connection must stay open for the lifetime of the in-memory database
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();

            // Register the connection as a singleton so it persists
            services.AddSingleton(_connection);

            // Add DbContext with in-memory SQLite
            services.AddDbContext<CakeDbContext>((serviceProvider, options) =>
            {
                var sqliteConnection = serviceProvider.GetRequiredService<SqliteConnection>();
                options.UseSqlite(sqliteConnection);
            });
        });
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _connection?.Close();
            _connection?.Dispose();
        }
        base.Dispose(disposing);
    }
}
