using PaymentGateway.Services.Entities;
using System.Threading.Tasks;

namespace PaymentGateway.Services.Services
{
    public interface IPaymentService
    {
        Task ProcessPayment(Payment paymentEntity);

        Task<Payment> Get(string paymentIdentifier);
    }
}
