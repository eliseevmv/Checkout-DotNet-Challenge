using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moq;
using PaymentGateway.Services.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentGateway.ComponentTests
{
    public class TestStartup : Startup
    {
        private readonly Mock<IPaymentRepository> _paymentRepository;

        public TestStartup(IConfiguration configuration)
            : base(configuration)
        {
            this._paymentRepository = new Mock<IPaymentRepository>(MockBehavior.Strict);
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            services
                .AddSingleton(this._paymentRepository);  //todo remove?

            base.ConfigureServices(services);

            services
                .AddSingleton(this._paymentRepository.Object);
        }
    }
}
