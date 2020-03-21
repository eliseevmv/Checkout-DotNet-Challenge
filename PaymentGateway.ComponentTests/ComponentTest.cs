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
    public class ComponentTest : CustomWebApplicationFactory<Startup>
    {
        private readonly HttpClient client;
        private readonly Mock<IPaymentRepository> paymentRepositoryMock;

        public ComponentTest()
        {
            var builder = new WebHostBuilder().UseStartup<Startup>();

            var testServer = new TestServer(builder);
            this.client = testServer.CreateClient();

            this.paymentRepositoryMock = this.PaymentRepositoryMock;
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