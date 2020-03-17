using PaymentGateway.Services.Entities;
using System.Net;

namespace PaymentGateway.Services.ServiceClients.AcquiringBankClient
{
    public interface IStatusCodeConverter
    {
        PaymentStatusCode ConvertToStatusCode(HttpStatusCode statusCode, string paymentErrorCode);
    }
}