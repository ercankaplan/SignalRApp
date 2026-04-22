namespace SignalRApp.ApiService.PaymentHub
{
    public interface IPaymentStatusNotifier
    {
        Task NotifyAsync(string paymentRequestId, string status, string? message = null);
    }

    public class PaymentStatusNotifier : IPaymentStatusNotifier
    {
        private readonly IHubContext<PaymentStatusHub> _hubContext;

        public PaymentStatusNotifier(IHubContext<PaymentStatusHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task NotifyAsync(string paymentRequestId, string status, string? message = null)
        {
            var payload = new PaymentStatusUpdatedMessage
            {
                PaymentRequestId = paymentRequestId,
                Status = status,
                Message = message,
                UpdatedAtUtc = DateTime.UtcNow
            };

            await _hubContext.Clients
                .Group(PaymentStatusHub.GetGroupName(paymentRequestId))
                .SendAsync("PaymentStatusUpdated", payload);
        }
    }

    public class PaymentStatusUpdatedMessage
    {
        public string PaymentRequestId { get; set; } = default!;
        public string Status { get; set; } = default!;
        public string? Message { get; set; }
        public DateTime UpdatedAtUtc { get; set; }
    }

}
