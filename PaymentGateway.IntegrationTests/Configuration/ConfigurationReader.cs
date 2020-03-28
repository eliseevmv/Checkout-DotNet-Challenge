using Microsoft.Extensions.Configuration;
using System.IO;

namespace PaymentGateway.IntegrationTests.Configuration
{
    public class ConfigurationReader
    {
        public string Get(string key)
        {
            var config = GetConfiguration();
            return config[key];
        }

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
