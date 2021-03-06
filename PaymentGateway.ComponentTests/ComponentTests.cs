using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Moq;
using NUnit.Framework;
using PaymentGateway.API.Models;
using PaymentGateway.ComponentTests.Builders;
using PaymentGateway.ComponentTests.Infrastructure;
using PaymentGateway.Services.Entities;
using PaymentGateway.Services.Repositories;
using PaymentGateway.Services.ServiceClients.AcquiringBankClient;
using PaymentGateway.Services.ServiceClients.AcquiringBankClient.Models;
using System;
using System.Net;
using System.Threading.Tasks;
using PaymentEntity = PaymentGateway.Services.Entities.Payment;

namespace PaymentGateway.ComponentTests
{
    public class ComponentTests 
    {
        private PaymentGatewayClient _client;
        private Mock<IPaymentRepository> _paymentRepositoryMock;
        private Mock<IBankClient> _bankClientMock;

        [SetUp]
        public void SetUp()
        {
            var builder = new WebHostBuilder().UseStartup<TestStartup>();
            var testServer = new TestServer(builder);
            _client = new PaymentGatewayClient(testServer.CreateClient());

            _paymentRepositoryMock = GetMock<IPaymentRepository>(testServer);
            _bankClientMock = GetMock<IBankClient>(testServer);
        }

        [Test]
        public async Task Given_valid_request_when_ProcessPayment_and_bank_returns_200_then_returns_200()
        {
            _paymentRepositoryMock.Setup(x => x.Save(It.IsAny<PaymentEntity>()))
                                  .Returns(Task.CompletedTask);
            _paymentRepositoryMock.Setup(x => x.Update(It.IsAny<PaymentEntity>()))
                                  .Returns(Task.CompletedTask);
            _bankClientMock.Setup(x => x.ProcessPayment(It.IsAny<BankPaymentRequest>()))
                           .ReturnsAsync(GetSuccessfulResponseFromBank());

            var request = PaymentRequestBuilder.BuildValidPaymentRequest();

            var response = await _client.Post<ProcessPaymentRequest, ProcessPaymentResponse>("/payments", request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.ResponseBody.StatusCode, Is.EqualTo(PaymentStatusCode.Success.ToString()));
            Assert.That(response.ResponseBody.PaymentId, Is.Not.Null.And.Not.Empty);
        }

        [Test]
        public async Task Given_valid_request_when_ProcessPayment_and_repository_throws_exception_before_calling_bank_then_returns_500()
        {
            _paymentRepositoryMock.Setup(x => x.Save(It.IsAny<PaymentEntity>()))
                                  .Throws(new System.Exception());
            _paymentRepositoryMock.Setup(x => x.Update(It.IsAny<PaymentEntity>()))
                                  .Returns(Task.CompletedTask);
            _bankClientMock.Setup(x => x.ProcessPayment(It.IsAny<BankPaymentRequest>()))
                           .ReturnsAsync(GetSuccessfulResponseFromBank());

            var request = PaymentRequestBuilder.BuildValidPaymentRequest();

            var response = await _client.Post<ProcessPaymentRequest>("/payments", request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.InternalServerError));
        }

        [Test]
        public async Task Given_valid_request_when_ProcessPayment_and_bank_returns_400_then_returns_400()
        {
            _paymentRepositoryMock.Setup(x => x.Save(It.IsAny<PaymentEntity>()))
                                  .Returns(Task.CompletedTask);
            _paymentRepositoryMock.Setup(x => x.Update(It.IsAny<PaymentEntity>()))
                                  .Returns(Task.CompletedTask);
            _bankClientMock.Setup(x => x.ProcessPayment(It.IsAny<BankPaymentRequest>()))
                           .ReturnsAsync(GetErrorResponseFromBank(HttpStatusCode.BadRequest));

            var request = PaymentRequestBuilder.BuildValidPaymentRequest();

            var response = await _client.Post<ProcessPaymentRequest>("/payments", request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task Given_valid_request_when_ProcessPayment_and_bank_returns_500_then_returns_400()
        {
            _paymentRepositoryMock.Setup(x => x.Save(It.IsAny<PaymentEntity>()))
                                  .Returns(Task.CompletedTask);
            _paymentRepositoryMock.Setup(x => x.Update(It.IsAny<PaymentEntity>()))
                                  .Returns(Task.CompletedTask);
            _bankClientMock.Setup(x => x.ProcessPayment(It.IsAny<BankPaymentRequest>()))
                           .ReturnsAsync(GetErrorResponseFromBank(HttpStatusCode.InternalServerError));

            var request = PaymentRequestBuilder.BuildValidPaymentRequest();

            var response = await _client.Post<ProcessPaymentRequest>("/payments", request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task Given_valid_request_when_ProcessPayment_and_repository_throws_exception_after_calling_bank_then_returns_ok()
        {
            _paymentRepositoryMock.Setup(x => x.Save(It.IsAny<PaymentEntity>()))
                                  .Returns(Task.CompletedTask);
            _paymentRepositoryMock.Setup(x => x.Update(It.IsAny<PaymentEntity>()))
                                  .Throws(new Exception());
            _bankClientMock.Setup(x => x.ProcessPayment(It.IsAny<BankPaymentRequest>()))
                           .ReturnsAsync(GetSuccessfulResponseFromBank());

            var request = PaymentRequestBuilder.BuildValidPaymentRequest();

            var response = await _client.Post<ProcessPaymentRequest>("/payments", request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public async Task Given_valid_request_when_ProcessPayment_then_passes_correct_request_to_bank()
        {
            BankPaymentRequest requestSentToBank = null;
            _paymentRepositoryMock.Setup(x => x.Save(It.IsAny<PaymentEntity>()))
                                  .Returns(Task.CompletedTask);
            _paymentRepositoryMock.Setup(x => x.Update(It.IsAny<PaymentEntity>()))
                                  .Returns(Task.CompletedTask);
            _bankClientMock.Setup(x => x.ProcessPayment(It.IsAny<BankPaymentRequest>()))
                           .Callback((BankPaymentRequest request) => requestSentToBank = request)
                           .ReturnsAsync(GetSuccessfulResponseFromBank());

            var request = PaymentRequestBuilder.BuildValidPaymentRequest();

            var response = await _client.Post<ProcessPaymentRequest, ProcessPaymentResponse>("/payments", request);

            AssertThatBankPaymentRequetIsCorrect(requestSentToBank, request);

            // Alternative solution is to use Verify command which compares properties of the bank request passed as an argument with the expected values 
        }

        [Test]
        public async Task Given_valid_request_when_ProcessPayment_and_bank_returns_200_then_updates_database_correctly()
        {
            string bankPaymentId = Guid.NewGuid().ToString();
            PaymentEntity paymentEntitySavedToDb = null;

            _paymentRepositoryMock.Setup(x => x.Save(It.IsAny<PaymentEntity>()))
                                  .Returns(Task.CompletedTask);
            _paymentRepositoryMock.Setup(x => x.Update(It.IsAny<PaymentEntity>()))
                                  .Callback((PaymentEntity pe) => paymentEntitySavedToDb = pe)
                                  .Returns(Task.CompletedTask);
            _bankClientMock.Setup(x => x.ProcessPayment(It.IsAny<BankPaymentRequest>()))
                           .ReturnsAsync(GetSuccessfulResponseFromBank(bankPaymentId));

            var request = PaymentRequestBuilder.BuildValidPaymentRequest();

            var response = await _client.Post<ProcessPaymentRequest, ProcessPaymentResponse>("/payments", request);

            AssertThatPaymentEntityIsCorrect(paymentEntitySavedToDb, request, bankPaymentId ,PaymentStatusCode.Success);

            // Alternative solution is to use Verify command which compares properties of the entity passed as an argument with the expected values 
        }    

        // ... More tests for ProcessPayment


        [Test]
        public async Task Given_wrong_identifier_when_Get_then_returns_404()
        {
            _paymentRepositoryMock.Setup(x => x.Get(It.IsAny<string>()))
                                  .ReturnsAsync((PaymentEntity)null);

            var response = await _client.Get<API.Models.Payment>("/payments/UNKNOWN");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        // ... More tests for Get

        private static Mock<T> GetMock<T>(TestServer testServer) where T : class
        {
            return (Mock<T>)testServer.Services.GetService(typeof(Mock<T>));
        }

        private static BankPaymentResponseWithStatus GetSuccessfulResponseFromBank(string bankPaymentId = "bankPaymentId")
        {
            return new BankPaymentResponseWithStatus()
            {
                StatusCode = HttpStatusCode.OK,
                ResponseBody = new BankPaymentResponse
                {
                    PaymentIdentifier = bankPaymentId,
                    PaymentErrorCode = null
                }
            };
        }

        private static BankPaymentResponseWithStatus GetErrorResponseFromBank(HttpStatusCode httpStatusCode)
        {
            return new BankPaymentResponseWithStatus()
            {
                StatusCode = httpStatusCode,
                ResponseBody = new BankPaymentResponse
                {
                    PaymentIdentifier = "bankPaymentId",
                    PaymentErrorCode = "bankErrorCode"
                }
            };
        }

        private void AssertThatBankPaymentRequetIsCorrect(BankPaymentRequest actual, ProcessPaymentRequest expected)
        {
            Assert.That(actual, Is.Not.Null);
            Assert.That(actual.PaymentAmount, Is.EqualTo(expected.Amount));
            Assert.That(actual.PaymentCurrency, Is.EqualTo(expected.Currency));
            Assert.That(actual.PaymentCardNumber, Is.EqualTo(expected.CardNumber));
            Assert.That(actual.PaymentExpiryMonthAndDate, Is.EqualTo(expected.ExpiryMonthAndDate));
            Assert.That(actual.PaymentCvv, Is.EqualTo(expected.Cvv));
        }

        private void AssertThatPaymentEntityIsCorrect(PaymentEntity actual,
                                                     ProcessPaymentRequest expected,
                                                     string expectedBankPaymentId,
                                                     PaymentStatusCode expectedStatusCode)
        {
            Assert.That(actual, Is.Not.Null);
            Assert.That(actual.PaymentId, Is.Not.EqualTo(Guid.Empty));
            Assert.That(actual.AcquringBankPaymentId, Is.EqualTo(expectedBankPaymentId));
            Assert.That(actual.StatusCode, Is.EqualTo(expectedStatusCode));
            Assert.That(actual.Amount, Is.EqualTo(expected.Amount));
            Assert.That(actual.Currency, Is.EqualTo(expected.Currency));
            Assert.That(actual.CardNumber, Is.EqualTo(expected.CardNumber));
            Assert.That(actual.MaskedCardNumber, Does.StartWith(expected.CardNumber.Substring(0, 4)).And.Contains("****"));
            Assert.That(actual.ExpiryMonthAndDate, Is.EqualTo(expected.ExpiryMonthAndDate));
            Assert.That(actual.Cvv, Is.EqualTo(expected.Cvv));
            Assert.That(actual.MerchantId, Is.EqualTo(expected.MerchantId));
        }
    }
}