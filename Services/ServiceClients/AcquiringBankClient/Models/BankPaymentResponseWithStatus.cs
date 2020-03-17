using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace PaymentGateway.Services.ServiceClients.AcquiringBankClient.Models
{
    public class BankPaymentResponseWithStatus
    {
        public BankPaymentResponse ResponseBody { get; set; }
        public bool IsSuccessStatusCode { get; set; }  
    }
}
