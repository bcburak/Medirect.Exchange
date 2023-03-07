using MeDirect.Exchange.Application.Responses;
using MeDirect.Exchange.Domain.Contracts;

namespace MeDirect.Exchange.Application.Services.Interfaces
{
    public interface IExchangeRateApiService
    {
        Task<ExchangeRateResponse> GetExchangeRateAsync(string baseCurrency, string targetCurrency, decimal amount);
        Task<List<CurrencyDto>> GetCurrencyListAsync();
    }
}
