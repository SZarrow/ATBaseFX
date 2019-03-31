using System;
using System.Collections.Generic;
using System.Linq;
using ATBase.Core;
using ATBase.Net.Mail;

namespace ATBase.Logging.Appenders
{
    /// <summary>
    /// 
    /// </summary>
    public class EmailAppender : Appender
    {
        private readonly EmailLogConfig _config;
        private readonly PostClient _postClient;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        public EmailAppender(EmailLogConfig config)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            _config = config;

            if (_config.SmtpServerAddress.HasValue()
                && _config.SmtpServerPort > 0
                && _config.Credential != null)
            {
                _postClient = new PostClient(_config.SmtpServerAddress, _config.SmtpServerPort, _config.UseSSL);
                _postClient.BindMainAccount(_config.SendAccount)
                    .BindCredential(_config.Credential)
                    .BindReceiveAccounts(_config.ReceiveAccounts.ToArray());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override LogLevel LogLevel
        {
            get
            {
                return _config.LogLevel;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contents"></param>
        public override XResult<IEnumerable<LogEntity>> Write(IEnumerable<LogEntity> contents)
        {
            if (_postClient == null)
            {
                return new XResult<IEnumerable<LogEntity>>(null);
            }

            if (contents == null || contents.Count() == 0)
            {
                return new XResult<IEnumerable<LogEntity>>(null);
            }

            var writterLogs = new List<LogEntity>(contents.Count());

            foreach (var content in contents)
            {
                var title = content.LogContent.HasValue() ? content.LogContent : String.Empty;
                if (title.IsNullOrWhiteSpace())
                {
                    if (content.Exceptions != null && content.Exceptions.Length > 0)
                    {
                        title = content.Exceptions[0].Message;
                    }
                    else
                    {
                        title = content.LogType.ToString();
                    }
                }
                else
                {
                    title = content.LogContent;
                }

                var sendResult = _postClient.SendMail(FormatContent(content), title);
                if (sendResult.Success && sendResult.Value)
                {
                    writterLogs.Add(content);
                }
            }

            return new XResult<IEnumerable<LogEntity>>(writterLogs.Count > 0 ? writterLogs : null);
        }

        private String FormatContent(LogEntity content)
        {
            return content.ToString();
        }
    }
}
