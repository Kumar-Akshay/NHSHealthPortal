using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace NHSHealthPortal.API.Helper;

/*
 Why we use the rate limiting
    - Preventing the Abuse, DDOS Attack, and Protecting Resource and Enhance Security
    - Ensure the fair use of API
*/
public static class RateLimiterExtension
{
    /*
     * There are two ways to implement like global rate limiting and endpoint or page wise rate limiting
     * Global set to all endpoint and Named set to specific controller using  [EnableRateLimiting("")] attribute or disable [DisableRateLimiting]
     * We can apply rate limit on HttpContext values like Host, Path, API Key, Token, UserId, etc (Partition RateLimiter)
     * We can apply any rate limit algorithm with it is pros and cons
     */
    public static IServiceCollection UseEndpointFixedRateLimiter(this IServiceCollection services, IConfiguration configuration)
    {
        // Define the default limit
        var permitRequestCount = 100;
        var windowSizeInSec = 1;
        var retryAfterSec = 10;
        var permitRequestValue = configuration["RateLimit:PermitRequestCount"];
        var windowSizeValue = configuration["RateLimit:PermitRequestCount"];
        var retryAfterValue = configuration["RateLimit:RetryAfter"];
        
        // Fail to load config values it load the default limit
        if (!string.IsNullOrEmpty(permitRequestValue) && !string.IsNullOrEmpty(windowSizeValue))
        {
            int.TryParse(permitRequestValue, out permitRequestCount);
            int.TryParse(windowSizeValue, out windowSizeInSec);
        }
        
        
        services.AddRateLimiter(options => options
                .AddFixedWindowLimiter("fixed", fixOptions =>
                {
                    fixOptions.PermitLimit = permitRequestCount;
                    fixOptions.QueueLimit = 0;
                    fixOptions.Window = TimeSpan.FromSeconds(windowSizeInSec);
                    fixOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                })
                .OnRejected = async (context, cancellationToken) =>
            {
                context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                context.HttpContext.Response.Headers.Append("Retry-After", TimeSpan.FromSeconds(retryAfterSec).ToString());

                //Update the response to return the problem details of 429
                var problemDetails = new ProblemDetails
                {
                    Type = "https://httpstatuses.com/429",
                    Title = "Too Many Requests",
                    Detail = "Rate limit exceeded. Please try again later.",
                    Status = StatusCodes.Status429TooManyRequests,
                    Instance = context.HttpContext.Request.Path,
                    Extensions =
                    {
                        ["retryAfterSeconds"] =
                            context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfterMeta)
                                ? (int)retryAfterMeta.TotalSeconds
                                : 30
                    }
                };

                await context.HttpContext.Response.WriteAsJsonAsync(problemDetails,
                    cancellationToken);

            }
        );
        return services;
    }
}