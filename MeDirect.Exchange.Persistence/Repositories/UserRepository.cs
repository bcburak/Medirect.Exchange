using MeDirect.Exchange.Application.Interfaces.Repositories;
using MeDirect.Exchange.Domain.Entities;
using MeDirect.Exchange.Persistence.Context;
namespace Medirect.Exchange.Persistence.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(CurrencyExchangeDbContext dbContext) : base(dbContext)
        {
        }

    }
}
