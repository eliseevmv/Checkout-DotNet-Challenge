using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PaymentGateway.Models;
using PaymentGateway.Services.Entities;
using PaymentGateway.Services.Repositories;
using PaymentGateway.Services.ServiceClients;
using PaymentGateway.Services.ServiceClients.AcquiringBankClient.Models;
using PaymentGateway.Services.Utils;

namespace PaymentGateway.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public partial class PaymentsController : ControllerBase
    {
        private readonly ILogger<PaymentsController> _logger;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IMapper _mapper;
        private readonly IBankClient _bankClient;

        public PaymentsController(ILogger<PaymentsController> logger, IPaymentRepository paymentRepository, IMapper mapper, IBankClient bankClient)
        {
            _logger = logger;
            _paymentRepository = paymentRepository;
            _mapper = mapper;
            _bankClient = bankClient;
        }

        // todo exception logging

        // todo idempotency? request identifier?
        [HttpPost]                                                                  // todo Api.ProcessPaymentRequest?
        public async Task<ProcessPaymentResponse> ProcessPayment(ProcessPaymentRequest request) // todo Can it be done more RESTFUL?
        {
            // todo validate required fields
            // todo bank identifier and also our identifier?
            var paymentDetails = _mapper.Map<Payment>(request);
            paymentDetails.MaskedCardNumber = CardDetailsUtility.MaskCardNumber(paymentDetails.CardNumber);

            var bankRequest = _mapper.Map<BankPaymentRequest>(paymentDetails); //todo consider pushing mapping to bank client and use the entity. How to deal with the non masked card number?
            var bankResponse = await _bankClient.ProcessPayment(bankRequest);
            // how do I deal with bank response which takes too much time?

            paymentDetails.PaymentIdentifier = bankResponse.PaymentIdentifier;
            paymentDetails.StatusCode = "FailureReason1"; // todo improve

            await _paymentRepository.Save(paymentDetails);
            // what happens if I get response from bank but saving to DB fails eg because of not null constraints
            //  in particular, what do we return to the merchant

            var response = _mapper.Map<ProcessPaymentResponse>(paymentDetails); // todo consider to get rid of it (but how to map enums? use same enum?)
            
            return response;
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
            // todo mention card number tokeniser?

           
        }
        }
}
