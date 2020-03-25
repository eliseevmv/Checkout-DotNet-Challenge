using Microsoft.Extensions.Logging;
using PaymentGateway.Services.Entities;
using PaymentGateway.Services.Repositories;
using PaymentGateway.Services.Utils;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway.Services.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly ILogger<PaymentService> _logger;
        private readonly IPaymentValidationService _validationService;
        private readonly IPaymentRepository _repository;
        private readonly IAcquiringBankService _acquiringBankService;

        public PaymentService(ILogger<PaymentService> logger,
                                        IPaymentValidationService validationService,
                                        IPaymentRepository repository,
                                        IAcquiringBankService acquiringBankService)
        {
            _logger = logger;
            _validationService = validationService;
            _repository = repository;
            _acquiringBankService = acquiringBankService;
        }

        public async Task ProcessPayment(Payment paymentEntity)
        {
            var validationErrors = _validationService.Validate(paymentEntity);

            if (validationErrors.Any())
            {
                paymentEntity.StatusCode = PaymentStatusCode.ValidationFailed;
                // It would be better to return a list of error messages as well
                return;
            }

            await _repository.Save(paymentEntity);

            await _acquiringBankService.ProcessPayment(paymentEntity);

            try
            {
                await _repository.Update(paymentEntity);
            }
            catch (Exception)
            {
                // See comments for Scenario 2.8 in readme.txt
            }
        }

        public async Task<Payment> Get(string paymentIdentifier)
        {
            return await _repository.Get(paymentIdentifier);
        }
    }
}
