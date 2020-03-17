using System.Net;

namespace PaymentGateway.IntegrationTests.ServiceClient.Models
{
    public class ProcessPaymentResponse
    {
        public string PaymentIdentifier { get; set; }   
        public string StatusCode { get; set; }
    }
}