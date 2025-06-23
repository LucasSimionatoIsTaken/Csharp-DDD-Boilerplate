using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;

namespace IntegrationTests.config;

public class TestServer : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseSetting("https_port", "443");
        builder.ConfigureAppConfiguration((_, config) =>
        {
            var root = Directory.GetCurrentDirectory();
            var fileProvider = new PhysicalFileProvider(root);
            config.AddJsonFile(fileProvider, "testsettings.json", false, false);
        });

        // builder.ConfigureLogging 
        // builder.ConfigureTestServices(services =>
        // {
        // });
    }
}