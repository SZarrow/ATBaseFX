using System;
using System.Collections.Generic;
using ATBase.Core;

namespace ATBase.Logging
{
    /// <summary>
    /// 同步Appender接口
    /// </summary>
    public interface IAppender
    {
        /// <summary>
        /// 获取当前Appender的日志级别
        /// </summary>
        LogLevel LogLevel { get; }
        /// <summary>
        /// 判断是否允许记录指定类型的日志
        /// </summary>
        /// <param name="logType">日志类型</param>
        Boolean Allow(LogType logType);
        /// <summary>
        /// 以同步的方式写入指定类型的日志
        /// </summary>
        /// <param name="contents">写入的日志的内容</param>
        XResult<IEnumerable<LogEntity>> Write(IEnumerable<LogEntity> contents);
    }
}
