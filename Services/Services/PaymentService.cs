using Microsoft.Extensions.Logging;
using PaymentGateway.Services.Entities;
using PaymentGateway.Services.Repositories;
using PaymentGateway.Services.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            paymentEntity.PaymentId = Guid.NewGuid();
            paymentEntity.StatusCode = PaymentStatusCode.Processing;
            paymentEntity.MaskedCardNumber = CardDetailsUtility.MaskCardNumber(paymentEntity.CardNumber);

            var validationErrors = _validationService.Validate(paymentEntity);

            if (validationErrors.Any())
            {
                paymentEntity.StatusCode = PaymentStatusCode.ValidationFailed;
                // It would be better to return a list of error messages as well
                return;
            }

            await _repository.Save(paymentEntity);

            await _acquiringBankService.ProcessPayment(paymentEntity);
            await _repository.Update(paymentEntity);

            // what happens if I get response from bank but saving to DB fails eg because of not null constraints
            //  in particular, what do we return to the merchant
        }

        public async Task<Payment> Get(string paymentIdentifier)
        {
            return await _repository.Get(paymentIdentifier);
        }
    }
}
