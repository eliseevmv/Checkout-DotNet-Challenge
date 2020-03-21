using System;

namespace PaymentGateway.Services.Entities
{
    public class Payment
    {
        public Guid PaymentId { get; set; }
        public string AcquringBankPaymentId { get; set; }
        public PaymentStatusCode StatusCode { get; set; } 
        public decimal Amount { get; set; }
        public string Currency { get; set; }  // It might be an enum or currencyId

        public string CardNumber { get; set; }  // It is NOT persisted to DB
        public string MaskedCardNumber { get; set; }  
        public string ExpiryMonthAndDate { get; set; } 
        public string Cvv { get; set; }

        public Guid MerchantId { get; set; }

        // Ideally this class should have private setters. All property changes should be done via methods.
    }
}
