using System;
using System.Collections.Generic;
using System.Text;

namespace Concos.Logging.Extension.FileLogger
{
    public class FileLoggerOptions
    {
        private string _logFileExtension;
        private string _logfilePath;

        public string LogFileExtension { get => _logFileExtension; set { if (!value.StartsWith(".")) { value += "."; } _logFileExtension = value; } }
        public string LogfilePath { get => _logfilePath; set => _logfilePath = value; }
        public FileLoggerOptions()
        {
            _logFileExtension = ".txt";
            _logfilePath = "Log";
        }

    }
}
