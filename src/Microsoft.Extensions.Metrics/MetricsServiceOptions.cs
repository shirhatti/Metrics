using System.Collections.Generic;

namespace Microsoft.Extensions.Metrics
{
    public class MetricsServiceOptions
    {
        public ICollection<string> ProviderNames { get; set; }
    }
}