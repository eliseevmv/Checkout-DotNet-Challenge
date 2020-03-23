using PaymentGateway.API.Models;
using PaymentGateway.ComponentTests.Infrastructure;
using System;

namespace PaymentGateway.ComponentTests.Builders
{
    public static class PaymentRequestBuilder
    {
        public static ProcessPaymentRequest BuildValidPaymentRequest()
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

    }
}
