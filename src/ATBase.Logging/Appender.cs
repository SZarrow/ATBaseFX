using System;
using System.Collections.Generic;
using System.Text;
using ATBase.Core;

namespace ATBase.Logging
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class Appender : IAppender
    {
        /// <summary>
        /// 
        /// </summary>
        protected Appender() { }
        /// <summary>
        /// 
        /// </summary>
        public abstract LogLevel LogLevel { get; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="contents"></param>
        public abstract XResult<IEnumerable<LogEntity>> Write(IEnumerable<LogEntity> contents);
        /// <summary>
        /// 根据日志级别判断是否允许记录指定类型的日志
        /// </summary>
        /// <param name="logType">日志类型</param>
        public Boolean Allow(LogType logType)
        {
            return ((Int32)this.LogLevel & (Int32)logType) == (Int32)logType;
        }
    }
}
