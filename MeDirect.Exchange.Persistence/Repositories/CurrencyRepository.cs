using Medirect.Exchange.Persistence.Repositories;
using MeDirect.Exchange.Application.Interfaces.Repositories;
using MeDirect.Exchange.Domain.Entities;
using MeDirect.Exchange.Persistence.Context;

namespace MeDirect.Exchange.Persistence.Repositories
{
    public class CurrencyRepository : Repository<Currency>, ICurrencyRepository
    {
        public CurrencyRepository(CurrencyExchangeDbContext dbContext) : base(dbContext)
        {
        }

    }
}
