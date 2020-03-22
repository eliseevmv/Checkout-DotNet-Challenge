using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using PaymentGateway.API;
using PaymentGateway.ComponentTests.Infrastructure;
using PaymentGateway.Services.Repositories;
using PaymentGateway.Services.ServiceClients.AcquiringBankClient;
using System.Reflection;

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
            // If a controllers throws an exception, the middleware ensures that tests receive 
            // not an exception but a proper HTTP response with code 500

            base.Configure(app, env);
        }
    }
}
