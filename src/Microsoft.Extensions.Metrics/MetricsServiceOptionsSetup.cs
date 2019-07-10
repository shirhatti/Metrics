using Microsoft.Extensions.Metrics;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Microsoft.AspNetCore.Builder
{
    internal class MetricsServiceOptionsSetup : IConfigureOptions<MetricsServiceOptions>
    {
        private readonly static ICollection<string> DefaultProviderNames = new List<string> { "System.Runtime", "Microsoft.AspNetCore" };
        public void Configure(MetricsServiceOptions options)
        {
            if (options.ProviderNames == null)
            {
                options.ProviderNames = new List<string>(DefaultProviderNames);
            }
        }
    }
}