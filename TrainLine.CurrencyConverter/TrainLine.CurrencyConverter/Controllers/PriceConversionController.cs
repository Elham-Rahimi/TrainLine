using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainLine.CurrencyConverter.Exceptions;
using TrainLine.CurrencyConverter.Models;
using TrainLine.CurrencyConverter.Services.CurrencyConverterService;

namespace TrainLine.CurrencyConverter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PriceConversionController : ControllerBase
    {
        private readonly ICurrencyConverterService _currencyConverterService;

        public PriceConversionController(ICurrencyConverterService currencyConverterService)
        {
            _currencyConverterService = currencyConverterService;
        }

        [HttpGet]
        public async Task<ActionResult<PriceConversionResult>> Get([FromQuery] PriceConversionRequest priceConversionRequest)
        {

            var priceConversionResult = await _currencyConverterService.ConvertAsync(
                priceConversionRequest.Price,
                priceConversionRequest.SourceCurrency,
                priceConversionRequest.TargetCurrency);

            if (priceConversionResult == null)
            {
                throw new NullResponseException();
            }

            return Ok(new PriceConversionResult()
            {
                Price = priceConversionResult.Price,
                Currency = priceConversionResult.Currency
            });
        }
    }
}
