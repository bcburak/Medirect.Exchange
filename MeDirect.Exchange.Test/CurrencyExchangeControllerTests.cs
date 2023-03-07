using MeDirect.Exchange.API.Controllers;
using MeDirect.Exchange.Application.Responses;
using MeDirect.Exchange.Application.Services.Interfaces;
using MeDirect.Exchange.Domain.Contracts;
using MeDirect.Exchange.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace MeDirect.Exchange.Test
{
    public class CurrencyExchangeControllerTests
    {

        private readonly ILogger<CurrencyExchangeController> _logger;
        private readonly Mock<ICurrencyExchangeService> _mockCurrencyExchangeService;
        private readonly CurrencyExchangeController _currencyExchangeController;

        public CurrencyExchangeControllerTests()
        {
            _mockCurrencyExchangeService = new Mock<ICurrencyExchangeService>();
            _currencyExchangeController = new CurrencyExchangeController(_logger, _mockCurrencyExchangeService.Object);
        }

        [Fact]
        public async Task ExecuteCurrencyExchange_ShouldReturnOkObjectResult_WhenServiceReturnsSuccessfulResponse()
        {
            // Arrange
            var baseCurrency = "EUR";
            var targetCurrency = "USD";
            var amount = 100;
            var userId = 1;
            var transactionDto = new TransactionDto
            {
                BaseCurrency = baseCurrency,
                TargetCurrency = targetCurrency,
                Amount = amount,

            };
            var serviceResponse = new ServiceResponse<TransactionDto>(transactionDto)
            {

                Value = transactionDto,
                IsSuccess = true
            };
            _mockCurrencyExchangeService.Setup(x => x.ExecuteCurrencyExchangeAsync(baseCurrency, targetCurrency, amount, userId))
                .ReturnsAsync(serviceResponse);

            // Act
            var result = await _currencyExchangeController.ExecuteCurrencyExchange(baseCurrency, targetCurrency, amount, userId);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.Equal(serviceResponse, okResult.Value);
        }

        [Fact]
        public async Task GetAllCurrencies_ShouldReturnOkObjectResult_WhenServiceReturnsSuccessfulResponse()
        {
            // Arrange
            var currencyDtoList = new List<CurrencyDto>
            {
                new CurrencyDto {Code = "EUR", Name = "Euro"},
                new CurrencyDto {Code = "USD", Name = "US Dollar"}
            };
            var serviceResponse = new ServiceResponse<List<CurrencyDto>>(currencyDtoList)
            {
                Value = currencyDtoList,
                IsSuccess = true
            };
            _mockCurrencyExchangeService.Setup(x => x.GetCurrencyList())
                .ReturnsAsync(serviceResponse);

            // Act
            var result = await _currencyExchangeController.GetAllCurrencies();

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.Equal(serviceResponse, okResult.Value);
        }

        [Fact]
        public async Task GetTransactionHistoryByUserId_ShouldReturnOkObjectResult_WhenServiceReturnsSuccessfulResponse()
        {
            // Arrange
            var userId = 1;
            var transactionList = new List<Transaction>
            {
                new Transaction { Id = 1, UserId = 1, BaseCurrencyId = 1, TargetCurrencyId = 2, Amount = 100 },
                new Transaction { Id = 2, UserId = 1, BaseCurrencyId = 3, TargetCurrencyId = 2, Amount = 50 }
            };
            var serviceResponse = new ServiceResponse<List<Transaction>>(transactionList) { Value = transactionList, IsSuccess = true };
            _mockCurrencyExchangeService.Setup(x => x.GetTransactionHistoryByUserId(userId)).ReturnsAsync(serviceResponse);

            // Act
            var result = await _currencyExchangeController.GetTransactionHistoryByUserId(userId);

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ServiceResponse<List<Transaction>>>(okObjectResult.Value);
            Assert.Equal(serviceResponse.Value.Count, response.Value.Count);
            Assert.True(response.IsSuccess);
        }
    }
}