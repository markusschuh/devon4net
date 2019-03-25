namespace Devon4Net.Infrastructure.CircuitBreaker
{
    using Infrastructure.CircuitBreaker.Common.Entities;
    using Infrastructure.CircuitBreaker.Handler;
    using Infrastructure.CircuitBreaker.Options;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;
    using Polly;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;

    public static class CircuitBreakerConfiguration
    {
        public static void ConfigureCircuitBreakerService(this IServiceCollection services, IConfiguration configuration)
        {
            var sp = services.BuildServiceProvider();
            var endPointPollyOptions = sp.GetService<IOptions<EndPointPollyOptions>>();
            services.AddHttpClient(endPointPollyOptions.Value.CircuitBreaker.ToList());
            services.AddTransient<ICircuitBreakerHttpClient, CircuitBreakerHttpClient>();
        }

        private static void AddHttpClient(this IServiceCollection services, EndPointEntity endPointEntity)
        {
            if (endPointEntity == null) throw new ArgumentNullException("endPointEntity", "The end point provided is null");

            var waitAndSync = endPointEntity.GetWaitAndRetry();
            var waitAndSyncList = waitAndSync.Select(w => new TimeSpan(Convert.ToInt32(w)));

            services.AddHttpClient(endPointEntity.Name, client =>
            {
                client.BaseAddress = new Uri(endPointEntity.BaseAddress);

                foreach (var header in endPointEntity.GetHeaders())
                {
                    client.DefaultRequestHeaders.Add(header.Key, header.Value);
                }
            })
            .AddTransientHttpErrorPolicy(builder => builder.WaitAndRetryAsync(waitAndSyncList, (result, timeSpan, retryCount, context) =>
            {
                Console.WriteLine($"Request failed to {endPointEntity.Name} ({endPointEntity.BaseAddress}). Waiting {timeSpan} before next retry. Retry attempt {retryCount}");
                if (waitAndSyncList.Count() != retryCount) return;
                Console.WriteLine($"Error getting {endPointEntity.Name} ({endPointEntity.BaseAddress}) : {result.Exception.Message} *** {result.Exception.InnerException} *** {result.Exception.StackTrace}");
                throw new HttpRequestException($"Error getting {endPointEntity.Name} ({endPointEntity.BaseAddress})", result.Exception);
            }))
            .AddTransientHttpErrorPolicy(builder => builder.CircuitBreakerAsync(
                handledEventsAllowedBeforeBreaking: waitAndSync.Count,
                durationOfBreak: TimeSpan.FromSeconds(endPointEntity.DurationOfBreak))
            );
        }

        private static void AddHttpClient(this IServiceCollection services, List<EndPointEntity> endPointEntityList)
        {
            if (endPointEntityList == null || !endPointEntityList.Any()) throw new ArgumentNullException("endPointEntityList", "The end point List provided does not have endpoints");

            foreach (var endPointEntity in endPointEntityList)
            {
                services.AddHttpClient(endPointEntity);
            }
        }
    }
}

