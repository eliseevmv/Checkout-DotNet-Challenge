using Microsoft.Extensions.DependencyInjection;
using PaymentGateway.Services.Repositories;
using PaymentGateway.Services.ServiceClients.AcquiringBankClient;
using PaymentGateway.Services.Services;

namespace PaymentGateway.API.Infrastructure
{
    public static class IocConfiguration
    {
        public static void ConfigureDependencies(this IServiceCollection services)
        {
            services.AddTransient<IPaymentRepository, PaymentRepository>();
            services.AddHttpClient<IBankClient, BankClient>();
            services.AddTransient<IStatusCodeConverter, StatusCodeConverter>();
            services.AddTransient<IAcquiringBankService, AcquiringBankService>();
            services.AddTransient<IPaymentValidationService, PaymentValidationService>();
            services.AddTransient<IPaymentService, PaymentService>();
        }
    }
}
