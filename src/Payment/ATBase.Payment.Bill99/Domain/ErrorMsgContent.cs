using System;
using System.Collections.Generic;
using System.Text;
using ATBase.Serialization;

namespace ATBase.Payment.Bill99.Domain
{
    /// <summary>
    /// 
    /// </summary>
    [XElement("ErrorMsgContent")]
    public class ErrorMsgContent
    {
        /// <summary>
        /// 对于管理类型操作请求处理出现错误或者系统级别错误时，返回的错误代码
        /// </summary>
        [XElement("errorCode")]
        public String ErrorCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [XElement("errorMessage")]
        public String ErrorMessage { get; set; }
    }
}
