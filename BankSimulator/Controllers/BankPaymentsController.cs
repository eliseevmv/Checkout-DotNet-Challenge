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
        //  - returns 4xx, payment identifier and error code for requests failed validation
        //  - returns 5xx, payment identifier and error code for requests failed because of server side issues
        [HttpPost]
        public async Task<ActionResult<BankPaymentResponse>> ProcessPayment(BankPaymentRequest request)
        {
            if (request.PaymentAmount == 1111)
            {
                return BadRequest(new BankPaymentResponse()
                                {
                                    PaymentIdentifier = Guid.NewGuid().ToString(),
                                    PaymentErrorCode = "ValidationErrorA"
                                });
            }

            if (request.PaymentAmount == 2222)
            {
                return StatusCode(500, new BankPaymentResponse()
                {
                    PaymentIdentifier = Guid.NewGuid().ToString(),
                    PaymentErrorCode = "InternalErrorA"
                });
            }

            return new BankPaymentResponse()
            {
                PaymentIdentifier = Guid.NewGuid().ToString(),
            };
        }
    }
}
