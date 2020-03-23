using System.Net;
using System.Threading.Tasks;
using NUnit.Framework;
using PaymentGateway.IntegrationTests.Builders;
using PaymentGateway.IntegrationTests.ServiceClient.Models;
using PaymentGateway.Services.ServiceClients.AcquiringBankClient;
using static PaymentGateway.Services.Entities.PaymentStatusCode;

namespace PaymentGateway.IntegrationTests
{
    public class IntegrationTests
    {
        private PaymentGatewayClient _client;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _client = new PaymentGatewayClient();
        }

        [Test]
        public async Task Given_valid_request_when_process_payment_and_get_details_then_details_are_correct()
        {
            var request = PaymentRequestBuilder.BuildValidPaymentRequest();

            var processPaymentResponse = await _client.ProcessPayment(request);

            Assert.That(processPaymentResponse.HttpStatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(processPaymentResponse.Content.StatusCode, Is.EqualTo(Success.ToString()));

            var getResponse = await _client.Get(processPaymentResponse.Content.PaymentId);

            Assert.That(getResponse.HttpStatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(getResponse.Content.StatusCode, Is.EqualTo(Success.ToString()));
            AssertThatPaymentDetailsAreCorrect(request, getResponse.Content);
        }

        [Test]
        public async Task Given_request_which_fails_payment_gateway_validation_when_process_payment_should_return_4xx_and_status_code()
        {
            var request = PaymentRequestBuilder.BuildPaymentRequestToFailPaymentGatewayValidation();

            var processPaymentResponse = await _client.ProcessPayment(request);

            Assert.That(processPaymentResponse.HttpStatusCode, Is.EqualTo(HttpStatusCode.UnprocessableEntity));
            Assert.That(processPaymentResponse.Content.StatusCode, Is.EqualTo(ValidationFailed.ToString()));

            var getResponse = await _client.Get(processPaymentResponse.Content.PaymentId);

            Assert.That(getResponse.HttpStatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public async Task Given_request_which_fails_bank_validation_when_process_payment_should_return_4xx_and_status_code()
        {
            var request = PaymentRequestBuilder.BuildPaymentRequestToFailBankValidation();

            var processPaymentResponse = await _client.ProcessPayment(request);

            Assert.That(processPaymentResponse.HttpStatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(processPaymentResponse.Content.StatusCode, Is.EqualTo(AcquiringBankFailureCode1.ToString()));

            var getResponse = await _client.Get(processPaymentResponse.Content.PaymentId);

            Assert.That(getResponse.HttpStatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(getResponse.Content.StatusCode, Is.EqualTo(AcquiringBankFailureCode1.ToString()));
        }

        [Test]
        public async Task When_process_payment_and_bank_cannot_process_payment_should_return_4xx_and_status_code()
        {
            var request = PaymentRequestBuilder.BuildPaymentRequestToSimulateErrorMessageFromBank();

            var processPaymentResponse = await _client.ProcessPayment(request);

            Assert.That(processPaymentResponse.HttpStatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(processPaymentResponse.Content.StatusCode, Is.EqualTo(AcquiringBankFailureCode2.ToString()));

            var getResponse = await _client.Get(processPaymentResponse.Content.PaymentId);

            Assert.That(getResponse.HttpStatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(getResponse.Content.StatusCode, Is.EqualTo(AcquiringBankFailureCode2.ToString()));
        }

        private void AssertThatPaymentDetailsAreCorrect(ProcessPaymentRequest expected, Payment actual)
        {
            Assert.That(actual.Amount, Is.EqualTo(expected.Amount));
            Assert.That(actual.Currency, Is.EqualTo(expected.Currency));
            AssertThatCardNumberIsSameAndIsMasked(actual.MaskedCardNumber, expected.CardNumber);
            Assert.That(actual.ExpiryMonthAndDate, Is.EqualTo(expected.ExpiryMonthAndDate));
            Assert.That(actual.Cvv, Is.EqualTo(expected.Cvv));
        }

        private void AssertThatCardNumberIsSameAndIsMasked(string actualMaskedCardNumber, string expectedCardNumber)
        {
            Assert.That(actualMaskedCardNumber.Length, Is.EqualTo(16));
            Assert.That(expectedCardNumber.Length, Is.EqualTo(16));

            Assert.That(actualMaskedCardNumber.Substring(0, 6), Is.EqualTo(expectedCardNumber.Substring(0, 6)));
            Assert.That(actualMaskedCardNumber.Substring(6, 6), Is.EqualTo("******"));
            Assert.That(actualMaskedCardNumber.Substring(12, 4), Is.EqualTo(expectedCardNumber.Substring(12, 4)));
        }
    }
}
