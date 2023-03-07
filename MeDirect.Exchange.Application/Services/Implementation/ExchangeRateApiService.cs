using MeDirect.Exchange.Application.Responses;
using MeDirect.Exchange.Application.Services.Interfaces;
using MeDirect.Exchange.Domain.Contracts;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MeDirect.Exchange.Application.Services.Implementation
{
    public class ExchangeRateApiService : IExchangeRateApiService
    {
        private readonly HttpClient _httpClient;
        protected readonly IConfiguration Configuration;

        public ExchangeRateApiService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            Configuration = configuration;
            _httpClient.DefaultRequestHeaders.Add("apikey", Configuration.GetSection("ApiData")["apiKey"]);
            _httpClient.BaseAddress = new Uri(Configuration.GetSection("ApiData")["baseUrl"]);
        }

        public async Task<ExchangeRateResponse> GetExchangeRateAsync(string baseCurrency, string targetCurrency, decimal amount)
        {
            var response = await _httpClient.GetAsync($"convert?to={targetCurrency}&from={baseCurrency}&amount={amount}");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to get exchange rate from API. Status code: {response.StatusCode}");
            }

            var content = await response.Content.ReadAsStringAsync();

            var exchangeRateResponse = JsonConvert.DeserializeObject<ExchangeRateResponse>(content);

            return exchangeRateResponse;
        }

        public async Task<List<CurrencyDto>> GetCurrencyListAsync()
        {
            var response = await _httpClient.GetAsync($"symbols");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to get exchange rate from API. Status code: {response.StatusCode}");
            }

            var content = await response.Content.ReadAsStringAsync();

            JObject jsonObj = JObject.Parse(content);
            var currencies = jsonObj["symbols"]
                            .Children<JProperty>()
                            .Select(prop => new CurrencyDto
                            {
                                Code = prop.Name,
                                Name = prop.Value.ToString()
                            })
                            .ToList();

            return currencies;
        }
    }

}
