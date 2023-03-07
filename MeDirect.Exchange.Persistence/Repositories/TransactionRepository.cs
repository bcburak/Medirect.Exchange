using MeDirect.Exchange.Application.Interfaces.Repositories;
using MeDirect.Exchange.Domain.Entities;
using MeDirect.Exchange.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Medirect.Exchange.Persistence.Repositories
{
    public class TransactionRepository : Repository<Transaction>, ITransactionRepository
    {
        private readonly CurrencyExchangeDbContext _dbContext;
        public TransactionRepository(CurrencyExchangeDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Transaction>> GetAllTransactionHistoryByUserId(int userId)
        {
            return await _dbContext.Transactions.Where(i => i.UserId.Equals(userId)).ToListAsync();
        }

    }
}
