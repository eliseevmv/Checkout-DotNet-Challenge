using PaymentGateway.Services.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentGateway.UnitTests.Common
{
    public class PaymentEntityBuilder
    {
        public static Payment CreateValidPayment()
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
