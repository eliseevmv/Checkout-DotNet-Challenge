using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentGateway.Services.ServiceClients.AcquiringBankClient.Models
{
    public class BankPaymentResponse
    {
        // Acquring bank API is a 3rd party API.  
        // It is possible that the bank API uses field names different that the field names used by Payment Gateway.
        // In order to simulate this scenario, I have prefixed all fields in this request with "Payment" 

        public string PaymentIdentifier { get; set; }
        public BankPaymentStatusCode PaymentStatusCode { get; set; }  
    }
}
