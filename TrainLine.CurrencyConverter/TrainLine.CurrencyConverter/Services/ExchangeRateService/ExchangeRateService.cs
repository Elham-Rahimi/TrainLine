using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;
using System.Net;
using System.Threading.Tasks;
using TrainLine.CurrencyConverter.Exceptions;

namespace TrainLine.CurrencyConverter.Services.ExchangeRateService
{
    public class ExchangeRateService : IExchangeRateService
    {
        private readonly IRestClient _restClient;
        private IConfiguration _configuration;

        public ExchangeRateService(IRestClient restClient, IConfiguration configuration)
        {
            _restClient = restClient;
            _configuration = configuration;
        }

        public async Task<ExchangeRateResult> GetExchangeRateAsync(string sourceCurrency, string targetCurrency)
        {
            var exchangeResult = await CallApi(sourceCurrency);
            if (exchangeResult == null)
            {
                throw new NullResponseException();
            }
            if (string.IsNullOrEmpty(targetCurrency) || !exchangeResult.Rates.ContainsKey(targetCurrency))
            {
                throw new CurrencyNotSupportedException();
            }
            return new ExchangeRateResult
            {
                Rate = exchangeResult.Rates[targetCurrency]
            };
        }

        private async Task<ExchangeResponce> CallApi(string sourceCurrency)
        {
            var baseUrl = _configuration["ExchangeBaseUrl"].Replace("{CURRENCY}", sourceCurrency);
            //error if ExchangeBaseUrl doesn't exist
            var response = await _restClient.ExecuteAsync(new RestRequest(baseUrl));
            if (response == null)
            {
                throw new NullResponseException();
            }
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new CurrencyNotSupportedException();
            }

            var exchangeResult = JsonConvert.DeserializeObject<ExchangeResponce>(response.Content);
            //error on jason convert
            return exchangeResult;
        }
    }
}
