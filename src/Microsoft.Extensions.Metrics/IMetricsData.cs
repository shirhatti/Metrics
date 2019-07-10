using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.Metrics
{
    public interface IMetricsData
    {
        public IDictionary<string, string> Metrics { get; }

        public string DumpToString()
        {
            var sb = new StringBuilder();
            foreach (var metric in Metrics)
            {
                sb.AppendLine($"{metric.Key} {metric.Value}");
            }
            return sb.ToString();
        }
    }
}