using System;
using System.Collections.Generic;
using System.Text;

namespace ATBase.MQCore.Common
{
    /// <summary>
    ///  订阅方式
    /// </summary>
    public enum SubscribeType
    {
        /// <summary>
        /// 集群订阅，默认方式
        /// </summary>
        CLUSTERING = 0,
        /// <summary>
        /// 广播订阅
        /// </summary>
        BROADCASTING = 1
    }
}
