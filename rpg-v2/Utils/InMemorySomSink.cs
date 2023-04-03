using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Serilog.Core;
using Serilog.Events;

namespace rpg_v2.Utils
{
    public class InMemorySomSink : ILogEventSink, IDisposable
    {
        private static readonly AsyncLocal<InMemorySomSink> LocalInstance = new AsyncLocal<InMemorySomSink>();
        private List<LogEvent> _logEvents;
        private const int MaxEventsCount = 32;

        private InMemorySomSink()
        {
            _logEvents = new List<LogEvent>();
        }
        
        public static InMemorySomSink Instance => LocalInstance.Value ?? (LocalInstance.Value = new InMemorySomSink());

        public IEnumerable<LogEvent> Events => _logEvents;

        public void Emit(LogEvent logEvent)
        {
            if (_logEvents.Count >= MaxEventsCount) 
                _logEvents.Remove(_logEvents[^1]);
            
            _logEvents = new List<LogEvent>(_logEvents.Prepend(logEvent));
        }

        public void Dispose()
        {
            _logEvents.Clear();
        }
    }
}