using MeDirect.Exchange.Application.Interfaces.Repositories;
using MeDirect.Exchange.Domain.Entities;
using MeDirect.Exchange.Persistence.Context;

namespace Medirect.Exchange.Persistence.Repositories
{
    public class TransactionRepository : Repository<Transaction>, ITransactionRepository
    {
        public TransactionRepository(CurrencyExchangeDbContext dbContext) : base(dbContext)
        {
        }

    }
}
