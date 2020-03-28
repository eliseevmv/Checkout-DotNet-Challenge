using Newtonsoft.Json;
using PaymentGateway.Client.Models;
using System.Net.Http;
using System.Threading.Tasks;

namespace PaymentGateway.Client
{
    public class PaymentGatewayClient     
    {
        private readonly HttpClient _httpClient;

        // HttpClient is injected in order to encourage reusing of the same HTTP Client
        // https://aspnetmonsters.com/2016/08/2016-08-27-httpclientwrong/
        // https://josef.codes/you-are-probably-still-using-httpclient-wrong-and-it-is-destabilizing-your-software/
        // https://docs.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/use-httpclientfactory-to-implement-resilient-http-requests
        // IoC registration example: 
        // services.AddHttpClient<PaymentGatewayClient>(x => { x.BaseAddress = new Uri(myBaseUrl); });

        public PaymentGatewayClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
         
        public async Task<ResponseWithHttpStatusCode<ProcessPaymentResponse>> ProcessPayment(ProcessPaymentRequest request)
        {
            var content = JsonConvert.SerializeObject(request);
            
            var httpResponse = await _httpClient.PostAsync($"payments", new StringContent(content, System.Text.Encoding.Default, "application/json"));
            
            var response = JsonConvert.DeserializeObject<ProcessPaymentResponse>(await httpResponse.Content.ReadAsStringAsync());

            return new ResponseWithHttpStatusCode<ProcessPaymentResponse>(response, httpResponse.StatusCode);
        }

        public async Task<ResponseWithHttpStatusCode<Payment>> Get(string paymentId)
        {
            var httpResponse = await _httpClient.GetAsync($"payments/{paymentId}");

            var response = JsonConvert.DeserializeObject<Payment>(await httpResponse.Content.ReadAsStringAsync());

            return new ResponseWithHttpStatusCode<Payment>(response, httpResponse.StatusCode);
        }
    }

}
