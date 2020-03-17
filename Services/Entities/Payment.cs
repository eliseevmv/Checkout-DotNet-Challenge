using System;

namespace PaymentGateway.Services.Entities
{
    // todo consider creating child objects for payment request and bank response
    public class Payment
    {
        public string PaymentIdentifier { get; set; }
        public string StatusCode { get; set; } //todo consider using enum
        public decimal Amount { get; set; }
        public string Currency { get; set; }  //todo GBP/EUR/USD? what are the currencies supported by the bank?

        public string CardNumber { get; set; }  // custom type? note - it is NOT persisted to DB
        public string MaskedCardNumber { get; set; }  // custom type?
        public string ExpiryMonthAndDate { get; set; } // custom type?
        public string Cvv { get; set; }

        public Guid MerchantId { get; set; }

        // todo consider making it immutable

    }
}
