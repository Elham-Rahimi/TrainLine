using Moq;
using System.Threading.Tasks;
using TrainLine.CurrencyConverter.Exceptions;
using TrainLine.CurrencyConverter.Services.CurrencyConverterService;
using TrainLine.CurrencyConverter.Services.ExchangeRateService;
using Xunit;

namespace TrainLine.CurrencyConverterTest.Services
{
    public class CurrencyConverterServiceTests
    {
        private readonly Mock<IExchangeRateService> _mockExchangeRateService;
        private CurrencyConverterService _currencyConverterService;

        public CurrencyConverterServiceTests()
        {
            _mockExchangeRateService = new Mock<IExchangeRateService>();
            _currencyConverterService = new CurrencyConverterService(_mockExchangeRateService.Object);
        }

        [Theory]
        [InlineData(1, "GBP", "EUR")]
        [InlineData(2, "EUR", "USD")]
        [InlineData(3, "USD", "GBP")]
        public async Task GIVEN_Valid_Input_WHEN_Called_ConvertAsync_THEN_Return_Converted_Price(decimal price, string sourceCurrency, string targetCurrency)
        {
            //Arrange
            var exchangeRateResult = new ExchangeRateResult()
            {
                Rate = 2
            };

            _mockExchangeRateService
                .Setup(x => x.GetExchangeRateAsync(sourceCurrency, targetCurrency))
                .Returns(Task.FromResult(exchangeRateResult));

            //Act
            var response = await _currencyConverterService.ConvertAsync(price, sourceCurrency, targetCurrency);

            //Assert
            Assert.NotNull(response);
            Assert.Equal(price * exchangeRateResult.Rate, response.Price);
            Assert.Equal(targetCurrency, response.Currency);
        }

        [Fact]
        public async Task GIVEN_Null_Response_WHEN_Called_ConvertAsync_THEN_Throw_Exception()
        {
            //Arrange
            var price = 10;
            var sourceCurrency = "USD";
            var targetCurrency = "GBP";
            _mockExchangeRateService
                .Setup(x => x.GetExchangeRateAsync(sourceCurrency, targetCurrency))
                .Returns(Task.FromResult((ExchangeRateResult)null));

            //Act
            //Assert
            await Assert.ThrowsAsync<NullResponseException>(()
                => _currencyConverterService.ConvertAsync(price, sourceCurrency, targetCurrency));
        }
    }
}
