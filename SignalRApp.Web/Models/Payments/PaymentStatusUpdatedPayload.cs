namespace SignalRApp.Web.Models.Payments;

public sealed class PaymentStatusUpdatedPayload
{
    public string PaymentRequestId { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string? Message { get; set; }
    public DateTime UpdatedAtUtc { get; set; }
}
