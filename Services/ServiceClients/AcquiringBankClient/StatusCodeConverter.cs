using PaymentGateway.Services.Entities;
using System.Net;

namespace PaymentGateway.Services.ServiceClients.AcquiringBankClient
{

    public class StatusCodeConverter : IStatusCodeConverter
    {

        // This method maps from Http status code and the error code returned from the bank endpoint
        // to status codes used in PaymentGateway.
        public PaymentStatusCode ConvertToStatusCode(HttpStatusCode statusCode, string paymentErrorCode)
        {
            // Current implementation ignores the error code but production code should use it too

            // This is a basic implementation, just to satisfy test scenarios
            if ((int)statusCode >= 200 && (int)statusCode < 300)
            {
                return PaymentStatusCode.Success;
            }

            if ((int)statusCode >= 400 && (int)statusCode < 500)
            {
                return PaymentStatusCode.AcquiringBankFailureCode1;
            }

            if (((int)statusCode >= 500 && (int)statusCode < 600))
            {
                return PaymentStatusCode.AcquiringBankFailureCode2;
            }

            return PaymentStatusCode.AcquiringBankFailureCode2;
        }
    }
}
