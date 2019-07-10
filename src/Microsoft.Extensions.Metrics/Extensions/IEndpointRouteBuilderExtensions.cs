using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Metrics;
using System;

namespace Microsoft.AspNetCore.Builder
{
    public static class IEndpointRouteBuilderExtensions
    {
        public static IEndpointRouteBuilder MapMetricsEndpoint(this IEndpointRouteBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.MapGet("/metrics", async context =>
            {
                var metricsService = context.RequestServices.GetRequiredService<IMetricsData>();
                await context.Response.WriteAsync(metricsService.DumpToString());
            });
            return builder;
        }
    }
}
