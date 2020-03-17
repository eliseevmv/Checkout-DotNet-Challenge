namespace PaymentGateway.Services.ServiceClients.AcquiringBankClient.Models
{
    // Acquring bank API is a 3rd party API.  
    // It is possible that the bank API uses an enum which is slightly different that the enum used by the Payment Gateway.
    // In order to simulate this scenario, I have changed enum member names and values and prefixed names with "Payment" 

    public enum BankPaymentStatusCode
    {
            PaymentSuccess = 0,
            PaymentFailureReasonA = 1000,
            PaymentFailureReasonB = 2000
    }
}
