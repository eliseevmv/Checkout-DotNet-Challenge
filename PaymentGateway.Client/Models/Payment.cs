using System;

namespace PaymentGateway.Client.Models
{
    public class Payment
    {
        public string PaymentIdentifier { get; set; }
        public string StatusCode { get; set; } 
        public decimal Amount { get; set; }
        public string Currency { get; set; }  
        public string MaskedCardNumber { get; set; }  
        public string ExpiryMonthAndDate { get; set; } 
        public string Cvv { get; set; }
        public Guid MerchantId { get; set; } 
    }
}
