using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using PaymentGateway.ComponentTests.Builders;
using PaymentGateway.ComponentTests.Infrastructure;
using PaymentGateway.Models;
using PaymentGateway.Services.Entities;
using PaymentGateway.Services.Repositories;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace PaymentGateway.ComponentTests
{
    public class ComponentTest //: CustomWebApplicationFactory<TestStartup>
    {
        private readonly PaymentGatewayClient client;
        private readonly Mock<IPaymentRepository> paymentRepositoryMock;

        public ComponentTest()
        {
            var builder = new WebHostBuilder().UseStartup<TestStartup>();


            builder.ConfigureServices(services => {
          

            });
            var testServer = new TestServer(builder);
            this.client = new PaymentGatewayClient( testServer.CreateClient());

            var x = testServer.Services.GetService(typeof(Mock<IPaymentRepository>));
            paymentRepositoryMock = (Mock<IPaymentRepository>)x;

        }

        [Test]
        public async Task tmp2()
        {
            paymentRepositoryMock.Setup(x => x.Save(It.IsAny<Payment>())).Returns(Task.CompletedTask);
            paymentRepositoryMock.Setup(x => x.Update(It.IsAny<Payment>())).Returns(Task.CompletedTask);

            var request = PaymentRequestBuilder.BuildValidPaymentRequest();

            var response = await client.Post<ProcessPaymentRequest, ProcessPaymentResponse>("/payments", request);


            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
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
    }
}