namespace PaymentGateway.Models
{
    public class ProcessPaymentResponse
    {
        public string PaymentIdentifier { get; set; }   
        public string StatusCode { get; set; } //todo consider using enum
    }
}