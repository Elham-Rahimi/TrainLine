using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainLine.CurrencyConverter.Controllers;
using TrainLine.CurrencyConverter.Exceptions;
using TrainLine.CurrencyConverter.Models;
using TrainLine.CurrencyConverter.Services.CurrencyConverterService;
using Xunit;

namespace TrainLine.CurrencyConverterTest.Controllers
{
    public class PriceConversionControllerTests
    {
        private readonly Mock<ICurrencyConverterService> _mockCurrencyConverterService;
        private PriceConversionController _priceConversionController;

        public PriceConversionControllerTests()
        {
            _mockCurrencyConverterService = new Mock<ICurrencyConverterService>();
            _priceConversionController = new PriceConversionController(_mockCurrencyConverterService.Object);
        }

        [Fact]
        public async Task GIVEN_Valid_Request_WHEN_Called_THEN_Return_Success()
        {
            //Arrange
            var request = new PriceConversionRequest()
            {
                Price = 22,
                SourceCurrency = "USD",
                TargetCurrency = "GBP"
            };

            var serviceResult = new CurrencyConvertResult()
            {
                Price = 20,
                Currency = "GBP"
            };

            _mockCurrencyConverterService
                .Setup(x => x.ConvertAsync(request.Price, request.SourceCurrency, request.TargetCurrency))
                .Returns(Task.FromResult(serviceResult));

            //Act
            var response = await _priceConversionController.Get(request);

            //Assert
            Assert.NotNull(response.Result);
            Assert.IsType<OkObjectResult>(response.Result);

            var result = ((OkObjectResult)response.Result).Value as PriceConversionResult;
            Assert.Equal(serviceResult.Price, result.Price);
            Assert.Equal(serviceResult.Currency, result.Currency);

        }

        [Fact]
        public async Task GIVEN_Null_Response_WHEN_Called_THEN_Throw_Exception()
        {
            //Arrange
            var request = new PriceConversionRequest()
            {
                Price = 22,
                SourceCurrency = "GBP",
                TargetCurrency = "FKP"
            };

            _mockCurrencyConverterService
                .Setup(x => x.ConvertAsync(request.Price, request.SourceCurrency, request.TargetCurrency))
                .Returns(Task.FromResult((CurrencyConvertResult)null));

            //Act

            //Assert
            await Assert.ThrowsAsync<NullResponseException>(() => _priceConversionController.Get(request));
        }
    }
}
