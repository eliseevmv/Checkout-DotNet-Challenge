using PaymentGateway.Services.ServiceClients.AcquiringBankClient.Models;
using System;
using System.Net;
using System.Threading.Tasks;

namespace PaymentGateway.Services.ServiceClients
{
    public interface IBankClient
    {
        Task<Tuple<BankPaymentResponse, HttpStatusCode>> ProcessPayment(BankPaymentRequest request);
    }
}
