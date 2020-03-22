using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PaymentGateway.API.Models;
using PaymentGateway.Services.Entities;
using PaymentGateway.Services.Services;

namespace PaymentGateway.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public partial class PaymentsController : ControllerBase
    {
        private readonly ILogger<PaymentsController> _logger;
        private readonly IMapper _mapper;
        private readonly IPaymentService _paymentService;

        public PaymentsController(ILogger<PaymentsController> logger, IMapper mapper, IPaymentService paymentService)
        {
            _logger = logger;
            _mapper = mapper;
            _paymentService = paymentService;
        }

        // todo mention card number tokeniser?
        // todo exception logging

        // todo idempotency? request identifier?
        // todo Api.ProcessPaymentRequest?

        // This endpoint returns either successful or unsuccessful response by HTTP status code.
        // However since Get endpoint returns payment details which include payment status,
        // for consistency this endpoint returns payment status too.
        // It also returns paymentId which can be used to get details of previously made payment from the Get endpoint.


        [HttpPost]                                                                  
        public async Task<ActionResult<ProcessPaymentResponse>> ProcessPayment(ProcessPaymentRequest request)
        {
            var paymentEntity = _mapper.Map<Services.Entities.Payment>(request);

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
        public async Task<ActionResult<API.Models.Payment>> Get(string paymentIdentifier)
        {
            var payment = await _paymentService.Get(paymentIdentifier);
            if (payment == null)
            {
                return NotFound();
            }

            var paymentResponse = _mapper.Map<API.Models.Payment>(payment);
            return paymentResponse;
        }
    }
}
