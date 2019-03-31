using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ATBase.MQCore;
using ATBase.MQCore.Common;
using ons;

namespace ATBase.MQProvider.RocketMQ
{
    /// <summary>
    /// 
    /// </summary>
    public class RocketMQConsumerFactory : ConsumerFactory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="consumerId"></param>
        /// <param name="subscribeType"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public override IConsumer CreateConsumer(String consumerId, SubscribeType subscribeType, params Object[] parameters)
        {
            if (String.IsNullOrWhiteSpace(consumerId))
            {
                throw new ArgumentNullException(nameof(consumerId));
            }

            var config = new ConsumerConfig(consumerId);
            ONSFactoryProperty onsconfig = new ONSFactoryProperty();
            onsconfig.setFactoryProperty(ONSFactoryProperty.ConsumerId, consumerId);
            onsconfig.setFactoryProperty(ONSFactoryProperty.AccessKey, config.AccessKeyId);
            onsconfig.setFactoryProperty(ONSFactoryProperty.SecretKey, config.AccessKeySecret);

            String logDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log");
            if (!Directory.Exists(logDir))
            {
                try
                {
                    Directory.CreateDirectory(logDir);
                    onsconfig.setFactoryProperty(ONSFactoryProperty.LogPath, logDir);
                }
                catch { }
            }

            switch (subscribeType)
            {
                case SubscribeType.CLUSTERING:
                    onsconfig.setFactoryProperty(ONSFactoryProperty.MessageModel, ONSFactoryProperty.CLUSTERING);
                    break;

                case SubscribeType.BROADCASTING:
                    onsconfig.setFactoryProperty(ONSFactoryProperty.MessageModel, ONSFactoryProperty.CLUSTERING);
                    break;
            }

            var pushConsumer = ONSFactory.getInstance().createPushConsumer(onsconfig);
            return new RocketMQConsumer(pushConsumer);
        }
    }
}
