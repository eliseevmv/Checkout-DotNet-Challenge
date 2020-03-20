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
using PaymentGateway.Services.Services;
using PaymentGateway.Services.Utils;

namespace PaymentGateway.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public partial class PaymentsController : ControllerBase
    {
        private readonly ILogger<PaymentsController> _logger;
        private readonly IPaymentValidationService _validationService;
        private readonly IPaymentRepository _repository;
        private readonly IMapper _mapper;
        private readonly IAcquiringBankService _acquiringBankService;

        public PaymentsController(ILogger<PaymentsController> logger, IPaymentValidationService validationService, IPaymentRepository repository, IMapper mapper, IAcquiringBankService acquiringBankService)
        {
            _logger = logger;
            _validationService = validationService;
            _repository = repository;
            _mapper = mapper;
            _acquiringBankService = acquiringBankService;
        }


        // todo exception logging

        // todo idempotency? request identifier?
        [HttpPost]                                                                  // todo Api.ProcessPaymentRequest?
        public async Task<ActionResult<ProcessPaymentResponse>> ProcessPayment(ProcessPaymentRequest request) // todo Can it be done more RESTFUL?
        {
            // todo implement validation: required fields, card number should be 16 digits, supported currencies etc)

            var paymentEntity = _mapper.Map<Payment>(request);
   
            paymentEntity.PaymentId = Guid.NewGuid();
            paymentEntity.StatusCode = PaymentStatusCode.Processing;
            paymentEntity.MaskedCardNumber = CardDetailsUtility.MaskCardNumber(paymentEntity.CardNumber);
   
            var validationErrors = _validationService.Validate(paymentEntity);

            if (validationErrors.Any())
            {
                return UnprocessableEntity(new ProcessPaymentResponse()
                    {
                        PaymentId = paymentEntity.PaymentId.ToString(),
                        StatusCode = PaymentStatusCode.ValidationFailed.ToString()
                    });
            }
            
            await _repository.Save(paymentEntity);

            await _acquiringBankService.ProcessPayment(paymentEntity);
            await _repository.Update(paymentEntity);

            // what happens if I get response from bank but saving to DB fails eg because of not null constraints
            //  in particular, what do we return to the merchant

            var response = _mapper.Map<ProcessPaymentResponse>(paymentEntity); // todo consider to get rid of it (but how to map enums? use same enum?)

            if (paymentEntity.StatusCode == PaymentStatusCode.Success)
            {
                return Ok(response);
            }
            else
            {
                // this logic can be extended to support more http status codes
                return BadRequest(response);
            };
        }

        [HttpGet("{paymentIdentifier}")]

        public async Task<ActionResult<PaymentDetails>> Get(string paymentIdentifier)
        {
            var payment = await _repository.Get(paymentIdentifier);
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
