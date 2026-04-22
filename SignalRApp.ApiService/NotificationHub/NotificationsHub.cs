
using Microsoft.AspNetCore.SignalR;

namespace SignalRApp.ApiService.NotificationHub
{

    /// <summary>
    /// Clients - used to invoke methods on the clients connected to this hub
    /// Groups - an abstraction for adding and removing connections from groups
    /// Context - used for accessing information about the hub caller connection
    /// </summary>
    public sealed class NotificationsHubX : Hub
    {
        public async Task SendNotification(string content)
        {
            // ISingleClientProxy Client(string connectionId) 

            await Clients.All.SendAsync("NotificationEndpoint", content);
        }
    }

    public sealed class NotificationsHub : Hub<INotificationsClient>
    {
        public async Task SendNotification(string content)
        {
            await Clients.All.ReceiveNotification(content);
        }
    }
}
