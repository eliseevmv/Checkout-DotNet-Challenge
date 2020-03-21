using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PaymentGateway.Models;
using PaymentGateway.Services.Entities;
using PaymentGateway.Services.Repositories;
using PaymentGateway.Services.Services;

namespace PaymentGateway.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public partial class PaymentsController : ControllerBase
    {
        private readonly ILogger<PaymentsController> _logger;
        private readonly IPaymentRepository _repository;
        private readonly IMapper _mapper;
        private readonly IPaymentService _paymentService;

        public PaymentsController(
            ILogger<PaymentsController> logger,
            IPaymentRepository repository,
            IMapper mapper,
            IPaymentService paymentService)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
            _paymentService = paymentService;
        }

        // todo exception logging

        // todo idempotency? request identifier?
        [HttpPost]                                                                  // todo Api.ProcessPaymentRequest?
        public async Task<ActionResult<ProcessPaymentResponse>> ProcessPayment(ProcessPaymentRequest request) // todo Can it be done more RESTFUL?
        {
            var paymentEntity = _mapper.Map<Payment>(request);

            await _paymentService.ProcessPayment(paymentEntity);

            var response = _mapper.Map<ProcessPaymentResponse>(paymentEntity); 

            switch (paymentEntity.StatusCode)
            {
                case PaymentStatusCode.Success:
                    return Ok(response);
                case PaymentStatusCode.ValidationFailed:
                    return UnprocessableEntity(response);
                default:
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
