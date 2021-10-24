using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using TrainLine.CurrencyConverter;
using TrainLine.CurrencyConverter.Models;
using Xunit;

namespace TrainLine.CurrencyConverterTest.Integration
{
    public class CurrencyConverterIntegrationTest : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public CurrencyConverterIntegrationTest(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GIVEN_Valid_input_WHEN_Call_Api_THEN_Successful()
        {
            //Arrange
            var client = _factory.CreateClient();
            decimal price = 24;
            var sourceCurrency = "USD";
            var targetCurrency = "GBP";
            var requestUrl = GenerateRequestUrl(price, sourceCurrency, targetCurrency);

            //Act
            var response = await client.GetAsync(requestUrl);

            //Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var priceConversionResultJson = await response.Content.ReadAsStringAsync();
            var priceConversionResult = JsonSerializer.Deserialize<PriceConversionResult>(priceConversionResultJson);

            Assert.Equal(targetCurrency, priceConversionResult.Currency);

        }

        [Theory]
        [InlineData("GBP", "NNN")]
        [InlineData("YYY", "USD")]
        [InlineData("www", "rrr")]
        public async Task GIVEN_Invalid_Currency_input_WHEN_Call_Api_THEN_Successful(string sourceCurrency, string targetCurrency)
        {
            //Arrange
            var client = _factory.CreateClient();
            decimal price = 24;
            var requestUrl = GenerateRequestUrl(price, sourceCurrency, targetCurrency);

            //Act
            var response = await client.GetAsync(requestUrl);

            //Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        private string GenerateRequestUrl(decimal price, string sourceCurrency, string targetCurrency)
        {
            var url = $"/api/PriceConversion?Price={price}&SourceCurrency={sourceCurrency}&TargetCurrency={targetCurrency}";
            return url;
        }
    }
}
