namespace SignalRApp.ApiService.PaymentHub
{
    public interface IPaymentStore
    {
        PaymentRecord? Get(string paymentRequestId);
        void Save(PaymentRecord record);
    }

    public class InMemoryPaymentStore : IPaymentStore
    {
        private static readonly Dictionary<string, PaymentRecord> _payments = new();

        public PaymentRecord? Get(string paymentRequestId)
        {
            _payments.TryGetValue(paymentRequestId, out var record);
            return record;
        }

        public void Save(PaymentRecord record)
        {
            _payments[record.PaymentRequestId] = record;
        }
    }

    public class PaymentRecord
    {
        public string PaymentRequestId { get; set; } = default!;
        public string Status { get; set; } = default!; // Pending, Succeeded, Failed
        public string? Message { get; set; }
    }

}
