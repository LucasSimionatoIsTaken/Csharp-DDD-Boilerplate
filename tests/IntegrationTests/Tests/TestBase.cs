using Core;
using Core.Enums;
using Infrastructure.Contexts;
using Infrastructure.Extensions;
using IntegrationTests.config;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace IntegrationTests.Tests;

[Collection("Integration Tests")]
public class TestBase : IAsyncLifetime
{
    private readonly TestServer _server;
    private AsyncServiceScope _scope;
    protected IServiceProvider _services;
    protected HttpClient _client;

    public TestBase(TestServer server)
    {
        _server = server;
    }

    public async Task InitializeAsync()
    {
        _client = _server.CreateClient(new WebApplicationFactoryClientOptions
        {
            BaseAddress = new Uri("https://localhost/")
        });
        _scope = _server.Services.CreateAsyncScope();
        _services = _scope.ServiceProvider;

        await ClearDatabaseAsync();
        await SeedDataAsync();
    }

    private Task ClearDatabaseAsync()
    {
        var context = _services.GetRequiredService<AppDbContext>();

        return context.Database.ExecuteSqlRawAsync(@"
            DELETE FROM dbo.Users;
            ");
    }

    private async Task SeedDataAsync()
    {
        var context = _services.GetRequiredService<AppDbContext>();

        context.Users.AddRange(
            new User(
                "Admin user",
                TestData.AdminEmail,
                TestData.AdminPassword.HashPassword(),
                UserRole.Admin
            )
            );

        await context.SaveChangesAsync();
    }

    public async Task DisposeAsync()
    {
        await _scope.DisposeAsync();
    }
}