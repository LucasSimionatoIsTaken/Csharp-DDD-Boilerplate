using API.Extensions;
using Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", true, true);

builder.Services.AddDependencyInjections(builder.Configuration, builder.Environment);

var app = builder.Build();

app.UseCors();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var providerName = db.Database.ProviderName;

    if (providerName != "Microsoft.EntityFrameworkCore.InMemory")
    {
        db.Database.Migrate();
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
        options.RoutePrefix = string.Empty;
        
        options.InjectStylesheet("/swagger-ui/SwaggerDark.min.css");
    });
    app.MapGet("/swagger-ui/SwaggerDark.min.css", async (CancellationToken ct) =>
    {
        var css = await File.ReadAllBytesAsync("wwwroot/swagger-ui/SwaggerDark.min.css", ct);
        return Results.File(css, "text/css");
    }).ExcludeFromDescription();
}

app.MapControllers();

app.UseHttpsRedirection();

app.UseCors("AnyOrigin");

app.Run();

/// <summary>
/// Program partial class, needed to create tests
/// </summary>
public partial class Program { }