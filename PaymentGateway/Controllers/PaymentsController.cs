using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace PaymentGateway.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly ILogger<PaymentsController> _logger;

        public PaymentsController(ILogger<PaymentsController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<PaymentDetails> Get(Guid id)
        {
            if (id.ToString().First() == '1')
            {
                return new PaymentDetails
                {
                    Id = id,
                    StatusCode = StatusCode.FailureReason1
                };
            }
            return new PaymentDetails
            {
                Id = id,
                MaskedCardNumber = "327237****43743",
                StatusCode = StatusCode.Success
            };
        }
        public class PaymentDetails
        {
            public Guid Id { get; set; }  
            public string MaskedCardNumber { get; set; }
            // todo card details
            public StatusCode StatusCode { get; set; }
        }

        public enum StatusCode
        {
            Success = 0,
            FailureReason1 = 1,
            FailureReason2 = 2
        }
        }
}
