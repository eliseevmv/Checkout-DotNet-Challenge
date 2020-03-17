using System;

namespace PaymentGateway.Models
{
    public class PaymentDetails
    {
        public string PaymentIdentifier { get; set; }
        public PaymentStatusCode StatusCode { get; set; }

        // todo consider putting payment details from the original request in a separate object

        public decimal Amount { get; set; }
        public string Currency { get; set; }  //todo GBP/EUR/USD? what are the currencies supported by the bank?

        public string MaskedCardNumber { get; set; }  // custom type?
        public string ExpiryMonthAndDate { get; set; } // custom type?
        public string Cvv { get; set; }

        public Guid MerchantId { get; set; }
    }
}
