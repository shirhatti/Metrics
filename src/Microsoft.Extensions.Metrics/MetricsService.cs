using Microsoft.AspNetCore.Rewrite.Internal.IISUrlRewrite;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Extensions.Metrics
{

    public class MetricsService : IHostedService, IDisposable
    {
        private Listener _listener;
        private ILogger _logger;
        private static readonly Action<ILogger, string, string, string, Exception> _counterReceived =
            LoggerMessage.Define<string, string, string>(LogLevel.Information,
                                                         new EventId(1, "CounterCallbackReceived"),
                                                         "{eventSourceName}:{eventName}={Count}");

        public MetricsService(IMetricsData metrics, ILoggerFactory loggerFactory)
        {
            var map = metrics.Metrics;

            _logger = loggerFactory.CreateLogger<MetricsService>();
            _listener = new Listener((eventSourceName, eventPayload) =>
            {
                ICounterPayload payload;
                if (eventPayload.ContainsKey("CounterType"))
                {
                    payload = eventPayload["CounterType"].Equals("Sum") ? (ICounterPayload)new IncrementingCounterPayload(eventPayload) : (ICounterPayload)new CounterPayload(eventPayload);
                }
                else
                {
                    payload = eventPayload.Count == 6 ? (ICounterPayload)new IncrementingCounterPayload(eventPayload) : (ICounterPayload)new CounterPayload(eventPayload);
                }
                _counterReceived(_logger, eventSourceName, payload.GetName(), payload.GetValue(), null);
                map[payload.GetName()] = payload.GetValue();
            });
        }

        public void Dispose()
        {
            _listener.Dispose();
        }

        public Task StartAsync(CancellationToken cancellationToken) => Task.CompletedTask;

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}