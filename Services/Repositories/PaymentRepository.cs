using Microsoft.Extensions.Configuration;
using PaymentGateway.Services.Entities;
using Dapper;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace PaymentGateway.Services.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly IConfiguration _configuration;

        public PaymentRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<Payment> Get(string paymentIdentifier)
        {
            var connectionString = _configuration.GetConnectionString("DB"); // todo improve
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                return await db.QuerySingleOrDefaultAsync<Payment>(
                    "SELECT * FROM dbo.Payments WHERE PaymentIdentifier = @PaymentIdentifier", //todo select *
                    new { paymentIdentifier });
            }
        }

        public async Task Save(Payment payment)
        {
            var connectionString = _configuration.GetConnectionString("DB"); // todo improve
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                await db.ExecuteAsync(
                    "INSERT INTO dbo.Payments (PaymentIdentifier,StatusCode,Amount,Currency,MaskedCardNumber,ExpiryMonthAndDate,Cvv,MerchantId) " +
                    "VALUES(@PaymentIdentifier,@StatusCode,@Amount,@Currency,@MaskedCardNumber,@ExpiryMonthAndDate,@Cvv,@MerchantId)", 
                    payment);
            }
        }
    }
}
