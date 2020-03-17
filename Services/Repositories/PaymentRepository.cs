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
            string connectionString = _configuration.GetConnectionString("DB"); // todo improve
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                return await db.QuerySingleOrDefaultAsync<Payment>(
                    "SELECT * FROM dbo.Payments WHERE PaymentIdentifier = @PaymentIdentifier", //todo select *
                    new { paymentIdentifier });
            }

            //if (paymentIdentifier.First() == '1')
            //{
            //    return new Payment(
            //    {
            //        PaymentIdentifier = paymentIdentifier,
            //        StatusCode = PaymentStatusCode.FailureReason1
            //    };
            //}
            //var payment = new Payment
            //(
            //    Guid.NewGuid().ToString(),
            //    PaymentStatusCode.Success,
            //    123,
            //    "GBP",
            //    "4343*****3433",
            //    "1220",
            //    "234",
            //    Guid.NewGuid()
            //);
            //return Task.FromResult(payment);
        }

        public Task Save(Payment payment)
        {
            throw new NotImplementedException();
        }
    }
}
