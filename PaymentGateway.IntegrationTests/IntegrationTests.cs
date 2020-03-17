using NUnit.Framework;
using PaymentGateway.IntegrationTests.ServiceClient.Models;
using PaymentGateway.Services.ServiceClients.AcquiringBankClient;
using System;
using System.Threading.Tasks;

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
            var request = CreateValidPaymentRequest();

            var response = await _client.ProcessPayment(request);
            var paymentDetails = await _client.Get(response.PaymentIdentifier);

            Assert.That(response.StatusCode, Is.EqualTo("Success"));
            Assert.That(paymentDetails.Amount, Is.EqualTo(request.Amount));
            Assert.That(paymentDetails.Currency, Is.EqualTo(request.Currency));
            AssertThatCardNumberIsSameAndIsMasked(paymentDetails.MaskedCardNumber, request.CardNumber);
            Assert.That(paymentDetails.ExpiryMonthAndDate, Is.EqualTo(request.ExpiryMonthAndDate));
            Assert.That(paymentDetails.Cvv, Is.EqualTo(request.Cvv));
            Assert.That(paymentDetails.StatusCode, Is.EqualTo("Success"));
        }

        private static ProcessPaymentRequest CreateValidPaymentRequest()
        {
            return new ProcessPaymentRequest()
            {
                Amount = 123,
                Currency = "GBP",
                CardNumber = "1234567812345678",
                ExpiryMonthAndDate = "1220",
                Cvv = "425",
                MerchantId = Guid.NewGuid()
            };
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
