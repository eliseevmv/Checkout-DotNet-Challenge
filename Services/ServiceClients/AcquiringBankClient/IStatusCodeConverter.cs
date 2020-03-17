using PaymentGateway.Services.Entities;

namespace PaymentGateway.Services.ServiceClients.AcquiringBankClient
{
    public interface IStatusCodeConverter
    {
        PaymentStatusCode ConvertToStatusCode(bool isSuccessStatusCode, string paymentErrorCode);
    }
}