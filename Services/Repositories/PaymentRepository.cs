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
            var payment = new Payment
            (
                Guid.NewGuid().ToString(),
                PaymentStatusCode.Success,
                123,
                "GBP",
                "4343*****3433",
                "1220",
                "234",
                Guid.NewGuid()
            );
            return Task.FromResult(payment);
        }

        public Task Save(Payment payment)
        {
            throw new NotImplementedException();
        }
    }
}
