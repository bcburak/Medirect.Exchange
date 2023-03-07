using MeDirect.Exchange.Application.Services.Implementation;
using MeDirect.Exchange.Application.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace MeDirect.Exchange.Application.Registration
{
    public static class ServiceRegistration
    {

        public static void AddApplicationRegistration(this IServiceCollection services)
        {
            //services.AddHostedService<_CacheUpdaterService>();

            services.AddTransient<ICacheService, CacheService>();
            services.AddScoped<ICurrencyExchangeService, CurrencyExchangeService>();
            services.AddTransient<IExchangeRateApiService, ExchangeRateApiService>();

        }
    }
}
