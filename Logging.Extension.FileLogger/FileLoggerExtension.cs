using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Concos.Logging.Extension.FileLogger
{
    public static class FileLoggerExtension
    {
        public static ILoggingBuilder AddFileLogger(this ILoggingBuilder builder)
        {
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, FileLoggerProvider>());
            return builder;
        }
        public static ILoggingBuilder AddFileLogger(this ILoggingBuilder builder, Func<string, LogLevel, bool> filter)
        {
            builder.AddProvider(new FileLoggerProvider(filter));
            return builder;
        }
        public static ILoggingBuilder AddFileLogger(this ILoggingBuilder builder,IConfiguration configuration)
        {
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, FileLoggerProvider>());
            builder.Services.Configure<FileLoggerOptions>(configuration);
            return builder;
        }
    }
}
