using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Microsoft.Extensions.Metrics
{
    public class MetricsData : IMetricsData
    {
        public IDictionary<string, string> Metrics { get; } = new ConcurrentDictionary<string, string>();
    }
}