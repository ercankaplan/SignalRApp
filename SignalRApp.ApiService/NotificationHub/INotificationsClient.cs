namespace SignalRApp.ApiService.NotificationHub
{
    public interface INotificationsClient
    {
        Task ReceiveNotification(string content);
    }
}
