using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Metrics;
using Microsoft.Extensions.Options;
using System;

namespace Microsoft.AspNetCore.Builder
{
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// Adds Metrics services to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> for adding services.</param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        public static IServiceCollection AddMetrics(this IServiceCollection services)
        {
            services.AddRouting();
            services.AddOptions();
            services.AddHostedService<MetricsService>();
            services.AddSingleton<IMetricsData, MetricsData>();
            services.AddSingleton<IConfigureOptions<MetricsServiceOptions>, MetricsServiceOptionsSetup>();
            return services;
        }

        /// <summary>
        /// Adds Metrics services to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> for adding services.</param>
        /// <param name="configureOptions">An <see cref="Action{GrpcServiceOptions}"/> to configure the provided <see cref="GrpcServiceOptions"/>.</param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
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
