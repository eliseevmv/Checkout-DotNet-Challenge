using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentGateway.Services.ServiceClients.AcquiringBankClient.Models
{
    public class BankPaymentRequest
    {
        // Acquring bank API is a 3rd party API.  
        // It is possible that the bank API uses field names different that the field names used by Payment Gateway.
        // In order to simulate this scenario, I have prefixed all fields in this request with "External" 

        public decimal PaymentAmount { get; set; }
        public string PaymentCurrency { get; set; }  //todo GBP/EUR/USD? what are the currencies supported by the bank?

        public string PaymentCardNumber { get; set; }  // custom type?
        public string PaymentExpiryMonthAndDate { get; set; } // custom type?
        public string PaymentCvv { get; set; }
    }
}
