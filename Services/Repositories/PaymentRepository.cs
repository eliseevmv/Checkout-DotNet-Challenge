using PaymentGateway.Services.Entities;
using System;
using System.Threading.Tasks;

namespace PaymentGateway.Services.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        public Task<Payment> Get(string paymentIdentifier)
        {
            //if (paymentIdentifier.First() == '1')
            //{
            //    return new Payment(
            //    {
            //        PaymentIdentifier = paymentIdentifier,
            //        StatusCode = PaymentStatusCode.FailureReason1
            //    };
            //}
            //return new Payment
            //{
            //    PaymentIdentifier = paymentIdentifier,
            //    MaskedCardNumber = "327237****43743",
            //    StatusCode = PaymentStatusCode.Success
            //};
            throw new NotImplementedException();
        }

        public Task Save(Payment payment)
        {
            throw new NotImplementedException();
        }
    }
}
