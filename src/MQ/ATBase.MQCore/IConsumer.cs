using System;
using System.Collections.Generic;
using System.Text;
using ATBase.Core;

namespace ATBase.MQCore
{
    /// <summary>
    /// 消费者接口类
    /// </summary>
    public interface IConsumer : IDisposable
    {
        /// <summary>
        /// 订阅消息
        /// </summary>
        /// <param name="publishedTopics">订阅的消息的主题</param>
        /// <param name="tags">订阅的消息的Tags。如果使用RocketMQ，则订阅主题下的所有Tag用 * 表示，订阅指定多个tag时，多个tag之间用||隔开，例如：TagA||TagB</param>
        /// <param name="parameters">订阅消息所需的其他参数集合</param>
        XResult<Boolean> Subscribe(String publishedTopics, String tags, params Object[] parameters);
    }
}
