using PaymentGateway.Services.Utils;
using System;

namespace PaymentGateway.Services.Entities
{
    public class Payment
    {
        private string cardNumber;

        public Guid PaymentId { get; set; }
        public string AcquringBankPaymentId { get; set; }
        public PaymentStatusCode StatusCode { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }  // It should be an enum or currencyId

        public string CardNumber 
        {
            get { return cardNumber; }
            set { cardNumber = value; MaskedCardNumber = CardDetailsUtility.MaskCardNumber(cardNumber); }
        }
        public string MaskedCardNumber { get; private set; }
        public string ExpiryMonthAndDate { get; set; }
        public string Cvv { get; set; }

        public Guid MerchantId { get; set; }

        public Payment()
        {
            PaymentId = Guid.NewGuid();
            StatusCode = PaymentStatusCode.Processing;
        }

        // Ideally this class should have private setters. All property changes should be done via methods.


    }
}
