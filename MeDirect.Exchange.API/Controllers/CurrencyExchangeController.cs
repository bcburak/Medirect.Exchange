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
        /// <summary>
        /// Endpoint to get latest exchange values and execute transaction for related user
        /// </summary>
        /// <param name="baseCurrency"></param>
        /// <param name="targetCurrency"></param>
        /// <param name="amount"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Get All Currencies from db and filled the related table if anyone missed
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Endpoint to get the transaction history for the authenticated user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("history/{userId}")]
        public async Task<IActionResult> GetTransactionHistoryByUserId(int userId)
        {
            try
            {
                var result = await _currencyExchangeService.GetTransactionHistoryByUserId(userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



    }
}