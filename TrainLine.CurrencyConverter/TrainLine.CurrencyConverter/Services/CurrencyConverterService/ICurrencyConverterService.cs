using System.Threading.Tasks;

namespace TrainLine.CurrencyConverter.Services.CurrencyConverterService
{
    public interface ICurrencyConverterService
    {
        Task<CurrencyConvertResult> ConvertAsync(decimal price, string sourceCurrency, string targetCurrency);
    }
}
