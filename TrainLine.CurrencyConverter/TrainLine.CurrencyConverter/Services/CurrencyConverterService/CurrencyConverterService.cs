using System.Threading.Tasks;
using TrainLine.CurrencyConverter.Exceptions;
using TrainLine.CurrencyConverter.Services.ExchangeRateService;

namespace TrainLine.CurrencyConverter.Services.CurrencyConverterService
{
    public class CurrencyConverterService : ICurrencyConverterService
    {
        private readonly IExchangeRateService _exchangeRateService;

        public CurrencyConverterService(IExchangeRateService exchangeRateService)
        {
            _exchangeRateService = exchangeRateService;
        }

        public async Task<CurrencyConvertResult> ConvertAsync(decimal price, string sourceCurrency, string targetCurrency)
        {
            var exchangeRate = await _exchangeRateService.GetExchangeRateAsync(sourceCurrency, targetCurrency);
            if (exchangeRate == null)
            {
                throw new NullResponseException();
            }
            var convertedPrice = price * exchangeRate.Rate;

            return new CurrencyConvertResult
            {
                Price = convertedPrice,
                Currency = targetCurrency
            };
        }
    }
}
