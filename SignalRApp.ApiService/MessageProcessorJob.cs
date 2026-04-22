
using SignalRApp.ApiService.NotificationHub;

namespace SignalRApp.ApiService
{
    public class MessageProcessorJob(ILogger<MessageProcessorJob> _logger, IHubContext<NotificationsHub, INotificationsClient> context) : BackgroundService
    {

        private static readonly TimeSpan PollInterval = TimeSpan.FromSeconds(5);

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Outbox processor started.");

            using var timer = new PeriodicTimer(PollInterval);

            while (!stoppingToken.IsCancellationRequested && await timer.WaitForNextTickAsync(stoppingToken))
            {
                await context.Clients.All.ReceiveNotification($"Test Message ({DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")})");
            }
           
        }
    }
}
