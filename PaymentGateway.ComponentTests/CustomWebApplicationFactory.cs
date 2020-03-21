using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using PaymentGateway.Services.Repositories;
using System;
using System.Net.Http;

namespace PaymentGateway.ComponentTests
{
    public class CustomWebApplicationFactory<TEntryPoint> : WebApplicationFactory<TEntryPoint>
        where TEntryPoint : class
    {
        public CustomWebApplicationFactory()
        {
        }

        public Mock<IPaymentRepository> PaymentRepositoryMock { get; private set; }

        public void VerifyAllMocks() => Mock.VerifyAll(this.PaymentRepositoryMock); // todo consider removing

        protected override void ConfigureClient(HttpClient client)
        {
            using (var serviceScope = this.Services.CreateScope())
            {
                var serviceProvider = serviceScope.ServiceProvider;
                this.PaymentRepositoryMock = serviceProvider.GetRequiredService<Mock<IPaymentRepository>>();
            }

            base.ConfigureClient(client);
        }

        protected override IWebHostBuilder CreateWebHostBuilder()
        {
            return base.CreateWebHostBuilder();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder) =>
            builder
            //    .UseEnvironment("Test")
                .UseStartup<TEntryPoint>();

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.VerifyAllMocks();
            }

            base.Dispose(disposing);
        }
    }
}
