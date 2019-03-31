using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATBase.Security;
using MimeKit;

namespace ATBase.Net.Mail
{
    internal class PostMessage : MimeMessage
    {

        public String Key
        {
            get
            {
                return CryptoHelper.GetMD5(this.ToString()).Value;
            }
        }

        public override Boolean Equals(Object obj)
        {
            if (obj == null) { return false; }
            if (this.GetType() != obj.GetType()) { return false; }
            var oMsg = obj as PostMessage;
            if (Key != oMsg.Key) { return false; }
            return true;
        }

        public override Int32 GetHashCode()
        {
            return this.Key.GetHashCode();
        }

        public override String ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(this.From.FirstOrDefault());

            foreach (var to in this.To)
            {
                sb.Append(to);
            }

            foreach (var bcc in this.Bcc)
            {
                sb.Append(bcc);
            }

            foreach (var cc in this.Cc)
            {
                sb.Append(cc);
            }

            foreach (var attachment in this.Attachments)
            {
                sb.Append(attachment.ContentId);
            }

            sb.Append(this.Body);

            return sb.ToString();
        }
    }
}
