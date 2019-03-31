using System;
using System.Collections.Generic;
using System.Text;

namespace ATBase.MQProvider.RocketMQ
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class ProducerConfig : AliyunONSConfig
    {
        private readonly String _producerId;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="producerId"></param>
        public ProducerConfig(String producerId)
        {
            if (String.IsNullOrWhiteSpace(producerId))
            {
                throw new ArgumentNullException("producerId");
            }

            _producerId = producerId;
        }

        /// <summary>
        /// 
        /// </summary>
        public override String MQRoleId
        {
            get
            {
                return _producerId;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override String AccessKeyId
        {
            get
            {
                return Configuration["ONSPub:AccessKeyId"];
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override String AccessKeySecret
        {
            get
            {
                return Configuration["ONSPub:AccessKeySecret"];
            }
        }
    }
}
