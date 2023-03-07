
using MeDirect.Exchange.Application.Responses;
using MeDirect.Exchange.Domain.Contracts;
using MeDirect.Exchange.Domain.Entities;

namespace MeDirect.Exchange.Application.Services.Interfaces
{
    public interface ICurrencyExchangeService
    {
        Task<ServiceResponse<Transaction>> ExecuteCurrencyExchangeAsync(string baseCurrency, string targetCurrency, decimal amount, int userId);
        Task<List<CurrencyDto>> GetCurrencyList();
    }
}
