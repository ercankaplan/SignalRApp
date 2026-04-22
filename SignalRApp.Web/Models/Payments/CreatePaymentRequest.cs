namespace SignalRApp.Web.Models.Payments;

public sealed class CreatePaymentRequest
{
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "USD";
}
