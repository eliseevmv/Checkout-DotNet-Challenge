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

        [HttpPost]
        public async Task<BankPaymentResponse> ProcessPayment(BankPaymentRequest request)
        {
            if (request.PaymentCardNumber.Length != 16)
            {
                return new BankPaymentResponse()
                {
                    PaymentIdentifier = Guid.NewGuid().ToString(),
                    PaymentStatusCode = "Failure_IncorrectCardNumber"
                };
            }

            return new BankPaymentResponse()
            {
                PaymentIdentifier = Guid.NewGuid().ToString(),
                PaymentStatusCode = "Success" //todo different code depending on request
            };
        }
    }
}
