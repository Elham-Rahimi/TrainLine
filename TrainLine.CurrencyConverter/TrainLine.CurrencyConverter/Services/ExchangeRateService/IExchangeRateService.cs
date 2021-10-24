using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrainLine.CurrencyConverter.Services.ExchangeRateService
{
    public interface IExchangeRateService
    {
        Task<ExchangeRateResult> GetExchangeRateAsync(string sourceCurrency, string targetCurrency);
    }
}
