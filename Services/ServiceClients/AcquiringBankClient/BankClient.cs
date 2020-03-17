using PaymentGateway.Services.ServiceClients.AcquiringBankClient.Models;
using System;
using System.Threading.Tasks;

namespace PaymentGateway.Services.ServiceClients
{
    public class BankClient : IBankClient
    {
        public async Task<BankPaymentResponse> ProcessPayment(BankPaymentRequest request)
        {
            // are retries allowed?

            // todo real implementation
            return new BankPaymentResponse()
            {
                PaymentIdentifier = Guid.NewGuid().ToString(),
                PaymentStatusCode = "Success" //todo different code depending on request
            };
        }
    }
}
