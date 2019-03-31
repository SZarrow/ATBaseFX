using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using System.Net.Mail;
using MimeKit;

namespace ATBase.Logging
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class LogConfig
    {
        /// <summary>
        /// 
        /// </summary>
        protected LogConfig() { }
        /// <summary>
        /// 
        /// </summary>
        public LogLevel LogLevel { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class FileLogConfig : LogConfig, IEquatable<FileLogConfig>
    {
        /// <summary>
        /// 文件名格式，支持日期时间格式字符串
        /// </summary>
        public String FileNameFormat { get; set; }

        /// <summary>
        /// 获取文件路径
        /// </summary>
        public String FilePath
        {
            get
            {

                String fileName = null;

                if (String.IsNullOrWhiteSpace(this.FileNameFormat))
                {
                    fileName = DateTime.Now.ToString("yyyy-MM-dd");
                }
                else
                {
                    try
                    {
                        fileName = DateTime.Now.ToString(this.FileNameFormat);
                    }
                    catch
                    {
                        fileName = this.FileNameFormat;
                    }
                }

                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log", $"{fileName}.txt");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public Boolean Equals(FileLogConfig other)
        {
            if (other == null) { return false; }
            return this.GetHashCode() == other.GetHashCode();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override Int32 GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override String ToString()
        {
            return $"{FilePath}:{((Int32)this.LogLevel).ToString()}";
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class MongoDbLogConfig : LogConfig, IEquatable<MongoDbLogConfig>
    {
        /// <summary>
        /// 获取MongoDb连接字符串
        /// </summary>
        public String ConnectionString { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        public Boolean Equals(MongoDbLogConfig other)
        {
            if (other == null) { return false; }
            return this.GetHashCode() == other.GetHashCode();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class EmailLogConfig : LogConfig, IEquatable<EmailLogConfig>
    {
        /// <summary>
        /// 
        /// </summary>
        public EmailLogConfig()
        {
            this.ReceiveAccounts = new Collection<MailboxAddress>();
        }
        /// <summary>
        /// 邮件发送服务器的地址
        /// </summary>
        public String SmtpServerAddress { get; set; }
        /// <summary>
        /// 邮件发送服务器的端口
        /// </summary>
        public Int32 SmtpServerPort { get; set; }
        /// <summary>
        /// 使用SSL加密协议发送邮件
        /// </summary>
        public Boolean UseSSL { get; set; }
        /// <summary>
        /// 发件人账户
        /// </summary>
        public MailboxAddress SendAccount { get; set; }
        /// <summary>
        /// 发件人账户密码
        /// </summary>
        public NetworkCredential Credential { get; set; }
        /// <summary>
        /// 收件人账户
        /// </summary>
        public ICollection<MailboxAddress> ReceiveAccounts { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        public Boolean Equals(EmailLogConfig other)
        {
            return this.GetHashCode() == other.GetHashCode();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ConsoleLogConfig : LogConfig, IEquatable<ConsoleLogConfig>
    {
        public Boolean Equals(ConsoleLogConfig other)
        {
            return this.GetHashCode() == other.GetHashCode();
        }
    }
}
