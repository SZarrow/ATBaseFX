using System;
using System.Collections.Generic;
using System.Text;
using ATBase.Core;
using ATBase.MQCore;
using ons;

namespace ATBase.MQProvider.RocketMQ
{
    /// <summary>
    /// 消费者默认实现类
    /// </summary>
    public class RocketMQConsumer : IConsumer
    {
        private PushConsumer _pushConsumer;
        private Boolean _isStarted = false;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="consumer"></param>
        public RocketMQConsumer(PushConsumer consumer)
        {
            if (consumer == null)
            {
                throw new ArgumentNullException(nameof(consumer));
            }

            _pushConsumer = consumer;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="publishedTopics"></param>
        /// <param name="tags"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public XResult<Boolean> Subscribe(String publishedTopics, String tags, params Object[] parameters)
        {
            if (String.IsNullOrWhiteSpace(publishedTopics))
            {
                return new XResult<Boolean>(false, new ArgumentNullException(nameof(publishedTopics)));
            }

            if (String.IsNullOrWhiteSpace(tags))
            {
                return new XResult<Boolean>(false, new ArgumentNullException(nameof(tags)));
            }

            if (_pushConsumer == null)
            {
                return new XResult<Boolean>(false, new ArgumentNullException(nameof(_pushConsumer)));
            }

            if (parameters == null || parameters.Length == 0)
            {
                return new XResult<Boolean>(false, new ArgumentNullException(nameof(parameters)));
            }

            var listener = parameters[0] as MessageListener;
            if (listener == null)
            {
                return new XResult<Boolean>(false, new FormatException("parameters[0] is not MessageListener"));
            }

            try
            {
                _pushConsumer.subscribe(publishedTopics, tags, listener);
                return Start();
            }
            catch (Exception ex)
            {
                return new XResult<Boolean>(false, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="consume"></param>
        /// <returns></returns>
        public static Object CreateSubscribeParameter(Func<Message, ConsumeContext, ons.Action> consume)
        {
            if (consume == null)
            {
                throw new ArgumentNullException(nameof(consume));
            }

            return new RocketMQMessageListener(consume);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            if (_pushConsumer != null)
            {
                try
                {
                    _pushConsumer.shutdown();
                    _pushConsumer.Dispose();
                    _isStarted = false;
                }
                catch { }
            }
        }

        private XResult<Boolean> Start()
        {
            if (!_isStarted)
            {
                try
                {
                    _pushConsumer.start();
                    _isStarted = true;
                }
                catch (Exception ex)
                {
                    return new XResult<Boolean>(false, ex);
                }
            }

            return new XResult<Boolean>(true);
        }
    }

    internal class RocketMQMessageListener : MessageListener
    {
        private Func<Message, ConsumeContext, ons.Action> _consume;

        public RocketMQMessageListener(Func<Message, ConsumeContext, ons.Action> consume)
        {
            if (consume == null)
            {
                throw new ArgumentNullException(nameof(consume));
            }

            _consume = consume;
        }

        public override ons.Action consume(Message message, ConsumeContext context)
        {
            return _consume(message, context);
        }
    }
}
