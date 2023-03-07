using MeDirect.Exchange.Application.Constants;
using MeDirect.Exchange.Application.Interfaces.Repositories;
using MeDirect.Exchange.Application.Responses;
using MeDirect.Exchange.Application.Services.Interfaces;
using MeDirect.Exchange.Domain.Contracts;
using MeDirect.Exchange.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace MeDirect.Exchange.Application.Services.Implementation
{
    public class CurrencyExchangeService : ICurrencyExchangeService
    {
        private readonly ICurrencyRepository _currencyRepository;
        private readonly IExchangeRateRepository _exchangeRateRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IExchangeRateApiService _exchangeRateApiService;
        private readonly ICacheService _cacheManager;
        private readonly ILogger<CurrencyExchangeService> _logger;

        public CurrencyExchangeService(ICurrencyRepository currencyRepository,
                                       IExchangeRateRepository exchangeRateRepository,
                                       IUserRepository userRepository,
                                       ITransactionRepository transactionRepository,
                                       IExchangeRateApiService exchangeRateApiService,
                                       ICacheService cacheManager, ILogger<CurrencyExchangeService> logger)
        {
            _currencyRepository = currencyRepository;
            _exchangeRateRepository = exchangeRateRepository;
            _userRepository = userRepository;
            _exchangeRateApiService = exchangeRateApiService;
            _transactionRepository = transactionRepository;
            _cacheManager = cacheManager;
            _logger = logger;
        }

        public async Task<ServiceResponse<TransactionDto>> ExecuteCurrencyExchangeAsync(string baseCurrency, string targetCurrency, decimal amount, int userId)
        {

            User user = await _userRepository.GetByIdAsync(userId);

            string cacheKey = $"{CacheKeys.ExchangeRates}_{baseCurrency}_{targetCurrency}";
            var exchangeRate = _cacheManager.Get<ExchangeRate>(cacheKey);

            if (exchangeRate == null)
            {
                _logger.LogWarning("ExecuteCurrencyExchangeAsync, ExchangeRate cache value is null");

                var currencyList = _cacheManager.Get<List<Currency>>(CacheKeys.AllCurrencies);
                if (currencyList != null)
                {
                    var baseCurrencyId = currencyList.FirstOrDefault(i => i.Code.Equals(baseCurrency)).Id;
                    var targetCurrencyId = currencyList.FirstOrDefault(i => i.Code.Equals(targetCurrency)).Id;


                    var exchangeResponse = await _exchangeRateApiService.GetExchangeRateAsync(baseCurrency, targetCurrency, amount);
                    //exchangeResponseCalculatedAmount = exchangeResponse.Result;
                    exchangeRate = await _exchangeRateRepository.AddAsync(new ExchangeRate { TargetCurrencyId = targetCurrencyId, BaseCurrencyId = baseCurrencyId, Rate = exchangeResponse.Info.Rate });
                    if (exchangeRate != null)
                    {
                        // Add the exchange rate to the cache with a 30-minute expiry
                        _cacheManager.Add(cacheKey, exchangeRate, TimeSpan.FromMinutes(30));
                    }
                }
            }

            if (exchangeRate == null)
            {
                _logger.LogError($"ExecuteCurrencyExchangeAsync, Exchange rate not found for : {baseCurrency}/{targetCurrency}");
                throw new Exception($"Exchange rate not found for {baseCurrency}/{targetCurrency}.");
            }

            decimal convertedAmount = Math.Round(amount * exchangeRate.Rate, 2);

            var transaction = new Transaction
            {
                Amount = amount,
                ConvertedAmount = convertedAmount,
                BaseCurrencyId = exchangeRate.BaseCurrencyId,
                ExchangeRateId = exchangeRate.Id,
                TargetCurrencyId = exchangeRate.TargetCurrencyId,
                TransactionDate = DateTime.UtcNow,
                UserId = userId

            };

            await _transactionRepository.AddAsync(transaction);
            _logger.LogInformation("ExecuteCurrencyExchangeAsync", $"Success; Adding transaction completed.");

            var transactionDto = new TransactionDto
            {
                TransactionDate = transaction.TransactionDate,
                Amount = transaction.Amount,
                BaseCurrency = baseCurrency,
                TargetCurrency = targetCurrency,
                ConvertedAmount = transaction.ConvertedAmount,
                UserName = user.Name

            };

            return new ServiceResponse<TransactionDto>(transactionDto) { Id = new Guid(), IsSuccess = true, Message = "Transaction is created" };

        }

        public async Task<ServiceResponse<List<CurrencyDto>>> GetCurrencyList()
        {

            var currencyListFromApi = await _exchangeRateApiService.GetCurrencyListAsync();
            var currencyList = await _currencyRepository.GetAllAsync();

            if (!currencyList.Count().Equals(currencyListFromApi.Count))
            {
                foreach (var item in currencyListFromApi)
                {
                    await _currencyRepository.AddAsync(new Domain.Entities.Currency { Code = item.Code, Name = item.Name });
                }
            }
            await UpdateCurrencyCacheAsync(currencyList);

            _logger.LogInformation($"GetCurrencyList, UpdatedCurrencies; Adding transaction completed.");

            return new ServiceResponse<List<CurrencyDto>>(currencyListFromApi) { Id = new Guid(), IsSuccess = true, Message = "Currency list fetched from api and updated the related cache" };

        }


        public async Task<ServiceResponse<List<Transaction>>> GetTransactionHistoryByUserId(int userId)
        {

            var transactionHistory = await _transactionRepository.GetAllTransactionHistoryByUserId(userId);


            if (transactionHistory.Count == 0)
            {
                _logger.LogError($"GetTransactionHistoryByUserId, User Id is not valid for: {userId}");
                throw new Exception($"User Id is not valid for {userId}.");
            }

            return new ServiceResponse<List<Transaction>>(transactionHistory) { Id = new Guid(), IsSuccess = true, Message = "User Transaction list is fetched" };

            //return convertedAmount;
        }


        private async Task UpdateCurrencyCacheAsync(IEnumerable<Currency> currencyList)
        {

            try
            {
                var currencies = await _currencyRepository.GetAllAsync();

                var key = CacheKeys.AllCurrencies;
                _cacheManager.Add(key, currencies, TimeSpan.FromDays(1), false);
            }
            catch (Exception ex)
            {
                _logger.LogError($"UpdateCurrencyCacheAsync, There is an issue occured while updating the cache {ex.Message}");
                throw new Exception($"There is an issue occured while updating the cache {ex.Message}");
                throw;
            }


        }

    }
}
