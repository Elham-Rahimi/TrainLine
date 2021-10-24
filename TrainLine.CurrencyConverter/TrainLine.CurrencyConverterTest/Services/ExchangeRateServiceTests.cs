using Microsoft.Extensions.Configuration;
using Moq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TrainLine.CurrencyConverter.Exceptions;
using TrainLine.CurrencyConverter.Services.ExchangeRateService;
using Xunit;

namespace TrainLine.CurrencyConverterTest.Services
{
    public class ExchangeRateServiceTests
    {
        private Mock<IRestClient> _mockRestClient;
        private Mock<IConfiguration> _mockConfiguration;
        private ExchangeRateService _exchangeRateService;

        public ExchangeRateServiceTests()
        {
            _mockRestClient = new Mock<IRestClient>();
            _mockConfiguration = new Mock<IConfiguration>();
            _exchangeRateService = new ExchangeRateService(_mockRestClient.Object, _mockConfiguration.Object);
        }

        [Theory]
        [InlineData("GBP", "EUR")]
        [InlineData("EUR", "USD")]
        [InlineData("USD", "GBP")]
        public async Task GIVEN_Valid_Input_WHEN_Called_GetExchangeRateAsync_THEN_Return_Result(string sourceCurrency, string targetCurrency)
        {
            //Arrange
            var url = "http://somthing.com/";
            _mockConfiguration
                .Setup(x => x[It.Is<string>(s => s == "ExchangeBaseUrl")])
                .Returns(url);

            _mockRestClient
                .Setup(x => x.ExecuteAsync(It.IsAny<IRestRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new RestResponse
                {
                    Content = ExpectedJsonResult(),
                    StatusCode = HttpStatusCode.OK,
                });

            //Act
            var response = await _exchangeRateService.GetExchangeRateAsync(sourceCurrency, targetCurrency);

            //Assert
            Assert.NotNull(response);
        }

        [Fact]
        public async Task GIVEN_Valid_Input_WHEN_Called_GetExchangeRateAsync_THEN_Return_Exchange_Rate()
        {
            //Arrange
            var sourceCurrency = "EUR";
            var targetCurrency = "USD";
            var url = "http://somthing.com/";

            _mockConfiguration
                .Setup(x => x[It.Is<string>(s => s == "ExchangeBaseUrl")])
                .Returns(url);

            _mockRestClient
                .Setup(x => x.ExecuteAsync(It.IsAny<IRestRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new RestResponse
                {
                    Content = ExpectedJsonResult(),
                    StatusCode = HttpStatusCode.OK,
                });

            //Act
            var response = await _exchangeRateService.GetExchangeRateAsync(sourceCurrency, targetCurrency);

            //Assert
            Assert.NotNull(response);
            Assert.Equal((decimal)1.183894, response.Rate);
        }

        [Fact]
        public async Task GIVEN_Invalid_targetCurrency_WHEN_Called_GetExchangeRateAsync_THEN_Throw_Exception()
        {
            //Arrange
            var sourceCurrency = "USD";
            var targetCurrency = "NNN";
            var url = "http://somthing.com/";

            _mockConfiguration
                .SetupGet(x => x[It.Is<string>(s => s == "ExchangeBaseUrl")])
                .Returns(url);

            _mockRestClient
               .Setup(x => x.ExecuteAsync(It.IsAny<IRestRequest>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(new RestResponse
               {
                   Content = ExpectedJsonResult(),
                   StatusCode = HttpStatusCode.OK,
               });

            //Act
            //Assert
            await Assert.ThrowsAsync<CurrencyNotSupportedException>(()
                => _exchangeRateService.GetExchangeRateAsync(sourceCurrency, targetCurrency));
        }

        [Fact]
        public async Task GIVEN_Invalid_sourceCurrency_WHEN_Called_GetExchangeRateAsync_THEN_Throw_Exception()
        {
            //Arrange
            var sourceCurrency = "NNN";
            var targetCurrency = "USD";
            var url = "http://somthing.com/";

            _mockConfiguration
                .SetupGet(x => x[It.Is<string>(s => s == "ExchangeBaseUrl")])
                .Returns(url);

            _mockRestClient
               .Setup(x => x.ExecuteAsync(It.IsAny<IRestRequest>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(new RestResponse
               {
                   StatusCode = HttpStatusCode.NotFound,
               });

            //Act
            //Assert
            await Assert.ThrowsAsync<CurrencyNotSupportedException>(()
                => _exchangeRateService.GetExchangeRateAsync(sourceCurrency, targetCurrency));
        }

        [Fact]
        public async Task GIVEN_null_sourceCurrency_WHEN_Called_GetExchangeRateAsync_THEN_Throw_Exception()
        {
            //Arrange
            string sourceCurrency = null;
            var targetCurrency = "USD";
            var url = "http://somthing.com/";

            _mockConfiguration
                .SetupGet(x => x[It.Is<string>(s => s == "ExchangeBaseUrl")])
                .Returns(url);

            _mockRestClient
               .Setup(x => x.ExecuteAsync(It.IsAny<IRestRequest>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(new RestResponse
               {
                   StatusCode = HttpStatusCode.NotFound,
               });

            //Act
            //Assert
            await Assert.ThrowsAsync<CurrencyNotSupportedException>(()
                => _exchangeRateService.GetExchangeRateAsync(sourceCurrency, targetCurrency));
        }

        [Fact]
        public async Task GIVEN_null_targetCurrency_WHEN_Called_GetExchangeRateAsync_THEN_Throw_Exception()
        {
            //Arrange
            var sourceCurrency = "GBP";
            string targetCurrency = null;
            var url = "http://somthing.com/";

            _mockConfiguration
                .SetupGet(x => x[It.Is<string>(s => s == "ExchangeBaseUrl")])
                .Returns(url);

            _mockRestClient
               .Setup(x => x.ExecuteAsync(It.IsAny<IRestRequest>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(new RestResponse
               {
                   Content = ExpectedJsonResult(),
                   StatusCode = HttpStatusCode.OK,
               });

            //Act
            //Assert
            await Assert.ThrowsAsync<CurrencyNotSupportedException>(()
                => _exchangeRateService.GetExchangeRateAsync(sourceCurrency, targetCurrency));
        }

        [Fact]
        public async Task GIVEN_null_Api_Respond_WHEN_Called_GetExchangeRateAsync_THEN_Throw_Exception()
        {
            //Arrange
            var sourceCurrency = "USD";
            var targetCurrency = "NNN";
            var url = "http://somthing.com/";
            _mockConfiguration
                .SetupGet(x => x[It.Is<string>(s => s == "ExchangeBaseUrl")])
                .Returns(url);
            _mockRestClient
               .Setup(x => x.ExecuteAsync(It.IsAny<IRestRequest>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync((RestResponse)null);

            //Act
            //Assert
            await Assert.ThrowsAsync<NullResponseException>(()
                => _exchangeRateService.GetExchangeRateAsync(sourceCurrency, targetCurrency));
        }

        private string ExpectedJsonResult()
        {

            return @"{
	            'base': 'EUR',
                'date': '2021-04-07',
	            'time_last_updated': 1617753602,
	            'rates': {
                    'GBP': 0.855552,
		            'EUR': 1,
		            'USD': 1.183894
                    }
                }";
        }
    }
}
