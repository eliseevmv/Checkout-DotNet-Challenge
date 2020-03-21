using PaymentGateway.Services.Entities;
using System.Collections.Generic;

namespace PaymentGateway.Services.Services
{
    public class PaymentValidationService : IPaymentValidationService
    {
        public IEnumerable<string> Validate(Payment payment)
        {
            if (payment.Amount <= 0 )
            {
                yield return "Payment amount is negative or zero";
            }

            // Validate currency. It should be in the list of supported currencies. 

            // Validate card number (might depend on card type and involve checksum calculation) , Expiry month/date and CVV
        }
    }
}
