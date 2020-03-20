namespace PaymentGateway.Services.Entities
{
    public enum PaymentStatusCode
    {
        Processing = 0,
        Success = 1,
        ValidationFailed = 2,
        AcquiringBankFailureCode1 = 3,
        AcquiringBankFailureCode2 = 4,
        Unknown = 5,
    }
}