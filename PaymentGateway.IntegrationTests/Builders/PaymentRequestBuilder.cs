using PaymentGateway.IntegrationTests.ServiceClient.Models;
using System;

namespace PaymentGateway.IntegrationTests.Builders
{
    public static class PaymentRequestBuilder
    {
        public static ProcessPaymentRequest BuildValidPaymentRequest()
        {
            return new ProcessPaymentRequest()
            {
                Amount = 123,
                Currency = "GBP",
                CardNumber = "1234567812345678",
                ExpiryMonthAndDate = "1220",
                Cvv = "425",
                MerchantId = Guid.NewGuid()
            };
        }

        public static ProcessPaymentRequest BuildPaymentRequestToFailPaymentGatewayValidation()
        { 
            var request = BuildValidPaymentRequest();
            request.Amount = 0;
            return request;
        }

        public static ProcessPaymentRequest BuildPaymentRequestToFailBankValidation()
        {
            var request = BuildValidPaymentRequest();
            request.Amount = BankSimulatorConstants.PaymentAmountForValidationError;
            return request;
        }

        public static ProcessPaymentRequest BuildPaymentRequestToSimulateErrorMessageFromBank()
        {
            var request = BuildValidPaymentRequest();
            request.Amount = BankSimulatorConstants.PaymentAmountToSimulateErrorMessageFromBank;
            return request;
        }
    }
}
