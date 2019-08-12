using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ATBase.Core;

namespace ATBase.Logging.Appenders
{
    public class ConsoleAppender : Appender
    {
        private readonly ConsoleLogConfig _config;

        public ConsoleAppender(ConsoleLogConfig config)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            _config = config;
        }

        public override LogLevel LogLevel
        {
            get
            {
                return _config.LogLevel;
            }
        }

        public override XResult<IEnumerable<LogEntity>> Write(IEnumerable<LogEntity> contents)
        {
            if (contents == null || contents.Count() == 0)
            {
                return new XResult<IEnumerable<LogEntity>>(null);
            }

            var writtenLogs = new List<LogEntity>(contents.Count());
            try
            {
                Console.OutputEncoding = Encoding.UTF8;
                foreach (var content in contents)
                {
                    Console.WriteLine(content.ToString());
                    writtenLogs.Add(content);
                    Console.WriteLine();
                }
            }
            catch { }

            return new XResult<IEnumerable<LogEntity>>(writtenLogs);
        }
    }
}
