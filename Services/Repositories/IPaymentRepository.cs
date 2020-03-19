using PaymentGateway.Services.Entities;
using System;
using System.Threading.Tasks;

namespace PaymentGateway.Services.Repositories
{
    public interface IPaymentRepository
    {
        Task<Payment> Get(string paymentIdentifier);

        Task Save(Payment payment);

        Task Update(Payment payment);
    }
}
