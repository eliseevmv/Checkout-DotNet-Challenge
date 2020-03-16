using System;

namespace PaymentGateway.Services.Entities
{
    public class Payment
    {
        public string PaymentIdentifier { get; private set; }
        public PaymentStatusCode StatusCode { get; private set; }
        public decimal Amount { get; private set; }
        public string Currency { get; private set; }  //todo GBP/EUR/USD? what are the currencies supported by the bank?

        public string MaskedCardNumber { get; private set; }  // custom type?
        public string ExpiryMonthDate { get; private set; } // custom type?
        public string Cvv { get; private set; }

        public Guid MerchantId { get; private set; }

        public Payment(string paymentIdentifier, 
            PaymentStatusCode statusCode, 
            decimal amount, 
            string currency, 
            string maskedCardNumber, 
            string expiryMonthDate, 
            string cvv, 
            Guid merchantId)
        {
            PaymentIdentifier = paymentIdentifier;
            StatusCode = statusCode;
            Amount = amount;
            Currency = currency;
            MaskedCardNumber = maskedCardNumber;
            ExpiryMonthDate = expiryMonthDate;
            Cvv = cvv;
            MerchantId = merchantId;
        }
    }
}
