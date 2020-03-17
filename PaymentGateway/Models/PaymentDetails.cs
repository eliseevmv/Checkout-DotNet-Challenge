using PaymentGateway.Services.Entities;
using System;

namespace PaymentGateway.Models
{
    public class PaymentDetails
    {
        // todo consider putting bank response into a separate object
        public string PaymentIdentifier { get; set; }
        public PaymentStatusCode StatusCode { get; set; } 

        // todo consider putting amount, currency and card information from the original request in a separate object

        public decimal Amount { get; set; }
        public string Currency { get; set; }  

        public string MaskedCardNumber { get; set; }  
        public string ExpiryMonthAndDate { get; set; } 
        public string Cvv { get; set; }

        public Guid MerchantId { get; set; } //todo remove?
    }
}
