using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Moq;
using NUnit.Framework;
using PaymentGateway.Services.Repositories;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace PaymentGateway.ComponentTests
{
    public class ComponentTest //: CustomWebApplicationFactory<TestStartup>
    {
        private readonly HttpClient client;
        private readonly Mock<IPaymentRepository> paymentRepositoryMock;

        public ComponentTest()
        {
            var builder = new WebHostBuilder().UseStartup<TestStartup>();


            builder.ConfigureServices(services => {
          

            });
            var testServer = new TestServer(builder);
            this.client = testServer.CreateClient();
            //; ; this.client = this.CreateClient();

            var x = testServer.Services.GetService(typeof(Mock<IPaymentRepository>));
            paymentRepositoryMock = (Mock<IPaymentRepository>)x;

        }

        [Test]
        public async Task tmp()
        {
            paymentRepositoryMock.Setup(x => x.Get(It.IsAny<string>())).ReturnsAsync(new Services.Entities.Payment() { });

            //this.paymentRepositoryMock.Setup(x => x.UtcNow).ReturnsAsync(new DateTimeOffset(2000, 1, 1));

            var response = await client.GetAsync("/payments/UNKNOWN");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public async Task GetFoo_Default_Returns200OK()
        {


            //this.paymentRepositoryMock.Setup(x => x.UtcNow).ReturnsAsync(new DateTimeOffset(2000, 1, 1));

            var response = await client.GetAsync("/weatherforecast");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }
    }
}