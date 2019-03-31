using System;
using System.Collections.Generic;
using System.Text;

namespace ATBase.MQCore.Common
{
    /// <summary>
    /// 消息类型
    /// </summary>
    public enum MQMessageType
    {
        /// <summary>
        /// 普通消息
        /// </summary>
        GeneralMessage = 0,
        /// <summary>
        /// 顺序消息
        /// </summary>
        OrderMessage = 1,
        /// <summary>
        /// 事务消息
        /// </summary>
        TransactionMessage = 2
    }
}
