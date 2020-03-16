using System;

namespace PaymentGateway.Models
{
    public class ProcessPaymentRequest
    {
        public decimal Amount { get; set; }
        public string Currency { get; set; }  //todo GBP/EUR/USD? what are the currencies supported by the bank?

        public string CardNumber { get; set; }  // custom type?
        public string ExpiryMonthDate { get; set; } // custom type?
        public string Cvv { get; set; }

        public Guid MerchantId { get; set; }
        // todo authentication
    }
}