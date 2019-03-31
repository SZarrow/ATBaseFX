using System;
using System.Collections.Generic;
using System.Text;
using ATBase.MQCore.Common;

namespace ATBase.MQCore
{
    /// <summary>
    /// 消费者工厂类，用来创建消费者
    /// </summary>
    public abstract class ConsumerFactory
    {
        /// <summary>
        /// 创建一个消费者
        /// </summary>
        /// <param name="consumerId"></param>
        /// <param name="subscribeType"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public abstract IConsumer CreateConsumer(String consumerId, SubscribeType subscribeType, params Object[] parameters);
    }
}
