using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankSimulator.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BankSimulator.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BankPaymentsController : ControllerBase 
    {
        private readonly ILogger<BankPaymentsController> _logger;

        public BankPaymentsController(ILogger<BankPaymentsController> logger)
        {
            _logger = logger;
        }

        // ASSUMPTIONS
        // Acquiring bank exposes a RESTful HTTP API
        // The payment processing endpoint
        //  - returns 200 and payment identifier for successful requests, 
        //  - returns 4xx/5xx, payment identifier and error code for failed requests 
        [HttpPost]
        public async Task<ActionResult<BankPaymentResponse>> ProcessPayment(BankPaymentRequest request)
        {
            if (request.PaymentCardNumber.Length != 16)
            {
                return BadRequest(new BankPaymentResponse()
                                {
                                    PaymentIdentifier = Guid.NewGuid().ToString(),
                                    PaymentErrorCode = "Failure_IncorrectCardNumber"
                                });
            }

            return new BankPaymentResponse()
            {
                PaymentIdentifier = Guid.NewGuid().ToString(),
            };
        }
    }
}
