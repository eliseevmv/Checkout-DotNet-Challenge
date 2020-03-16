using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PaymentGateway.Models;

namespace PaymentGateway.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public partial class PaymentsController : ControllerBase
    {
        private readonly ILogger<PaymentsController> _logger;

        public PaymentsController(ILogger<PaymentsController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public async Task<ProcessPaymentResponse> ProcessPayment(ProcessPaymentRequest request) // todo Can it be done more RESTFUL?
        {
            throw new NotImplementedException();

            // todo bank identifier and also our identifier?
            // are retries allowed?
            // how do I deal with bank response which takes too much time?
        }

        [HttpGet]
        public async Task<PaymentDetails> Get(string paymentIdentifier)
        {
            // todo 404 for payment not found
            // id - query string or url?

            // todo mention card number tokeniser?

            if (paymentIdentifier.First() == '1')
            {
                return new PaymentDetails
                {
                    PaymentIdentifier = paymentIdentifier,
                    StatusCode = PaymentStatusCode.FailureReason1
                };
            }
            return new PaymentDetails
            {
                PaymentIdentifier = paymentIdentifier,
                MaskedCardNumber = "327237****43743",
                StatusCode = PaymentStatusCode.Success
            };
        }
        }
}
