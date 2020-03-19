using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using PaymentGateway.IntegrationTests.Configuration;
using PaymentGateway.IntegrationTests.ServiceClient.Models;
using System.Net.Http;
using System.Threading.Tasks;

namespace PaymentGateway.Services.ServiceClients.AcquiringBankClient
{
    public class PaymentGatewayClient     
    {
        private readonly HttpClient _client;
        private readonly string _baseUrl;

        public PaymentGatewayClient()
        {
            _client = new HttpClient();
            var _configuration = new ConfigurationReader().GetConfiguration();
            _baseUrl = _configuration["Dependencies:PaymentGateway:BaseUrl"];
        }
         
        public async Task<ProcessPaymentResponse> ProcessPayment(ProcessPaymentRequest request)
        {
            var content = JsonConvert.SerializeObject(request);
            
            var httpResponse = await _client.PostAsync($"{_baseUrl}/payments", new StringContent(content, System.Text.Encoding.Default, "application/json"));
            
            httpResponse.EnsureSuccessStatusCode();

            var response = JsonConvert.DeserializeObject<ProcessPaymentResponse>(await httpResponse.Content.ReadAsStringAsync());
            return response;
        }

        public async Task<PaymentDetails> Get(string paymentId)
        {
            var httpResponse = await _client.GetAsync($"{_baseUrl}/payments/{paymentId}");

            httpResponse.EnsureSuccessStatusCode();

            var response = JsonConvert.DeserializeObject<PaymentDetails>(await httpResponse.Content.ReadAsStringAsync());
            return response;
        }
    }
}
