using Microsoft.Extensions.Configuration;
using System.IO;

namespace PaymentGateway.IntegrationTests.Configuration
{
    public class ConfigurationReader
    {
        public IConfiguration GetConfiguration()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.test.json")
                .Build();
            return config;
        }
    }
}
