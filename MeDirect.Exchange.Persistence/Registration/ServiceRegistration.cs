using Medirect.Exchange.Persistence.Repositories;
using MeDirect.Exchange.Application.Interfaces.Repositories;
using MeDirect.Exchange.Persistence.Context;
using MeDirect.Exchange.Persistence.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MeDirect.Exchange.Persistence.Registration
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceRegistration(this IServiceCollection services, IConfiguration? configuration = null)
        {
            services.AddTransient<CurrencyExchangeDbContext>();
            services.AddTransient<ICurrencyRepository, CurrencyRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IExchangeRateRepository, ExchangeRateRepository>();
            services.AddTransient<ITransactionRepository, TransactionRepository>();
        }
    }
}
