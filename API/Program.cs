using API.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", true, true);

builder.Services.AddDependencyInjections(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
        options.RoutePrefix = string.Empty;
        
        options.InjectStylesheet("/swagger-ui/swaggerDark.min.css");
    });
    app.MapGet("/swagger-ui/swaggerDark.min.css", async (CancellationToken ct) =>
    {
        var css = await File.ReadAllBytesAsync("wwwroot/swagger-ui/swaggerDark.min.css", ct);
        return Results.File(css, "text/css");
    }).ExcludeFromDescription();
}

app.MapControllers();

app.UseHttpsRedirection();

app.Run();