using System;
using System.Collections.Generic;
using System.IO;

namespace ATBase.Logging
{
    /// <summary>
    /// 
    /// </summary>
    public static class LoggerBuilderExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="configAction"></param>
        public static void UseFileLogger(this LoggerBuilder builder, Action<FileLogConfig> configAction = null)
        {
            FileLogConfig fileConfig = new FileLogConfig()
            {
                FileNameFormat = "yyyy-MM-dd",
                LogLevel = (LogLevel.Error | LogLevel.Warn | LogLevel.Info | LogLevel.Debug | LogLevel.Trace)
            };

            if (configAction != null)
            {
                configAction(fileConfig);
            }

            builder.AddLogConfig(fileConfig);

            String saveDir = Path.GetDirectoryName(fileConfig.FilePath);
            if (!Directory.Exists(saveDir))
            {
                try
                {
                    Directory.CreateDirectory(saveDir);
                }
                catch (Exception ex) { throw ex; }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="configAction"></param>
        public static void UseMongoDb(this LoggerBuilder builder, Action<MongoDbLogConfig> configAction = null)
        {
            MongoDbLogConfig config = new MongoDbLogConfig()
            {
                LogLevel = (LogLevel.Error | LogLevel.Warn | LogLevel.Info | LogLevel.Debug | LogLevel.Trace)
            };

            if (configAction != null)
            {
                configAction(config);
            }

            builder.AddLogConfig(config);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="configAction"></param>
        public static void UseEmailLogger(this LoggerBuilder builder, Action<EmailLogConfig> configAction = null)
        {
            EmailLogConfig config = new EmailLogConfig()
            {
                LogLevel = LogLevel.Error
            };

            if (configAction != null)
            {
                configAction(config);
            }

            builder.AddLogConfig(config);
        }

        public static void UseConsoleLogger(this LoggerBuilder builder, Action<ConsoleLogConfig> configAction = null)
        {
            ConsoleLogConfig config = new ConsoleLogConfig()
            {
                LogLevel = LogLevel.Trace
            };

            if (configAction != null)
            {
                configAction(config);
            }

            builder.AddLogConfig(config);
        }
    }
}
