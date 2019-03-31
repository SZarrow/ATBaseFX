using System;
using System.Collections.Generic;

namespace ATBase.Logging
{
    /// <summary>
    /// 
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// 
        /// </summary>
        String CurrentTraceId { get; }
        /// <summary>
        /// 
        /// </summary>
        String CurrentAppName { get; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="appName"></param>
        /// <param name="traceId"></param>
        /// <param name="ip"></param>
        void StartTrace(String appName, String traceId, String ip);
        /// <summary>
        /// 
        /// </summary>
        void StopTrace();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="appName"></param>
        /// <param name="traceId"></param>
        void ContinueTrace(String appName, String traceId);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="traceType"></param>
        /// <param name="callResultStatus"></param>
        /// <param name="service"></param>
        /// <param name="tag"></param>
        /// <param name="phase"></param>
        /// <param name="content"></param>
        /// <param name="keyData"></param>
        void Trace(String traceType, String callResultStatus, String service, String tag, LogPhase phase, String content = null, Object keyData = null);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        /// <param name="keyData"></param>
        void Debug(String content, Object keyData = null);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="traceType"></param>
        /// <param name="callResultStatus"></param>
        /// <param name="service"></param>
        /// <param name="tag"></param>
        /// <param name="phase"></param>
        /// <param name="content"></param>
        /// <param name="keyData"></param>
        void Debug(String traceType, String callResultStatus, String service, String tag, LogPhase phase, String content = null, Object keyData = null);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="traceType"></param>
        /// <param name="callResultStatus"></param>
        /// <param name="service"></param>
        /// <param name="tag"></param>
        /// <param name="phase"></param>
        /// <param name="content"></param>
        /// <param name="keyData"></param>
        /// <param name="exception"></param>
        void Error(String traceType, String callResultStatus, String service, String tag, String content, Exception exception = null, Object keyData = null);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="traceType"></param>
        /// <param name="callResultStatus"></param>
        /// <param name="service"></param>
        /// <param name="tag"></param>
        /// <param name="phase"></param>
        /// <param name="content"></param>
        /// <param name="keyData"></param>
        /// <param name="exception"></param>
        void Error(String traceType, String callResultStatus, String service, String tag, LogPhase phase, String content, Exception exception = null, Object keyData = null);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="message"></param>
        void Error(Exception ex, String message, Object keyData = null);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="keyData"></param>
        void Error(String message, Object keyData = null);
    }
}
