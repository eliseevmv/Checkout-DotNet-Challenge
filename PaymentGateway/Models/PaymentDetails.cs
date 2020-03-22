using PaymentGateway.Services.Entities;
using System;

namespace PaymentGateway.API.Models
{
    public class PaymentDetails
    {
        public string PaymentId { get; set; }
        public string StatusCode { get; set; } 

        // todo consider putting amount, currency and card information from the original request in a separate object

        public decimal Amount { get; set; }
        public string Currency { get; set; }  

        public string MaskedCardNumber { get; set; }  
        public string ExpiryMonthAndDate { get; set; } 
        public string Cvv { get; set; }
    }
}
