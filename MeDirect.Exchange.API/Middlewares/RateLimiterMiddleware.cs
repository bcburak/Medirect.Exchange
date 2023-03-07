using MeDirect.Exchange.Application.Constants;
using MeDirect.Exchange.Application.Interfaces.Repositories;
using MeDirect.Exchange.Application.Services.Interfaces;

namespace MeDirect.Exchange.API.Middlewares
{
    public class RateLimiterMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ICacheService _cacheService;
        private readonly IUserRepository _userRepository;

        public RateLimiterMiddleware(RequestDelegate next, ICacheService cacheService, IUserRepository userRepository)
        {
            _next = next;
            _cacheService = cacheService;
            _userRepository = userRepository;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var requestedEndpoint = context.Request.Path.Value;
            var route = context.GetRouteData();
            var userId = Convert.ToInt32(route.Values["userId"]);
            // Uncomment depends if needed ip with userId
            //var ipAddress = context.Connection.RemoteIpAddress.ToString();


            if (requestedEndpoint.Contains("executeCurrencyExchange"))
            {
                var userHourlyRateLimit = _userRepository.GetByIdAsync(userId).Result.TradesPerHourLimit;
                // Set the key for the client's currency exchange count
                var key = $"{userId}-{CacheKeys.RateLimit}";

                // Get the client's currency exchange count from the cache
                var count = _cacheService.Get<int>(key);

                // Increment the count and set the cache with a 1-hour expiration
                count++;
                _cacheService.Add(key, count, TimeSpan.FromHours(1));

                // If the client has exceeded the limit, return a 429 Too Many Requests response
                if (count > userHourlyRateLimit)
                {
                    context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                    await context.Response.WriteAsync("Too many currency exchange requests.");
                    return;
                }

                // If the client has not exceeded the limit, continue to the next middleware

            }
            await _next(context);


        }
    }
}
