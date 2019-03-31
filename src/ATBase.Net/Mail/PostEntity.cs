using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MailKit.Net.Smtp;

namespace ATBase.Net.Mail
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    internal class PostEntity : IEquatable<PostEntity>
    {
        private const String EmptyKey = "F3587402-B44E-46D8-9860-4ED08F64E5CA";

        public PostEntity(SmtpClient client, PostMessage message)
        {
            if (client == null)
            {
                throw new ArgumentNullException("client");
            }

            if (message == null)
            {
                throw new ArgumentNullException("message");
            }

            this.SmtpClient = client;
            this.Message = message;
            this.Retry = 3;
        }

        public String Key
        {
            get
            {
                if (this.Message != null)
                {
                    return this.Message.Key;
                }

                return EmptyKey;
            }
        }

        internal PostMessage Message { get; }

        public Int32 Retry { get; set; }

        public SmtpClient SmtpClient { get; }

        public Int32 GetHashCode(PostEntity entity)
        {
            return entity.Key.GetHashCode();
        }

        public Boolean Equals(PostEntity other)
        {
            return other != null && this.Key == other.Key;
        }
    }
}
