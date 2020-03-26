using NUnit.Framework;
using PaymentGateway.Services.Services;
using PaymentGateway.UnitTests.Common;
using System;
using System.Linq;
using Payment = PaymentGateway.Services.Entities.Payment;

namespace PaymentGateway.UnitTests
{
    // This is an example of a unit test
    // It tests a class in isolation. All dependencies should be replaced by test doubles, eg mocks
    // Ideally most classes in the solution, including PaymentsController and PaymentService, should have a set of unit tests
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

        [Test]
        public void Given_payment_with_negative_amount_when_validates_should_return_error_message()
        {
            var paymentEntity = PaymentEntityBuilder.CreateValidPayment();
            paymentEntity.Amount = -1;

            var result = paymentValidationService.Validate(paymentEntity);

            Assert.IsNotNull(result);
            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(result.First(), Does.Contain("negative"));
        }

        // All other validation scenarios


    }
}