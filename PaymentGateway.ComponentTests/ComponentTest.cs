using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Moq;
using Newtonsoft.Json;
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

namespace PaymentGateway.ComponentTests
{
    public class ComponentTest 
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
            _paymentRepositoryMock.Setup(x => x.Save(It.IsAny<Services.Entities.Payment>())).Returns(Task.CompletedTask);
            _paymentRepositoryMock.Setup(x => x.Update(It.IsAny<Services.Entities.Payment>())).Returns(Task.CompletedTask);
            _bankClientMock.Setup(x => x.ProcessPayment(It.IsAny<BankPaymentRequest>())).ReturnsAsync(new BankPaymentResponseWithStatus()
            {
                StatusCode = HttpStatusCode.OK,
                ResponseBody = new BankPaymentResponse
                {
                    PaymentIdentifier = "someIdentifier",
                    PaymentErrorCode = null
                }
            }); ;

            var request = PaymentRequestBuilder.BuildValidPaymentRequest();

            var response = await _client.Post<ProcessPaymentRequest, ProcessPaymentResponse>("/payments", request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.ResponseBody.StatusCode, Is.EqualTo(PaymentStatusCode.Success.ToString()));
            Assert.That(response.ResponseBody.PaymentId, Is.Not.Null.And.Not.Empty);
        }

        [Test]
        public async Task Given_valid_request_when_ProcessPayment_and_repository_throws_exception_before_calling_bank_then_returns_500()
        {
            _paymentRepositoryMock.Setup(x => x.Save(It.IsAny<Services.Entities.Payment>())).Throws(new System.Exception());
            _paymentRepositoryMock.Setup(x => x.Update(It.IsAny<Services.Entities.Payment>())).Returns(Task.CompletedTask);
            _bankClientMock.Setup(x => x.ProcessPayment(It.IsAny<BankPaymentRequest>())).ReturnsAsync(new BankPaymentResponseWithStatus()
            {
                StatusCode = HttpStatusCode.OK,
                ResponseBody = new BankPaymentResponse
                {
                    PaymentIdentifier = "someIdentifier",
                    PaymentErrorCode = null
                }
            }); ;

            var request = PaymentRequestBuilder.BuildValidPaymentRequest();

            var response = await _client.Post<ProcessPaymentRequest>("/payments", request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.InternalServerError));
        }

        [Test]
        public async Task Given_valid_request_when_ProcessPayment_and_repository_throws_exception_after_calling_bank_then_returns_ok()
        {
            _paymentRepositoryMock.Setup(x => x.Save(It.IsAny<Services.Entities.Payment>())).Returns(Task.CompletedTask);
            _paymentRepositoryMock.Setup(x => x.Update(It.IsAny<Services.Entities.Payment>())).Throws(new Exception());
            _bankClientMock.Setup(x => x.ProcessPayment(It.IsAny<BankPaymentRequest>())).ReturnsAsync(new BankPaymentResponseWithStatus()
            {
                StatusCode = HttpStatusCode.OK,
                ResponseBody = new BankPaymentResponse
                {
                    PaymentIdentifier = "someIdentifier",
                    PaymentErrorCode = null
                }
            }); ;

            var request = PaymentRequestBuilder.BuildValidPaymentRequest();


            var response = await _client.Post<ProcessPaymentRequest>("/payments", request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        // ... More tests for ProcessPayment


        [Test]
        public async Task Given_wrong_identifier_when_Get_then_returns_404()
        {
            _paymentRepositoryMock.Setup(x => x.Get(It.IsAny<string>())).ReturnsAsync((Services.Entities.Payment)null);

            var response = await _client.Get<API.Models.Payment>("/payments/UNKNOWN");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        // ... More tests for Get

        private static Mock<T> GetMock<T>(TestServer testServer) where T : class
        {
            return (Mock<T>)testServer.Services.GetService(typeof(Mock<T>));
        }

    }
}