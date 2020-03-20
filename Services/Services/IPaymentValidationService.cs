using PaymentGateway.Services.Entities;
using System.Collections.Generic;

namespace PaymentGateway.Services.Services
{
    public interface IPaymentValidationService
    {
        IEnumerable<string> Validate(Payment payment);
    }
}
