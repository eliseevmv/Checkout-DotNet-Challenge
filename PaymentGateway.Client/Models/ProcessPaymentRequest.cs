﻿using System;

namespace PaymentGateway.Client.Models
{
    public class ProcessPaymentRequest
    {
        public decimal Amount { get; set; }
        public string Currency { get; set; }  
        public string CardNumber { get; set; } 
        public string ExpiryMonthAndDate { get; set; } 
        public string Cvv { get; set; }
        public Guid MerchantId { get; set; }
    }
}