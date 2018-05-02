using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions.Internal;
using System.IO;

namespace Concos.Logging.Extension.FileLogger
{
    public class FileLogger : ILogger
    {
        private string _catalogName;
        private Func<string, LogLevel, bool> _filter;
        private readonly FileLoggerOptions _options;
        private readonly string _logFilePath;
        public FileLogger(string catalogName,Func<string,LogLevel,bool> filter,FileLoggerOptions options)
        {
            _catalogName = catalogName;
            _filter = filter;
            _options = options ?? new FileLoggerOptions();
            EnsurePath();
            _logFilePath = GenerateFilePath();
        }
        private void EnsurePath()
        {
            if(!Directory.Exists(_options.LogfilePath))
            {
                Directory.CreateDirectory(_options.LogfilePath);
            }
        }
        private string GenerateFilePath()
        {
            return Path.Combine(_options.LogfilePath,_catalogName +_options.LogFileExtension);
        }
        public IDisposable BeginScope<TState>(TState state)
        {
            return NullScope.Instance;
        }
        public bool IsEnabled(LogLevel logLevel)
        {
            if(logLevel == LogLevel.None)
            {
                return false;
            }
            else
            {
                return _filter(_catalogName, logLevel);
            }
        }
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if(!IsEnabled(logLevel))
            {
                return;
            }
            File.AppendAllText(_logFilePath, eventId.ToString());
            File.AppendAllText(_logFilePath, Environment.NewLine);
            File.AppendAllText(_logFilePath, formatter(state, exception));
            File.AppendAllText(_logFilePath, Environment.NewLine);
        }
    }
}
