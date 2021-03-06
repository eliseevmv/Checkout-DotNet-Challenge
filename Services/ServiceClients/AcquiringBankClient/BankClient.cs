﻿using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using PaymentGateway.Services.ServiceClients.AcquiringBankClient.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace PaymentGateway.Services.ServiceClients.AcquiringBankClient
{
    // ASSUMPTIONS
    // Acquiring bank exposes a RESTful HTTP API
    // The payment processing endpoint
    //  - returns 200 and payment identifier for successful requests, 
    //  - returns 4xx/5xx, payment identifier and error code for failed requests 
    public class BankClient : IBankClient
    {
        private readonly HttpClient _client;
        private readonly IConfiguration _configuration;

        public BankClient(HttpClient client, IConfiguration configuration)
        {
            _client = client;
            _configuration = configuration;
        }
         
        public async Task<BankPaymentResponseWithStatus> ProcessPayment(BankPaymentRequest request)
        {
            var url = _configuration["Dependencies:AcquiringBank:ProcessPaymentEndpointUrl"];

            var content = JsonConvert.SerializeObject(request);
            var httpResponse = await _client.PostAsync(url, new StringContent(content, System.Text.Encoding.Default, "application/json"));

            // This class should also support non-json responses (eg an exception)

            var response = JsonConvert.DeserializeObject<BankPaymentResponse>(await httpResponse.Content.ReadAsStringAsync());
            
            return new BankPaymentResponseWithStatus() 
            {
                ResponseBody = response,
                StatusCode = httpResponse.StatusCode
            };
        }
    }
}
