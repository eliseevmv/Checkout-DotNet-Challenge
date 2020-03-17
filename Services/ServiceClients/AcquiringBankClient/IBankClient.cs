using PaymentGateway.Services.ServiceClients.AcquiringBankClient.Models;
using System.Threading.Tasks;

namespace PaymentGateway.Services.ServiceClients
{
    public interface IBankClient
    {
        Task<BankPaymentResponse> ProcessPayment(BankPaymentRequest request);
    }
}
