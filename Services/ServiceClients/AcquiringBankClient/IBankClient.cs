using PaymentGateway.Services.ServiceClients.AcquiringBankClient.Models;
using System.Threading.Tasks;

namespace PaymentGateway.Services.ServiceClients.AcquiringBankClient
{
    public interface IBankClient
    {
        Task<BankPaymentResponseWithStatus> ProcessPayment(BankPaymentRequest request);
    }
}
