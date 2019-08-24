using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Threading;
using ATBase.Core;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace ATBase.Logging
{
    /// <summary>
    /// 
    /// </summary>
    public static class LogManager
    {
        private static readonly ReaderWriterLockSlim s_lock;
        private static ILogger s_logger;

        static LogManager()
        {
            s_lock = new ReaderWriterLockSlim();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configAction"></param>
        /// <returns></returns>
        public static ILogger GetLogger(Action<LoggerBuilder> configAction)
        {
            String appName = "ATBase.Logging";

            var cachedLogger = AppDomain.CurrentDomain.GetData(appName) as ILogger;
            if (cachedLogger != null) { return cachedLogger; }

            try
            {
                s_lock.EnterWriteLock();

                cachedLogger = AppDomain.CurrentDomain.GetData(appName) as ILogger;
                if (cachedLogger != null) { return cachedLogger; }

                var builder = new LoggerBuilder();
                if (configAction != null)
                {
                    configAction(builder);
                }

                var logger = new TimerLogger(builder.Build());
                s_logger = logger;

                AppDomain.CurrentDomain.SetData(appName, s_logger);

                return s_logger;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (s_lock.IsWriteLockHeld)
                {
                    s_lock.ExitWriteLock();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static ILogger GetLogger()
        {
            return GetLogger(builder =>
           {
               String configFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Configs", "ATBase.Logging.Config.json");
               if (!File.Exists(configFilePath))
               {
                   throw new FileNotFoundException(configFilePath);
               }

               IConfigurationRoot cfgRoot = null;
               try
               {
                   var cb = new ConfigurationBuilder();
                   cb.AddJsonFile(configFilePath);
                   cfgRoot = cb.Build();
               }
               catch (Exception ex)
               {
                   throw new FileLoadException($"解析日志配置文件{configFilePath}出现异常", ex);
               }

               builder.UseFileLogger(config =>
               {
                   config.LogLevel = ParseLogLevel(cfgRoot["Appenders:FileAppender:LogLevel"]);
               });

               builder.UseEmailLogger(config =>
               {
                   config.LogLevel = ParseLogLevel(cfgRoot["Appenders:EmailAppender:LogLevel"]);
                   config.SmtpServerAddress = cfgRoot["Appenders:EmailAppender:SmtpServerAddress"];
                   config.SmtpServerPort = cfgRoot["Appenders:EmailAppender:SmtpServerPort"].ToInt32();
                   config.UseSSL = String.Compare(cfgRoot["Appenders:EmailAppender:UseSSL"], "true", true) == 0;
                   config.SendAccount = new MailboxAddress(cfgRoot["Appenders:EmailAppender:SendAccount"]);
                   config.Credential = new System.Net.NetworkCredential(config.SendAccount.Address, cfgRoot["Appenders:EmailAppender:Credential"]);
                   var reveiveAccounts = cfgRoot["Appenders:EmailAppender:ReceiveAccounts"].Split(new Char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                   if (reveiveAccounts != null && reveiveAccounts.Length > 0)
                   {
                       foreach (var acct in reveiveAccounts)
                       {
                           config.ReceiveAccounts.Add(new MailboxAddress(acct));
                       }
                   }
               });

               builder.UseConsoleLogger(config =>
               {
                   config.LogLevel = ParseLogLevel(cfgRoot["Appenders:ConsoleAppender:LogLevel"]);
               });
           });
        }

        private static LogLevel ParseLogLevel(String logLevelValues)
        {
            if (logLevelValues.HasValue())
            {
                LogLevel? result = null;
                String[] logLevels = logLevelValues.Split(new Char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var logLevel in logLevels)
                {
                    if (Enum.TryParse(logLevel, out LogLevel lv))
                    {
                        if (result == null) { result = lv; }
                        else { result |= lv; }
                    }
                }

                if (result != null) { return result.Value; }
            }

            return LogLevel.Error;
        }
    }
}
