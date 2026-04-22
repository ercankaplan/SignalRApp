using SignalRApp.ApiService;
using SignalRApp.ApiService.NotificationHub;
using SignalRApp.ApiService.PaymentHub;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();
builder.Services.AddControllers();
builder.Services.AddSingleton<IPaymentStore, InMemoryPaymentStore>();
builder.Services.AddScoped<IPaymentStatusNotifier, PaymentStatusNotifier>();


builder.Services.AddHostedService<MessageProcessorJob>();

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

app.MapControllers();

app.MapDefaultEndpoints();

app.MapHub<NotificationsHub>("notifications-hub");
app.MapHub<PaymentStatusHub>("/hubs/payment-status");

app.Run();
