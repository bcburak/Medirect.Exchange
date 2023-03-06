using MeDirect.Exchange.Application.Interfaces.Repositories;
using MeDirect.Exchange.Domain.Entities;
using MeDirect.Exchange.Persistence.Context;
namespace Medirect.Exchange.Persistence.Repositories
{
    public class ExchangeRateRepository : Repository<ExchangeRate>, IExchangeRateRepository
    {
        public ExchangeRateRepository(CurrencyExchangeDbContext dbContext) : base(dbContext)
        {
        }
        //public async Task<List<ExchangeRate>> GetExchangeRateHistoryAsync(string baseCurrency, string targetCurrency, DateTime startDate, DateTime endDate)
        //{
        //    return await dbContext.ExchangeRates
        //        .Where(x => x.BaseCurrency == baseCurrency && x.TargetCurrency == targetCurrency && x.Date >= startDate && x.Date <= endDate)
        //        .ToListAsync();
        //}

    }
}
