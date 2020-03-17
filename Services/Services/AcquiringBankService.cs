using AutoMapper;
using PaymentGateway.Services.Entities;
using PaymentGateway.Services.ServiceClients;
using PaymentGateway.Services.ServiceClients.AcquiringBankClient.Models;
using System.Threading.Tasks;

namespace PaymentGateway.Services.Services
{
    public class AcquiringBankService : IAcquiringBankService
    {
        private readonly IMapper _mapper;
        private readonly IBankClient _bankClient;

        public AcquiringBankService(IMapper mapper, IBankClient bankClient)
        {
            _mapper = mapper;
            _bankClient = bankClient;
        }

        public async Task ProcessPayment(Payment payment)
        {
            var bankRequest = _mapper.Map<BankPaymentRequest>(payment); 
        
            var bankResponse = await _bankClient.ProcessPayment(bankRequest);
            // how do I deal with bank response which takes too much time?

            // todo consider creating a single method eg UpdateBankResponseDetails or using child class
            payment.PaymentIdentifier = bankResponse.PaymentIdentifier;
            payment.StatusCode = bankResponse.PaymentStatusCode;
        }
    }
}
