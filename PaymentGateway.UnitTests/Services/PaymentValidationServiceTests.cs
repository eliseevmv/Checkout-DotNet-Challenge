using NUnit.Framework;
using PaymentGateway.Services.Services;
using PaymentGateway.UnitTests.Common;
using System;
using Payment = PaymentGateway.Services.Entities.Payment;

namespace PaymentGateway.UnitTests
{
    // This is an example of a unit test
    // It tests a class in isolation. All dependencies should be replaced by test doubles, eg mocks
    public class PaymentValidationServiceTests
    {
        private PaymentValidationService paymentValidationService; 

        [SetUp]
        public void Setup()
        {
            paymentValidationService = new PaymentValidationService();
        }

        [Test]
        public void Given_valid_payment_when_validates_should_return_empty_collection()
        {
            var paymentEntity = PaymentEntityBuilder.CreateValidPayment();
            
            var result = paymentValidationService.Validate(paymentEntity);
            
            Assert.IsNotNull(result);
            CollectionAssert.IsEmpty(result);
        }

        // All other validation scenarios

       
    }
}