using CreditCardPaymentAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Json;
using System.Text.Json;

namespace CreditCardPaymentAPI.Services
{
    public class CreditCardEventProducer : ICreditCardEventProducer
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public CreditCardEventProducer(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task ProduceCreditCardEventAsync(CreditCardModel creditCardModel)
        {
            var client = _httpClientFactory.CreateClient("Test");
           
            var response = await client.PostAsJsonAsync("Test", creditCardModel);
     
            response.EnsureSuccessStatusCode();
         
        }
    }
}
