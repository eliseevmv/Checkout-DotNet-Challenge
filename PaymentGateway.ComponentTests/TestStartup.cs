using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moq;
using PaymentGateway.ComponentTests.Infrastructure;
using PaymentGateway.Services.Repositories;
using PaymentGateway.Services.ServiceClients.AcquiringBankClient;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace PaymentGateway.ComponentTests
{
    public class TestStartup : Startup
    {
        private readonly Mock<IPaymentRepository> _paymentRepository;
        private readonly Mock<IBankClient> _bankClient;

        public TestStartup(IConfiguration configuration)
            : base(configuration)
        {
            this._paymentRepository = new Mock<IPaymentRepository>(MockBehavior.Strict);
            this._bankClient = new Mock<IBankClient>(MockBehavior.Strict);
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().AddApplicationPart(Assembly.Load(new AssemblyName("PaymentGateway")));

            services.AddSingleton(this._paymentRepository);  
            services.AddSingleton(this._bankClient);  

            base.ConfigureServices(services);

            services.AddSingleton(this._paymentRepository.Object);
            services.AddSingleton(this._bankClient.Object);
        }

        public override void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<ExceptionMiddleware>();

            base.Configure(app, env);
        }
    }
}
