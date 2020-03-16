namespace PaymentGateway.Models
{
    public class ProcessPaymentResponse
    {
        public string PaymentIdentifier { get; set; }   
        public PaymentStatusCode StatusCode { get; set; }
    }
}