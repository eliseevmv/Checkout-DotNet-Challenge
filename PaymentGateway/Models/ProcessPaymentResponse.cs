using PaymentGateway.Services.Entities;
using System.Net;

namespace PaymentGateway.API.Models
{
    public class ProcessPaymentResponse
    {
        public string PaymentId { get; set; }   
        public string StatusCode { get; set; }
    }
}