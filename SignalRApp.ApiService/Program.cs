using Microsoft.AspNetCore.Mvc;
using SignalRApp.ApiService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();

builder.Services.AddHostedService<MessageProcessor>();

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

string[] summaries = ["Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"];

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.MapPost("notifications/all", async ([FromQuery]string content,IHubContext<NotificationsHub, INotificationsClient> context) =>
{
    await context.Clients.All.ReceiveNotification(content);

    return Results.NoContent();
});

app.MapPost("notifications/user", async (string userId,string content,IHubContext<NotificationsHub, INotificationsClient> context) =>
{
    await context.Clients.User(userId).ReceiveNotification(content);

    return Results.NoContent();
});

app.MapDefaultEndpoints();

app.MapHub<NotificationsHub>("notifications-hub");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
