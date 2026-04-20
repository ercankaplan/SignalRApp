namespace SignalRApp.ApiService
{
    public interface INotificationsClient
    {
        Task ReceiveNotification(string content);
    }
}
