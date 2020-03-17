using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using PaymentGateway.Services.ServiceClients.AcquiringBankClient.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace PaymentGateway.Services.ServiceClients
{
    public class BankClient : IBankClient
    {
        private const string BaseUrl = "https://localhost:44317/bankPayments/";  //todo configuration
        private readonly HttpClient _client;
        private readonly IConfiguration _configuration;

        public BankClient(HttpClient client, IConfiguration configuration)
        {
            _client = client;
            _configuration = configuration;
        }
         
        public async Task<BankPaymentResponse> ProcessPayment(BankPaymentRequest request)
        {
            var url = _configuration["Dependencies:AcquiringBank:ProcessPaymentEndpointUrl"];
            // todo authentication
            var content = JsonConvert.SerializeObject(request);
            var httpResponse = await _client.PostAsync(BaseUrl, new StringContent(content, System.Text.Encoding.Default, "application/json"));

            if (!httpResponse.IsSuccessStatusCode)
            {
                // todo are retries allowed?
                throw new Exception("Cannot make a payment"); // todo return error message to the client? analyse response code?
            }

            var response = JsonConvert.DeserializeObject<BankPaymentResponse>(await httpResponse.Content.ReadAsStringAsync());
            return response;
        }
    }
}
