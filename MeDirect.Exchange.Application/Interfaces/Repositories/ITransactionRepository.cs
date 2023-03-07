using MeDirect.Exchange.Domain.Entities;

namespace MeDirect.Exchange.Application.Interfaces.Repositories
{
    public interface ITransactionRepository : IRepository<Transaction>
    {
        Task<List<Transaction>> GetAllTransactionHistoryByUserId(int userId);
    }
}
