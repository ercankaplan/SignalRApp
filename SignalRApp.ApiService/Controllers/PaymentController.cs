using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SignalRApp.ApiService.PaymentHub;

namespace SignalRApp.ApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentStore _paymentStore;
        private readonly IPaymentStatusNotifier _notifier;

        public PaymentController(IPaymentStore paymentStore, IPaymentStatusNotifier notifier)
        {
            _paymentStore = paymentStore;
            _notifier = notifier;
        }

        [HttpPost]
        public IActionResult CreatePayment([FromBody] CreatePaymentRequest request)
        {
            var paymentRequestId = Guid.NewGuid().ToString("N");

            _paymentStore.Save(new PaymentRecord
            {
                PaymentRequestId = paymentRequestId,
                Status = "Pending",
                Message = "Payment created, waiting for provider callback."
            });

            return Ok(new
            {
                paymentRequestId,
                status = "Pending"
            });
        }

        [HttpGet("{paymentRequestId}")]
        public IActionResult GetStatus(string paymentRequestId)
        {
            var payment = _paymentStore.Get(paymentRequestId);
            if (payment is null)
                return NotFound();

            return Ok(payment);
        }

        // Simulated webhook from payment provider
        [HttpPost("webhook")]
        public async Task<IActionResult> PaymentWebhook([FromBody] PaymentWebhookRequest request)
        {
            var payment = _paymentStore.Get(request.PaymentRequestId);
            if (payment is null)
                return NotFound();

            payment.Status = request.Success ? "Succeeded" : "Failed";
            payment.Message = request.Success
                ? "Payment completed successfully."
                : "Payment failed.";

            _paymentStore.Save(payment);

            await _notifier.NotifyAsync(payment.PaymentRequestId, payment.Status, payment.Message);

            return Ok();
        }
    }

    public class CreatePaymentRequest
    {
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "USD";
    }

    public class PaymentWebhookRequest
    {
        public string PaymentRequestId { get; set; } = default!;
        public bool Success { get; set; }
    }

}
