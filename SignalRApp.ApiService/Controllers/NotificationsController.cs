using Microsoft.AspNetCore.Mvc;
using SignalRApp.ApiService.NotificationHub;

namespace SignalRApp.ApiService.Controllers;

[ApiController]
[Route("notifications")]
public class NotificationsController(IHubContext<NotificationsHub, INotificationsClient> hubContext) : ControllerBase
{
    [HttpPost("all")]
    public async Task<IActionResult> SendToAll([FromQuery] string content)
    {
        await hubContext.Clients.All.ReceiveNotification(content);
        return NoContent();
    }

    [HttpPost("user")]
    public async Task<IActionResult> SendToUser([FromQuery] string userId, [FromQuery] string content)
    {
        await hubContext.Clients.User(userId).ReceiveNotification(content);
        return NoContent();
    }
}
