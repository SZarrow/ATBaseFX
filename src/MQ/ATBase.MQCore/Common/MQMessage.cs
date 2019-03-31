using System;
using System.Collections.Generic;
using System.Text;
using ATBase.Core;

namespace ATBase.MQCore.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class MQMessage : IValidator
    {
        /// <summary>
        /// 
        /// </summary>
        public String PublishTopics { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public String Tags { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public String MessageContent { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public XResult<Boolean> Validate()
        {
            if (String.IsNullOrWhiteSpace(this.PublishTopics))
            {
                return new XResult<Boolean>(false, new ArgumentNullException("MQMessage.PublishTopics"));
            }

            if (String.IsNullOrWhiteSpace(this.Tags))
            {
                return new XResult<Boolean>(false, new ArgumentNullException("MQMessage.Tags"));
            }

            if (String.IsNullOrWhiteSpace(this.MessageContent))
            {
                return new XResult<Boolean>(false, new ArgumentNullException("MQMessage.MessageContent"));
            }

            return new XResult<Boolean>(true);
        }
    }
}
