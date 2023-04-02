using System;
using System.Collections.Generic;
using System.Threading;
using Serilog.Core;
using Serilog.Events;

namespace rpg_v2.Utils
{
    public class InMemorySomSink : ILogEventSink, IDisposable
    {
        private static readonly AsyncLocal<InMemorySomSink> LocalInstance = new AsyncLocal<InMemorySomSink>();
        private readonly List<LogEvent> _logEvents;
        private const int MaxEventsCount = 32;

        private InMemorySomSink()
        {
            _logEvents = new List<LogEvent>();
        }
        
        public static InMemorySomSink Instance => LocalInstance.Value ?? (LocalInstance.Value = new InMemorySomSink());

        public void Emit(LogEvent logEvent)
        {
            if (_logEvents.Count >= MaxEventsCount) 
                _logEvents.RemoveAt(0);
            
            _logEvents.Add(logEvent);
        }

        public void Dispose()
        {
            _logEvents.Clear();
        }
    }
}