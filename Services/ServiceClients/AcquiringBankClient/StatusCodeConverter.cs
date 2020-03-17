using PaymentGateway.Services.Entities;

namespace PaymentGateway.Services.ServiceClients.AcquiringBankClient
{

    public class StatusCodeConverter : IStatusCodeConverter
    {

        // This method maps from Http status code and the error code returned from the bank endpoint
        // to status codes used in PaymentGateway.
        public PaymentStatusCode ConvertToStatusCode(bool isSuccessStatusCode, string paymentErrorCode)
        {
            if (isSuccessStatusCode)
            {
                return PaymentStatusCode.Success;
            }

            // some logic which maps error codes (not implemented)
           
            return PaymentStatusCode.FailureCode1;
        }
    }
}
