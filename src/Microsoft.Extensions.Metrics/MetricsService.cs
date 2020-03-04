using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Extensions.Metrics
{

    public class MetricsService : IHostedService, IDisposable
    {
        private Listener _listener;
        private ILogger _logger;
        private MetricsServiceOptions _options;
        private readonly IHubContext<MetricsHub> _hubContext;
        private static readonly Action<ILogger, string, string, string, Exception> _counterReceived =
            LoggerMessage.Define<string, string, string>(LogLevel.Trace,
                                                         new EventId(1, "CounterCallbackReceived"),
                                                         "{eventSourceName}:{eventName}={Count}");

        public MetricsService(IMetricsData metrics, ILoggerFactory loggerFactory, IOptions<MetricsServiceOptions> options, IHubContext<MetricsHub> hubContext)
        {
            if (options.Value == null)
            {
                throw new ArgumentNullException(nameof(options.Value));
            }
            _options = options.Value;
            var map = metrics.Metrics;
            _logger = loggerFactory.CreateLogger<MetricsService>();
            _hubContext = hubContext;
            _listener = new Listener(_options.ProviderNames,
                                    async (eventSourceName, eventPayload) =>
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
                _counterReceived(_logger, eventSourceName, payload.Name, payload.Value, null);
                await _hubContext.Clients.All.SendAsync("Notify", $"{payload.DisplayName}\t{payload.Value}");
                map[payload.Name] = payload.Value;
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