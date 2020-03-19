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

        public async Task<Payment> Get(string paymentId)
        {
            var connectionString = _configuration.GetConnectionString("DB"); // todo improve
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                return await db.QuerySingleOrDefaultAsync<Payment>(
                    "SELECT * FROM dbo.Payments WHERE PaymentId = @PaymentId", //todo select *
                    new { paymentId });
            }
        }

        public async Task Save(Payment payment)
        {
            var connectionString = _configuration.GetConnectionString("DB"); // todo improve
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                await db.ExecuteAsync(
                    "INSERT INTO dbo.Payments (PaymentId,AcquringBankPaymentId,StatusCode,Amount,Currency,MaskedCardNumber,ExpiryMonthAndDate,Cvv,MerchantId) " +
                    "VALUES(@PaymentId,@AcquringBankPaymentId,@StatusCode,@Amount,@Currency,@MaskedCardNumber,@ExpiryMonthAndDate,@Cvv,@MerchantId)", 
                    payment);
            }
        }

        public async Task Update(Payment payment)
        {
            var connectionString = _configuration.GetConnectionString("DB"); // todo improve
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                await db.ExecuteAsync(
                    "UPDATE dbo.Payments SET " +
                    " AcquringBankPaymentId = @AcquringBankPaymentId," +
                    " StatusCode = @StatusCode," +
                    " Amount = @Amount," +
                    " Currency = @Currency," +
                    " MaskedCardNumber = @MaskedCardNumber," +
                    " ExpiryMonthAndDate = @ExpiryMonthAndDate," +
                    " Cvv = @Cvv," +
                    " MerchantId = @MerchantId " +
                    "WHERE PaymentId = @PaymentId",
                    payment);
            }
        }

    }
}
