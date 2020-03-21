﻿using PaymentGateway.Services.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.Services.Services
{
    public interface IPaymentProcessingService
    {
        Task ProcessPayment(Payment paymentEntity);
    }
}
