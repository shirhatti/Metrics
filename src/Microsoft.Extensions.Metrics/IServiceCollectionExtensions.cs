using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Metrics;
using Microsoft.Extensions.Options;
using System;

namespace Microsoft.AspNetCore.Builder
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddMetrics(this IServiceCollection services)
        {
            services.AddRouting();
            services.AddOptions();
            services.AddHostedService<MetricsService>();
            services.AddSingleton<IMetricsData, MetricsData>();
            return services;
        }

        public static IServiceCollection AddMetrics(this IServiceCollection services, Action<MetricsServiceOptions> configureOptions)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (configureOptions == null)
            {
                throw new ArgumentNullException(nameof(configureOptions));
            }

            return services.Configure(configureOptions).AddMetrics();
        }
    }
}
