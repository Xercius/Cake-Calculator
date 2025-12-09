using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Data.Sqlite;
using CakeCalculatorApi.Data;
using CakeCalculatorApi.Models;

namespace CakeCalculatorApi.Tests;

public class RolesEndpointsTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private const int NonExistentRoleId = 99999;

    public RolesEndpointsTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                // Remove the existing DbContext registration
                services.RemoveAll(typeof(DbContextOptions<CakeDbContext>));
                
                // Create a singleton in-memory SQLite connection
                // The connection must stay open for the lifetime of the in-memory database
                var connection = new SqliteConnection("DataSource=:memory:");
                connection.Open();
                
                // Register the connection as a singleton so it persists
                services.AddSingleton(connection);
                
                // Add DbContext with in-memory SQLite
                services.AddDbContext<CakeDbContext>((serviceProvider, options) =>
                {
                    var sqliteConnection = serviceProvider.GetRequiredService<SqliteConnection>();
                    options.UseSqlite(sqliteConnection);
                });
                
                // Build the service provider
                var sp = services.BuildServiceProvider();
                
                // Create a scope to obtain a reference to the database context
                using var scope = sp.CreateScope();
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<CakeDbContext>();
                
                // Ensure the database is created
                db.Database.EnsureCreated();
            });
        });
    }

    [Fact]
    public async Task CreateRole_WithValidData_Returns201AndPersists()
    {
        // Arrange
        var client = _factory.CreateClient();
        var newRole = new Role
        {
            Name = "Test Baker",
            HourlyRate = 25.50m
        };

        // Act
        var response = await client.PostAsJsonAsync("/api/roles", newRole);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        
        var createdRole = await response.Content.ReadFromJsonAsync<Role>();
        Assert.NotNull(createdRole);
        Assert.True(createdRole.Id > 0);
        Assert.Equal("Test Baker", createdRole.Name);
        Assert.Equal(25.50m, createdRole.HourlyRate);
        
        // Verify it was persisted
        var getResponse = await client.GetAsync($"/api/roles/{createdRole.Id}");
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);
    }

    [Fact]
    public async Task CreateRole_WithEmptyName_Returns400()
    {
        // Arrange
        var client = _factory.CreateClient();
        var newRole = new Role
        {
            Name = "",
            HourlyRate = 25.00m
        };

        // Act
        var response = await client.PostAsJsonAsync("/api/roles", newRole);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task CreateRole_WithWhitespaceName_Returns400()
    {
        // Arrange
        var client = _factory.CreateClient();
        var newRole = new Role
        {
            Name = "   ",
            HourlyRate = 25.00m
        };

        // Act
        var response = await client.PostAsJsonAsync("/api/roles", newRole);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task CreateRole_WithNegativeHourlyRate_Returns400()
    {
        // Arrange
        var client = _factory.CreateClient();
        var newRole = new Role
        {
            Name = "Test Baker",
            HourlyRate = -10.00m
        };

        // Act
        var response = await client.PostAsJsonAsync("/api/roles", newRole);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task UpdateRole_WithExistingRole_Returns204AndUpdates()
    {
        // Arrange
        var client = _factory.CreateClient();
        
        // Create a role first
        var newRole = new Role
        {
            Name = "Original Name",
            HourlyRate = 20.00m
        };
        var createResponse = await client.PostAsJsonAsync("/api/roles", newRole);
        var createdRole = await createResponse.Content.ReadFromJsonAsync<Role>();
        Assert.NotNull(createdRole);

        // Act - Update the role
        var updatedRole = new Role
        {
            Name = "Updated Name",
            HourlyRate = 30.00m
        };
        var updateResponse = await client.PutAsJsonAsync($"/api/roles/{createdRole.Id}", updatedRole);

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, updateResponse.StatusCode);
        
        // Verify the update
        var getResponse = await client.GetAsync($"/api/roles/{createdRole.Id}");
        var retrievedRole = await getResponse.Content.ReadFromJsonAsync<Role>();
        Assert.NotNull(retrievedRole);
        Assert.Equal("Updated Name", retrievedRole.Name);
        Assert.Equal(30.00m, retrievedRole.HourlyRate);
    }

    [Fact]
    public async Task UpdateRole_WithNonExistentRole_Returns404()
    {
        // Arrange
        var client = _factory.CreateClient();
        var updatedRole = new Role
        {
            Name = "Updated Name",
            HourlyRate = 30.00m
        };

        // Act
        var response = await client.PutAsJsonAsync($"/api/roles/{NonExistentRoleId}", updatedRole);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task DeleteRole_WithExistingRole_Returns204()
    {
        // Arrange
        var client = _factory.CreateClient();
        
        // Create a role first
        var newRole = new Role
        {
            Name = "Role to Delete",
            HourlyRate = 25.00m
        };
        var createResponse = await client.PostAsJsonAsync("/api/roles", newRole);
        var createdRole = await createResponse.Content.ReadFromJsonAsync<Role>();
        Assert.NotNull(createdRole);

        // Act
        var deleteResponse = await client.DeleteAsync($"/api/roles/{createdRole.Id}");

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);
        
        // Verify it was deleted
        var getResponse = await client.GetAsync($"/api/roles/{createdRole.Id}");
        Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
    }

    [Fact]
    public async Task DeleteRole_WithNonExistentRole_Returns404()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.DeleteAsync($"/api/roles/{NonExistentRoleId}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task UpdateRole_WithEmptyName_Returns400()
    {
        // Arrange
        var client = _factory.CreateClient();
        
        // Create a role first
        var newRole = new Role
        {
            Name = "Original Name",
            HourlyRate = 20.00m
        };
        var createResponse = await client.PostAsJsonAsync("/api/roles", newRole);
        var createdRole = await createResponse.Content.ReadFromJsonAsync<Role>();
        Assert.NotNull(createdRole);

        // Act - Try to update with empty name
        var updatedRole = new Role
        {
            Name = "",
            HourlyRate = 30.00m
        };
        var updateResponse = await client.PutAsJsonAsync($"/api/roles/{createdRole.Id}", updatedRole);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, updateResponse.StatusCode);
    }

    [Fact]
    public async Task UpdateRole_WithNegativeHourlyRate_Returns400()
    {
        // Arrange
        var client = _factory.CreateClient();
        
        // Create a role first
        var newRole = new Role
        {
            Name = "Original Name",
            HourlyRate = 20.00m
        };
        var createResponse = await client.PostAsJsonAsync("/api/roles", newRole);
        var createdRole = await createResponse.Content.ReadFromJsonAsync<Role>();
        Assert.NotNull(createdRole);

        // Act - Try to update with negative hourly rate
        var updatedRole = new Role
        {
            Name = "Updated Name",
            HourlyRate = -15.00m
        };
        var updateResponse = await client.PutAsJsonAsync($"/api/roles/{createdRole.Id}", updatedRole);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, updateResponse.StatusCode);
    }
}
