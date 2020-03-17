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
            if ((int)statusCode >= 200 && (int)statusCode < 300)
            {
                return PaymentStatusCode.Success;
            }

            // some logic which maps error codes (not implemented)
           
            return PaymentStatusCode.FailureCode1;
        }
    }
}
