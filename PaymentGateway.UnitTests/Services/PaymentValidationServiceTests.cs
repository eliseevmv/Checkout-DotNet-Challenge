using NUnit.Framework;
using PaymentGateway.Services.Services;
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
            var paymentEntity = CreateValidPayment();
            
            var result = paymentValidationService.Validate(paymentEntity);
            
            Assert.IsNotNull(result);
            CollectionAssert.IsEmpty(result);
        }

        // All other validation scenarios

        private static Payment CreateValidPayment()
        {
            return new Payment()
            {
                PaymentId = Guid.NewGuid(),
                Amount = 123,
                CardNumber = "1234123412341234",
                Currency = "GBP",
                ExpiryMonthAndDate = "0421",
                Cvv = "526",
                MerchantId = Guid.NewGuid(),
                StatusCode = Services.Entities.PaymentStatusCode.Processing
            };
        }
    }
}