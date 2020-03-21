using PaymentGateway.Services.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.Services.Services
{
    public interface IPaymentService
    {
        Task ProcessPayment(Payment paymentEntity);
    }
}
