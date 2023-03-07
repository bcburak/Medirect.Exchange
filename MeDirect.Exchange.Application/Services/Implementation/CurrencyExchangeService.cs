using MeDirect.Exchange.Application.Constants;
using MeDirect.Exchange.Application.Interfaces.Repositories;
using MeDirect.Exchange.Application.Responses;
using MeDirect.Exchange.Application.Services.Interfaces;
using MeDirect.Exchange.Domain.Contracts;
using MeDirect.Exchange.Domain.Entities;

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


        public CurrencyExchangeService(ICurrencyRepository currencyRepository,
                                       IExchangeRateRepository exchangeRateRepository,
                                       IUserRepository userRepository,
                                       ITransactionRepository transactionRepository,
                                       IExchangeRateApiService exchangeRateApiService,
                                       ICacheService cacheManager)
        {
            _currencyRepository = currencyRepository;
            _exchangeRateRepository = exchangeRateRepository;
            _userRepository = userRepository;
            _exchangeRateApiService = exchangeRateApiService;
            _transactionRepository = transactionRepository;
            _cacheManager = cacheManager;
        }

        public async Task<ServiceResponse<Transaction>> ExecuteCurrencyExchangeAsync(string baseCurrency, string targetCurrency, decimal amount, int userId)
        {
            string cacheKey = $"{CacheKeys.ExchangeRates}_{baseCurrency}_{targetCurrency}";
            var exchangeRate = _cacheManager.Get<ExchangeRate>(cacheKey);

            if (exchangeRate == null)
            {
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
                throw new Exception($"Exchange rate not found for {baseCurrency}/{targetCurrency}.");
            }

            User user = await _userRepository.GetByIdAsync(userId);
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

            return new ServiceResponse<Transaction>(transaction) { Id = new Guid(), IsSuccess = true, Message = "Transaction is created" };

            //return convertedAmount;
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


            return new ServiceResponse<List<CurrencyDto>>(currencyListFromApi) { Id = new Guid(), IsSuccess = true, Message = "Currency list fetched from api and updated the related cache" };

        }


        public async Task<ServiceResponse<List<Transaction>>> GetTransactionHistoryByUserId(int userId)
        {

            var transactionHistory = await _transactionRepository.GetAllTransactionHistoryByUserId(userId);


            return new ServiceResponse<List<Transaction>>(transactionHistory) { Id = new Guid(), IsSuccess = true, Message = "User Transaction list is fetched" };

            //return convertedAmount;
        }


        private async Task UpdateCurrencyCacheAsync(IEnumerable<Currency> currencyList)
        {
            var currencies = await _currencyRepository.GetAllAsync();

            var key = CacheKeys.AllCurrencies;
            _cacheManager.Add(key, currencies, TimeSpan.FromDays(1), false);

        }

    }
}
