using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Globalization;
using System.Linq;

namespace Microsoft.Extensions.Metrics
{
    internal class Listener : EventListener
    {
        private readonly int _refreshInterval = TimeSpan.FromSeconds(1).Seconds;
        private bool _initialized = false;
        private Dictionary<string, string> _refreshIntervalDictionary;
        private Action<string, IDictionary<string, object>> _eventWritten;

        // Thread-safe variable to hold the list of all EventSourcesCreated.
        // This class may not be instantiated at the time of EventSource creation, so the list of EventSources should be stored to be enabled after initialization.
        private ConcurrentQueue<EventSource> _eventSources;

        public Listener(Action<string, IDictionary<string, object>> eventWritten)
        {
            _eventWritten = eventWritten;
            _refreshIntervalDictionary = new Dictionary<string, string>();
            _refreshIntervalDictionary.Add("EventCounterIntervalSec", _refreshInterval.ToString(CultureInfo.InvariantCulture));
            _initialized = true;
            foreach (var eventSource in _eventSources)
            {
                EnableEvents(eventSource, EventLevel.Critical, EventKeywords.All, _refreshIntervalDictionary);
            }
        }

        protected override void OnEventSourceCreated(EventSource eventSource)
        {
            // Keeping track of all EventSources here, as this call may happen before initialization.
            lock (this)
            {
                if (_eventSources == null)
                {
                    _eventSources = new ConcurrentQueue<EventSource>();
                }

                _eventSources.Enqueue(eventSource);
            }

            // If initialization is already done, we can enable EventSource right away.
            // This will take care of all EventSources created after initialization is done.
            if (_initialized)
            {
                EnableEvents(eventSource, EventLevel.Critical, EventKeywords.All, _refreshIntervalDictionary);
            }
        }

        protected override void OnEventWritten(EventWrittenEventArgs eventData)
        {
            if (_initialized)
            {
                var eventPayload = eventData.Payload[0] as IDictionary<string, object>;
                if (eventPayload != null)
                {
                    var eventSourceName = eventData.EventSource.Name;
                    _eventWritten(eventSourceName, eventPayload);
                }
            }
        }
    }
}
