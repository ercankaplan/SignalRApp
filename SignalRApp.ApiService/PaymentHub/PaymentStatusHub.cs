namespace SignalRApp.ApiService.PaymentHub
{
    public class PaymentStatusHub : Hub
    {
        public async Task SubscribeToPayment(string paymentRequestId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, GetGroupName(paymentRequestId));
        }

        public async Task UnsubscribeFromPayment(string paymentRequestId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, GetGroupName(paymentRequestId));
        }

        public static string GetGroupName(string paymentRequestId)
            => $"payment:{paymentRequestId}";
    }

}
