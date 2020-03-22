using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PaymentGateway.Services.Repositories;
using AutoMapper;
using PaymentGateway.Services.Services;
using PaymentGateway.Services.ServiceClients.AcquiringBankClient;

namespace PaymentGateway.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IConfiguration>(Configuration);

            // todo wrap in an extension method and consider AddScoped
            services.AddTransient<IPaymentRepository, PaymentRepository>();
            services.AddHttpClient<IBankClient, BankClient>();
            services.AddTransient<IStatusCodeConverter, StatusCodeConverter>();
            services.AddTransient<IAcquiringBankService, AcquiringBankService>();
            services.AddTransient<IPaymentValidationService, PaymentValidationService>();
            services.AddTransient<IPaymentService, PaymentService>();

            services.AddAutoMapper(typeof(Models.Payment)); //todo comment?

            services.AddControllers();
            services.AddApplicationInsightsTelemetry();
            services.AddHealthChecks();
        }

        public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                //Implement error page for productionW
            }

            app.UseHttpsRedirection();

            app.UseHealthChecks("/health");

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
