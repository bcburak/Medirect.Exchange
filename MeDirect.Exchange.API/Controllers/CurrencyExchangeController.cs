using MeDirect.Exchange.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MeDirect.Exchange.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CurrencyExchangeController : ControllerBase
    {

        private readonly ILogger<CurrencyExchangeController> _logger;

        private readonly ICurrencyExchangeService _currencyExchangeService;
        public CurrencyExchangeController(ILogger<CurrencyExchangeController> logger, ICurrencyExchangeService currencyExchangeService)
        {
            _logger = logger;
            _currencyExchangeService = currencyExchangeService;
        }

        [HttpGet("executeCurrencyExchange/{baseCurrency}/{targetCurrency}/{amount}/{userId}")]
        public async Task<IActionResult> ExecuteCurrencyExchange(string baseCurrency, string targetCurrency, decimal amount, int userId)
        {
            try
            {

                var result = await _currencyExchangeService.ExecuteCurrencyExchangeAsync(baseCurrency.ToUpperInvariant(), targetCurrency.ToUpperInvariant(), amount, userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("convertCurrency/{baseCurrency}/{targetCurrency}/{amount}")]
        public async Task<IActionResult> ConvertCurrency(string baseCurrency, string targetCurrency, decimal amount)
        {
            try
            {

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet]
        [Route("getAllCurrencies")]
        public async Task<IActionResult> GetAllCurrencies()
        {
            try
            {
                var result = await _currencyExchangeService.GetCurrencyList();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Endpoint to get the latest exchange rate between two currencies
        [HttpGet("rate/{sourceCurrency}/{targetCurrency}")]
        public async Task<IActionResult> GetExchangeRate(string sourceCurrency, string targetCurrency)
        {
            try
            {
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Endpoint to get the transaction history for the authenticated user

        [HttpGet("history/{userId}")]
        public async Task<IActionResult> GetTransactionHistory(int userId)
        {
            try
            {
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



    }
}