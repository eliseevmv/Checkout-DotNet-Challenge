using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Azure;
using PaymentGateway.Services.Repositories;
using AutoMapper;
using PaymentGateway.Services.ServiceClients;
using PaymentGateway.Services.Services;
using PaymentGateway.Services.ServiceClients.AcquiringBankClient;

namespace PaymentGateway
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IConfiguration>(Configuration);

            // todo wrap in an extension method and consider AddScoped
            services.AddTransient<IPaymentRepository, PaymentRepository>();
            services.AddHttpClient<IBankClient, BankClient>();
            services.AddTransient<IStatusCodeConverter, StatusCodeConverter>();
            services.AddTransient<IAcquiringBankService, AcquiringBankService>();

            services.AddAutoMapper(typeof(Models.PaymentDetails)); //todo comment?

            services.AddControllers();
            services.AddApplicationInsightsTelemetry();

            //todo removr
            services.AddAzureClients(builder =>
            {
                builder.AddBlobServiceClient(Configuration["ConnectionStrings:Storage1"]);
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
