using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using PaymentGateway.ComponentTests.Builders;
using PaymentGateway.ComponentTests.Infrastructure;
using PaymentGateway.Models;
using PaymentGateway.Services.Entities;
using PaymentGateway.Services.Repositories;
using PaymentGateway.Services.ServiceClients.AcquiringBankClient;
using PaymentGateway.Services.ServiceClients.AcquiringBankClient.Models;
using System.Net;
using System.Threading.Tasks;

namespace PaymentGateway.ComponentTests
{
    public class ComponentTest 
    {
        private readonly PaymentGatewayClient client;
        private readonly Mock<IPaymentRepository> paymentRepositoryMock;
        private readonly Mock<IBankClient> bankClientMock;

        // todo ensure mocks are recreated for every test
        public ComponentTest()
        {
            var builder = new WebHostBuilder().UseStartup<TestStartup>();
            var testServer = new TestServer(builder);
            this.client = new PaymentGatewayClient(testServer.CreateClient());

            paymentRepositoryMock = GetMock<IPaymentRepository>(testServer);
            bankClientMock = GetMock<IBankClient>(testServer);
        }

        [Test]
        public async Task Given_valid_request_when_ProcessPayment_and_bank_returns_200_then_returns_200()
        {
            paymentRepositoryMock.Setup(x => x.Save(It.IsAny<Payment>())).Returns(Task.CompletedTask);
            paymentRepositoryMock.Setup(x => x.Update(It.IsAny<Payment>())).Returns(Task.CompletedTask);
            bankClientMock.Setup(x => x.ProcessPayment(It.IsAny<BankPaymentRequest>())).ReturnsAsync(new BankPaymentResponseWithStatus()
            {
                StatusCode = HttpStatusCode.OK,
                ResponseBody = new BankPaymentResponse
                {
                    PaymentIdentifier = "someIdentifier",
                    PaymentErrorCode = null
                }
            }); ;

            var request = PaymentRequestBuilder.BuildValidPaymentRequest();

            var response = await client.Post<ProcessPaymentRequest, ProcessPaymentResponse>("/payments", request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.ResponseBody.StatusCode, Is.EqualTo(PaymentStatusCode.Success.ToString()));
            Assert.That(response.ResponseBody.PaymentId, Is.Not.Null.And.Not.Empty);
        }

        [Test]
        public async Task Given_valid_request_when_ProcessPayment_and_repository_throws_exception_before_calling_bank_then_returns_500()
        {
            paymentRepositoryMock.Setup(x => x.Save(It.IsAny<Payment>())).Throws(new System.Exception());
            paymentRepositoryMock.Setup(x => x.Update(It.IsAny<Payment>())).Returns(Task.CompletedTask);
            bankClientMock.Setup(x => x.ProcessPayment(It.IsAny<BankPaymentRequest>())).ReturnsAsync(new BankPaymentResponseWithStatus()
            {
                StatusCode = HttpStatusCode.OK,
                ResponseBody = new BankPaymentResponse
                {
                    PaymentIdentifier = "someIdentifier",
                    PaymentErrorCode = null
                }
            }); ;

            var request = PaymentRequestBuilder.BuildValidPaymentRequest();


            var response = await client.Post<ProcessPaymentRequest>("/payments", request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.InternalServerError));
        }


        [Test]
        public async Task Given_wrong_identifier_when_Get_then_returns_404()
        {
            paymentRepositoryMock.Setup(x => x.Get(It.IsAny<string>())).ReturnsAsync((Payment)null);

            var response = await client.Get<PaymentDetails>("/payments/UNKNOWN");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        //[Test]
        //public async Task GetFoo_Default_Returns200OK()
        //{


        //    //this.paymentRepositoryMock.Setup(x => x.UtcNow).ReturnsAsync(new DateTimeOffset(2000, 1, 1));

        //    var response = await client.Get<string>("/weatherforecast");

        //    Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        //}


        private static Mock<T> GetMock<T>(TestServer testServer) where T : class
        {
            return (Mock<T>)testServer.Services.GetService(typeof(Mock<T>));
        }

    }
}