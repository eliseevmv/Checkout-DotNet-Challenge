using AutoMapper;
using PaymentGateway.Services.Entities;
using PaymentGateway.Services.ServiceClients.AcquiringBankClient;
using PaymentGateway.Services.ServiceClients.AcquiringBankClient.Models;
using System.Threading.Tasks;

namespace PaymentGateway.Services.Services
{
    public class AcquiringBankService : IAcquiringBankService
    {
        private readonly IMapper _mapper;
        private readonly IBankClient _bankClient;
        private readonly IStatusCodeConverter _statusCodeConverter;

        public AcquiringBankService(IMapper mapper, IBankClient bankClient, IStatusCodeConverter statusCodeConverter)
        {
            _mapper = mapper;
            _bankClient = bankClient;
            _statusCodeConverter = statusCodeConverter;
        }

        public async Task ProcessPayment(Payment payment)
        {
            var bankRequest = _mapper.Map<BankPaymentRequest>(payment); 
        
            var bankResponse = await _bankClient.ProcessPayment(bankRequest);
            // We should consider dealing with scenario when with bank response takes too much time
            // If retries are allowed and are safe, and bank returns 5xx, this class should retry a payment several times
            // In high-load scenarios we should consider using circuit breaker 

            payment.AcquringBankPaymentId = bankResponse.ResponseBody.PaymentIdentifier;
            payment.StatusCode = _statusCodeConverter.ConvertToStatusCode(bankResponse.StatusCode, bankResponse.ResponseBody.PaymentErrorCode);
        }
    }
}
