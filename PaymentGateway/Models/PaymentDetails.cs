using System;

namespace PaymentGateway.Models
{
    public class PaymentDetails
    {
        public string PaymentIdentifier { get; set; }
        public string MaskedCardNumber { get; set; }
        // todo card details
        public PaymentStatusCode StatusCode { get; set; }
    }
}
