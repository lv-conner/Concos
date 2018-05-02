using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Concos.Logging.Extension.FileLogger
{
    public class FileLoggerProvider:ILoggerProvider
    {
        private static readonly Func<string, LogLevel, bool> _trueFilter = (s, l) => true;
        private readonly ConcurrentDictionary<string, FileLogger> _loggers = new ConcurrentDictionary<string, FileLogger>();
        private readonly Func<string, LogLevel, bool> _filter;
        private FileLoggerOptions _options;
        private IDisposable _reloadChangeToken;

        public FileLoggerProvider():this(null,_trueFilter)
        {

        }
        public FileLoggerProvider(Func<string, LogLevel, bool> filter) : this(null,filter)
        {

        }
        public FileLoggerProvider(IOptionsMonitor<FileLoggerOptions> options) : this(options,_trueFilter)
        {

        }
        public FileLoggerProvider(IOptionsMonitor<FileLoggerOptions> options, Func<string, LogLevel, bool> filter)
        {
            _options = options?.CurrentValue ?? new FileLoggerOptions();
            _filter = filter ?? _trueFilter;
            _reloadChangeToken = options.OnChange(Reload);
        }
        public ILogger CreateLogger(string categoryName)
        {
            return _loggers.GetOrAdd(categoryName, CreateLoggerImplement);
        }
        private void Reload(FileLoggerOptions options)
        {
            _options = options;
            _loggers.Clear();
        }
        private FileLogger CreateLoggerImplement(string name)
        {
            return new FileLogger(name, _filter, _options);
        }
        public void Dispose()
        {
            _reloadChangeToken.Dispose();
        }
    }
}
