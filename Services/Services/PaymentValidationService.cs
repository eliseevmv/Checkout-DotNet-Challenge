using PaymentGateway.Services.Entities;
using System;
using System.Collections.Generic;
using System.Text;

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

            // todo Validate currency. It should be in the list of supported currencies. 

            // todo Validate card number, Expiry month and date and CVV
        }
    }
}
