using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PaymentGateway.Models;
using PaymentGateway.Services.Repositories;

namespace PaymentGateway.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public partial class PaymentsController : ControllerBase
    {
        private readonly ILogger<PaymentsController> _logger;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IMapper _mapper;

        public PaymentsController(ILogger<PaymentsController> logger, IPaymentRepository paymentRepository, IMapper mapper)
        {
            _logger = logger;
            _paymentRepository = paymentRepository;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ProcessPaymentResponse> ProcessPayment(ProcessPaymentRequest request) // todo Can it be done more RESTFUL?
        {
            throw new NotImplementedException();

            // todo validate required fields

            // todo bank identifier and also our identifier?
            // are retries allowed?
            // how do I deal with bank response which takes too much time?

            // what happens if I get response from bank but saving to DB fails eg because of not null constraints
            //  in particular, what do we return to the merchant
        }

        [HttpGet("{paymentIdentifier}")]

      //  [Consumes(MediaTypeNames.Application.Json)]
        //[ProducesResponseType(StatusCodes.Status201Created)]
        //  [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PaymentDetails>> Get(string paymentIdentifier)
        {
            var payment = await _paymentRepository.Get(paymentIdentifier);
            if (payment == null)
            {
                return NotFound();
            }

            var paymentResponse = _mapper.Map<PaymentDetails>(payment);
            return paymentResponse;

            // id - query string or url?
            // todo mention card number tokeniser?

           
        }
        }
}
