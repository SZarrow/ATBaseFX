using System;
using System.Collections.Generic;
using System.Text;

namespace ATBase.MQProvider.RocketMQ
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class ConsumerConfig : AliyunONSConfig
    {
        private readonly String _consumerId;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="consumerId"></param>
        public ConsumerConfig(String consumerId)
        {
            if (String.IsNullOrWhiteSpace(consumerId))
            {
                throw new ArgumentNullException(nameof(consumerId));
            }

            _consumerId = consumerId;
        }

        /// <summary>
        /// 
        /// </summary>
        public override String MQRoleId
        {
            get
            {
                return _consumerId;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override String AccessKeyId
        {
            get
            {
                return Configuration["ONSSub:AccessKeyId"];
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override String AccessKeySecret
        {
            get
            {
                return Configuration["ONSSub:AccessKeySecret"];
            }
        }
    }
}
