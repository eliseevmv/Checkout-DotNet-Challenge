using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace PaymentGateway.ComponentTests.Infrastructure
{
    public class PaymentGatewayClient 
    {
        private readonly HttpClient _client;

        public PaymentGatewayClient(HttpClient client)
        {
            _client = client;
        }
         
        public async Task<ResponseWithStatus<TResponse>> Post<TRequest,TResponse>(string url, TRequest request)
        {
            var responseBodyAndStatusCode = await Post<TRequest>(url, request);

            return new ResponseWithStatus<TResponse>() 
            {
                ResponseBody = JsonConvert.DeserializeObject<TResponse>(responseBodyAndStatusCode.ResponseBody),
                StatusCode = responseBodyAndStatusCode.StatusCode
            };
        }

        public async Task<ResponseWithStatus<string>> Post<TRequest>(string url, TRequest request)
        {
            var content = JsonConvert.SerializeObject(request);
            var httpResponse = await _client.PostAsync(url, new StringContent(content, System.Text.Encoding.Default, "application/json"));

            string response = await httpResponse.Content.ReadAsStringAsync();

            return new ResponseWithStatus<string>()
            {
                ResponseBody = response,
                StatusCode = httpResponse.StatusCode
            };
        }

        public async Task<ResponseWithStatus<TResponse>> Get<TResponse>(string url)
        {
            var httpResponse = await _client.GetAsync(url);

            var response = JsonConvert.DeserializeObject<TResponse>(await httpResponse.Content.ReadAsStringAsync());

            return new ResponseWithStatus<TResponse>()
            {
                ResponseBody = response,
                StatusCode = httpResponse.StatusCode
            };
        }

    }
}
