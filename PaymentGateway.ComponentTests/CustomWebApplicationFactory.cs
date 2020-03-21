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
            this.ClientOptions.AllowAutoRedirect = false;
         //   this.ClientOptions.BaseAddress = new Uri("https://localhost");
        }

//        public ApplicationOptions ApplicationOptions { get; private set; } // todo consider removing

        public Mock<IPaymentRepository> PaymentRepositoryMock { get; private set; }

        public void VerifyAllMocks() => Mock.VerifyAll(this.PaymentRepositoryMock); // todo consider removing

        protected override void ConfigureClient(HttpClient client)
        {
            using (var serviceScope = this.Services.CreateScope())
            {
                var serviceProvider = serviceScope.ServiceProvider;
            //    this.ApplicationOptions = serviceProvider.GetRequiredService<IOptions<ApplicationOptions>>().Value;
                this.PaymentRepositoryMock = serviceProvider.GetRequiredService<Mock<IPaymentRepository>>();
            }

            base.ConfigureClient(client);
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder) =>
            builder
            //    .UseEnvironment("Test")
                .UseStartup<TestStartup>();

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
