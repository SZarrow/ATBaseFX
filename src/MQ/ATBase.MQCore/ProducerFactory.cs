using System;
using System.Collections.Generic;
using System.Text;
using ATBase.MQCore.Common;

namespace ATBase.MQCore
{
    /// <summary>
    /// 生产者工厂类
    /// </summary>
    public abstract class ProducerFactory
    {
        /// <summary>
        /// 创建一个生产者
        /// </summary>
        /// <param name="producerId"></param>
        /// <param name="messageType"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public abstract IProducer CreateProducer(String producerId, MQMessageType messageType, params Object[] parameters);
    }
}
