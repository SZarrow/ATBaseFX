using System;
using System.Collections.Generic;
using System.Text;
using ATBase.Serialization;

namespace ATBase.Payment.Bill99.Domain
{
    /// <summary>
    /// 
    /// </summary>
    [XElement("indAuthContent")]
    public class IndAuthResponseContent
    {
        /// <summary>
        /// 商户Id
        /// </summary>
        [XElement("merchantId")]
        public String MerchantId { get; set; }
        /// <summary>
        /// 终端Id
        /// </summary>
        [XElement("terminalId")]
        public String TerminalId { get; set; }
        /// <summary>
        /// 客户Id
        /// </summary>
        [XElement("customerId")]
        public String CustomerId { get; set; }
        /// <summary>
        /// 外部跟踪编号，最长 32 个字节（字母和数字字符）
        /// </summary>
        [XElement("externalRefNumber")]
        public String ExternalRefNumber { get; set; }
        /// <summary>
        /// 银行卡号
        /// </summary>
        [XElement("pan")]
        public String Pan { get; set; }
        /// <summary>
        /// 缩略银行卡号
        /// </summary>
        [XElement("storablePan")]
        public String StorablePan { get; set; }
        /// <summary>
        /// 支付令牌
        /// </summary>
        [XElement("payToken")]
        public String PayToken { get; set; }
        /// <summary>
        /// 令牌
        /// </summary>
        [XElement("token")]
        public String Token { get; set; }
        /// <summary>
        /// 应答码，2 位定长的字母和数字字符，
        /// 返回00表示交易处理完结，消费交易请求处理成功。
        /// C0表示消费交易请求已经提交发卡银行处理，稍后将通知商户端处理结果，商户端挂起当前请求，等待快钱的通知消息。
        /// 68表示快钱未收到银行端的结果、最终交易结果未知。
        /// 96表示系统异常、失效，请联系快钱。
        /// </summary>
        [XElement("responseCode")]
        public String ResponseCode { get; set; }
        /// <summary>
        /// 应答文本信息，最长 128 字节的任意字符（包括中文字符）
        /// </summary>
        [XElement("responseTextMessage")]
        public String ResponseTextMessage { get; set; }
    }
}
