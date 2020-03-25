using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using PaymentGateway.API.Controllers;
using PaymentGateway.API.Models;
using PaymentGateway.Services.Services;
using PaymentGateway.UnitTests.Common;
using System;
using System.Threading.Tasks;
using Payment = PaymentGateway.Services.Entities.Payment;

namespace PaymentGateway.UnitTests
{
    // This is an example of a unit test for a controller
    // It tests the controller in isolation. All dependencies should be replaced by test doubles, eg mocks
    public class PaymentsControllerTests
    {
        private Mock<ILogger<PaymentsController>> _logger;
        private Mock<IMapper> _mapper;
        private Mock<IPaymentService> _paymentService;

        private PaymentsController _paymentsController;

        [SetUp]
        public void Setup()
        {
            _logger = new Mock<ILogger<PaymentsController>>();
            _mapper = new Mock<IMapper>();
            _paymentService = new Mock<IPaymentService>();

            _paymentsController = new PaymentsController(_logger.Object, _mapper.Object, _paymentService.Object);
        }

        [Test]
        public async Task Given_valid_payment_when_processes_payment_should_return_200_and_correct_response()
        {
            var request = new ProcessPaymentRequest();
            var paymentEntity = new Services.Entities.Payment();
            var paymentResponse = new ProcessPaymentResponse();

            _mapper.Setup(x => x.Map<Services.Entities.Payment>(request))
                   .Returns(paymentEntity);

            _paymentService.Setup(x => x.ProcessPayment(paymentEntity))
                           .Callback(() => paymentEntity.StatusCode = Services.Entities.PaymentStatusCode.Success);

            _mapper.Setup(x => x.Map<ProcessPaymentResponse>(paymentEntity))
                   .Returns(paymentResponse);

            var response = await _paymentsController.ProcessPayment(request);

            Assert.IsInstanceOf<ActionResult<ProcessPaymentResponse>>(response);
           // var returnValue = ((ActionResult<ProcessPaymentResponse>)response).Result.ExecuteResultAsync();

          

            
            //var x = (ActionResult<ProcessPaymentResponse>)response;
           // x.Value.
        }

        // All other validation scenarios

       
    }
}