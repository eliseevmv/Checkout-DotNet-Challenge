using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentGateway.Services.ServiceClients.AcquiringBankClient.Models
{
    public class BankPaymentRequest
    {
        // Acquring bank API is a 3rd party API.  
        // It is possible that the bank API uses field names different that the field names used by Payment Gateway.
        // In order to simulate this scenario, I have prefixed all fields in this request with "Payment" 

        public decimal PaymentAmount { get; set; }
        public string PaymentCurrency { get; set; }  
        public string PaymentCardNumber { get; set; }  
        public string PaymentExpiryMonthAndDate { get; set; } 
        public string PaymentCvv { get; set; }
    }
}
