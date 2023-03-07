
using MeDirect.Exchange.Application.Responses;
using MeDirect.Exchange.Domain.Contracts;
using MeDirect.Exchange.Domain.Entities;

namespace MeDirect.Exchange.Application.Services.Interfaces
{
    public interface ICurrencyExchangeService
    {
        Task<ServiceResponse<TransactionDto>> ExecuteCurrencyExchangeAsync(string baseCurrency, string targetCurrency, decimal amount, int userId);

        Task<ServiceResponse<List<CurrencyDto>>> GetCurrencyList();

        Task<ServiceResponse<List<Transaction>>> GetTransactionHistoryByUserId(int userId);
    }
}
