using Microsoft.AspNetCore.Mvc;

namespace MeDirect.Exchange.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CurrencyExchangeController : ControllerBase
    {

        private readonly ILogger<CurrencyExchangeController> _logger;

        public CurrencyExchangeController()
        {

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
        [HttpGet("executeCurrencyExchange/{baseCurrency}/{targetCurrency}/{amount}/{userId}")]
        public async Task<IActionResult> ExecuteCurrencyExchange(string baseCurrency, string targetCurrency, decimal amount, int userId)
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
                return Ok();
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