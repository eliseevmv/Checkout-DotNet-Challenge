﻿using System.Net;

namespace PaymentGateway.Services.ServiceClients.AcquiringBankClient.Models
{
    public class BankPaymentResponseWithStatus
    {
        public BankPaymentResponse ResponseBody { get; set; }
        public HttpStatusCode StatusCode { get; set; }  
    }
}
