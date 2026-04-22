namespace SignalRApp.Web.Models.Payments;

public sealed class CreatePaymentResponse
{
    public string PaymentRequestId { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}
