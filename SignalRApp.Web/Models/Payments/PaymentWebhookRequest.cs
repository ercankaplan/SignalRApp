namespace SignalRApp.Web.Models.Payments;

public sealed class PaymentWebhookRequest
{
    public string PaymentRequestId { get; set; } = string.Empty;
    public bool Success { get; set; }
}
