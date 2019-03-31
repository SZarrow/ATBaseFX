using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ATBase.Core;
using MailKit.Net.Smtp;
using MimeKit;

namespace ATBase.Net.Mail
{
    /// <summary>
    /// 邮件客户端。
    /// </summary>
    public sealed class PostClient : IDisposable
    {
        private SmtpClient _client;
        private MailboxAddress _mainAccount;
        private MailboxAddress[] _receiveAccounts;
        private MailboxAddress[] _bccAccounts;
        private MailboxAddress[] _ccAccounts;

        private static ConcurrentQueue<PostEntity> _messages;
        private static ConcurrentQueue<Exception> _exceptions;

        static PostClient()
        {
            _messages = new ConcurrentQueue<PostEntity>();
            _exceptions = new ConcurrentQueue<Exception>();
        }

        /// <summary>
        /// 
        /// </summary>
        public Collection<Exception> Exceptions
        {
            get
            {
                var exceptions = _exceptions.ToList();
                return exceptions.Count > 0 ? new Collection<Exception>(exceptions) : new Collection<Exception>();
            }
        }

        /// <summary>
        /// 初始化邮寄客户端。
        /// </summary>
        /// <param name="host">SMTP服务器地址</param>
        /// <param name="port">SMTP服务器端口</param>
        /// <param name="useSSL">是否使用SSL连接</param>
        public PostClient(String host, Int32 port = 25, Boolean useSSL = false)
        {
            if (String.IsNullOrWhiteSpace(host))
            {
                throw new ArgumentNullException("host");
            }

            this.Host = host;
            this.Port = port;
            this.UseSSL = useSSL;

            _client = new SmtpClient();

            if (this.UseSSL)
            {
                _client.ServerCertificateValidationCallback = (a, b, c, d) => true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public String Host { get; }
        /// <summary>
        /// 
        /// </summary>
        public Int32 Port { get; }
        /// <summary>
        /// 
        /// </summary>
        public Boolean UseSSL { get; }
        /// <summary>
        /// 
        /// </summary>
        public ICredentials Credentials { get; private set; }

        /// <summary>
        /// 绑定主账号
        /// </summary>
        /// <param name="mainAccount">主账号邮件地址</param>
        public PostClient BindMainAccount(MailboxAddress mainAccount)
        {
            if (mainAccount == null)
            {
                throw new ArgumentNullException("account");
            }

            _mainAccount = mainAccount;

            return this;
        }

        /// <summary>
        /// 绑定主账号使用的凭证
        /// </summary>
        /// <param name="credential">网络凭证，通常由用户名和密码组成。</param>
        public PostClient BindCredential(NetworkCredential credential)
        {
            if (credential == null)
            {
                throw new ArgumentNullException("credential");
            }

            this.Credentials = credential;

            return this;
        }

        /// <summary>
        /// 绑定收件地址
        /// </summary>
        /// <param name="receiveAccount">收件地址</param>
        /// <param name="bccAccounts">设置抄送地址</param>
        /// <param name="ccAccounts">设置密送地址</param>
        public PostClient BindReceiveAccount(MailboxAddress receiveAccount, MailboxAddress[] bccAccounts = null, MailboxAddress[] ccAccounts = null)
        {
            return BindReceiveAccounts(new MailboxAddress[] { receiveAccount }, bccAccounts, ccAccounts);
        }

        /// <summary>
        /// 绑定收件地址集合
        /// </summary>
        /// <param name="receiveAccounts">收件地址集合</param>
        /// <param name="bccAccounts">设置抄送地址</param>
        /// <param name="ccAccounts">设置密送地址</param>
        public PostClient BindReceiveAccounts(MailboxAddress[] receiveAccounts, MailboxAddress[] bccAccounts = null, MailboxAddress[] ccAccounts = null)
        {
            if (receiveAccounts == null)
            {
                throw new ArgumentNullException("receiveAddresses");
            }

            _receiveAccounts = receiveAccounts;
            _bccAccounts = bccAccounts;
            _ccAccounts = ccAccounts;

            return this;
        }

        /// <summary>
        /// 同步发送邮件。
        /// </summary>
        /// <param name="mailBody">邮件正文</param>
        /// <param name="mainTitle">邮件标题</param>
        /// <param name="bodyIsHtml">设置正文是否为HTML格式内容</param>
        public XResult<Boolean> SendMail(String mailBody, String mainTitle, Boolean bodyIsHtml = false)
        {
            var entity = BuildPostEntity(mailBody, mainTitle, bodyIsHtml);
            return SendMail(entity);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            while (_messages.TryDequeue(out PostEntity entity))
            {
                SendMail(entity);
            }

            _client.Dispose();
        }

        private XResult<Boolean> SendMail(PostEntity entity)
        {
            if (entity == null)
            {
                return new XResult<Boolean>(false, new ArgumentNullException("entity"));
            }

            try
            {
                _client.Connect(this.Host, this.Port, this.UseSSL);
                _client.Authenticate(this.Credentials);
                _client.Send(entity.Message);
                _client.Disconnect(true);

                while (_messages.Count > 0)
                {
                    if (_messages.TryDequeue(out PostEntity retryEntity))
                    {
                        SendMail(retryEntity);
                    }
                }

                return new XResult<Boolean>(true);
            }
            catch (Exception ex)
            {
                if (entity.Retry > 0)
                {
                    entity.Retry--;
                    _messages.Enqueue(entity);
                }

                return new XResult<Boolean>(false, ex);
            }
        }

        private PostEntity BuildPostEntity(String mailBody, String mainTitle, Boolean bodyIsHtml = false)
        {
            PostMessage msg = new PostMessage()
            {
                Subject = mainTitle
            };

            if (bodyIsHtml)
            {
                msg.Body = new TextPart("html") { Text = mailBody };
            }
            else
            {
                msg.Body = new TextPart("plain") { Text = mailBody };
            }

            msg.From.Add(_mainAccount);

            #region #设置抄送

            if (this._receiveAccounts != null && this._receiveAccounts.Length > 0)
            {
                foreach (var ac in this._receiveAccounts)
                {
                    msg.To.Add(ac);
                }
            }

            if (this._bccAccounts != null && this._bccAccounts.Length > 0)
            {
                foreach (var bcc in this._bccAccounts)
                {
                    msg.Bcc.Add(bcc);
                }
            }

            if (this._ccAccounts != null && this._ccAccounts.Length > 0)
            {
                foreach (var cc in this._ccAccounts)
                {
                    msg.Cc.Add(cc);
                }
            }

            #endregion

            return new PostEntity(this._client, msg);
        }
    }
}
