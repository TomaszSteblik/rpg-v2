using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Serilog;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Formatting.Display;

namespace rpg_v2.Utils
{
    public class InMemorySomSink : ILogEventSink, IDisposable
    {
        private static readonly AsyncLocal<InMemorySomSink> LocalInstance = new AsyncLocal<InMemorySomSink>();
        private List<string> _logEvents;
        private const int MaxEventsCount = 32;
        private ITextFormatter _textFormatter;

        private InMemorySomSink()
        {
            _logEvents = new List<string>();
        }

        public void SetFormatter(ITextFormatter textFormatter)
        {
            _textFormatter = textFormatter;
        }
        
        public static InMemorySomSink Instance => LocalInstance.Value ?? (LocalInstance.Value = new InMemorySomSink());

        public IEnumerable<string> Events => _logEvents;

        public void Emit(LogEvent logEvent)
        {
            if (_logEvents.Count >= MaxEventsCount) 
                _logEvents.Remove(_logEvents[^1]);

            using var buffer = new StringWriter();
            _textFormatter.Format(logEvent, buffer);
            _logEvents = new List<string>(_logEvents.Prepend(buffer.ToString()));
        }

        public void Dispose()
        {
            _logEvents.Clear();
        }
    }

    public static class InMemorySomSinkExtensions
    {
        const string DefaultDebugOutputTemplate = "[{Timestamp:HH:mm:ss}] {Message:lj}";
        public static LoggerConfiguration InMemorySom(
            this LoggerSinkConfiguration sinkConfiguration,
            LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum,
            string outputTemplate = DefaultDebugOutputTemplate,
            IFormatProvider formatProvider = null,
            LoggingLevelSwitch levelSwitch = null)
        {
            if (sinkConfiguration == null) throw new ArgumentNullException(nameof(sinkConfiguration));
            if (outputTemplate == null) throw new ArgumentNullException(nameof(outputTemplate));

            var formatter = new MessageTemplateTextFormatter(outputTemplate, formatProvider);
            return sinkConfiguration.InMemorySom(formatter, restrictedToMinimumLevel, levelSwitch);
        }

        private static LoggerConfiguration InMemorySom(
            this LoggerSinkConfiguration sinkConfiguration,
            ITextFormatter formatter,
            LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum,
            LoggingLevelSwitch levelSwitch = null)
        {
            if (sinkConfiguration == null) throw new ArgumentNullException(nameof(sinkConfiguration));
            if (formatter == null) throw new ArgumentNullException(nameof(formatter));
            
            InMemorySomSink.Instance.SetFormatter(formatter);

            return sinkConfiguration.Sink(InMemorySomSink.Instance, restrictedToMinimumLevel, levelSwitch);
        }
    }
}