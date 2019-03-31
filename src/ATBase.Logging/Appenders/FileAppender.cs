using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ATBase.Core;

namespace ATBase.Logging.Appenders
{
    internal class FileAppender : Appender
    {
        private FileLogConfig _config;

        public FileAppender(FileLogConfig config)
        {
            if (config == null)
            {
                throw new ArgumentNullException("config");
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

            FileStream fs = null;
            StreamWriter sw = null;
            List<LogEntity> writtenLogs = new List<LogEntity>(contents.Count());

            try
            {
                fs = new FileStream(_config.FilePath, FileMode.Append, FileAccess.Write, FileShare.Read);
                sw = new StreamWriter(fs, Encoding.UTF8);

                foreach (var content in contents)
                {
                    sw.WriteLine(FormatContent(content));
                    writtenLogs.Add(content);
                }

                return new XResult<IEnumerable<LogEntity>>(writtenLogs.Count > 0 ? writtenLogs : null);
            }
            catch (Exception ex)
            {
                try { }
                finally
                {
                    String dumpFilePath = Path.Combine(Path.GetDirectoryName(_config.FilePath), "LoggerDump.txt");
                    try
                    {
                        File.AppendAllText(dumpFilePath, ex.Message);
                        File.AppendAllText(dumpFilePath, ex.StackTrace);

                        if (ex.InnerException != null)
                        {
                            File.AppendAllText(dumpFilePath, ex.InnerException.Message);
                            File.AppendAllText(dumpFilePath, ex.InnerException.StackTrace);
                        }
                    }
                    catch { }
                }

                return new XResult<IEnumerable<LogEntity>>(writtenLogs.Count > 0 ? writtenLogs : null);
            }
            finally
            {
                if (sw != null) { sw.Dispose(); }
                if (fs != null) { fs.Dispose(); }
            }
        }

        private String FormatContent(LogEntity content)
        {
            return content.ToString();
        }
    }
}
