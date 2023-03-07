using MeDirect.Exchange.Application.Constants;
using MeDirect.Exchange.Application.Services.Interfaces;

namespace MeDirect.Exchange.API.Middlewares
{
    public class RateLimiterMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ICacheService _cacheService;

        public RateLimiterMiddleware(RequestDelegate next, ICacheService cacheService)
        {
            _next = next;
            _cacheService = cacheService;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var requestedEndpoint = context.Request.Path.Value;
            var route = context.GetRouteData();
            var userId = route.Values["userId"];
            // Uncomment depends if needed ip with userId
            //var ipAddress = context.Connection.RemoteIpAddress.ToString();


            if (requestedEndpoint.Contains("executeCurrencyExchange"))
            {

                // Set the key for the client's currency exchange count
                var key = $"{userId}-{CacheKeys.RateLimit}";

                // Get the client's currency exchange count from the cache
                var count = _cacheService.Get<int>(key);

                // Increment the count and set the cache with a 1-hour expiration
                count++;
                _cacheService.Add(key, count, TimeSpan.FromHours(1));

                // If the client has exceeded the limit, return a 429 Too Many Requests response
                if (count > 10)
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
