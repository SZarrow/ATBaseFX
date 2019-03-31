using System;
using System.Collections.Generic;
using System.Text;
using ATBase.Serialization;

namespace ATBase.Payment.Bill99.Domain
{
    /// <summary>
    /// 
    /// </summary>
    [XElement("TxnMsgContent")]
    public class TxnMsgRequestContent
    {
        /// <summary>
        /// 交易类型编码，3 位定长字母字符，
        /// PUR表示消费交易，INP表示分期消费交易,
        /// PRE表示预授权交易，CFM表示预授权完成交易,
        /// VTX表示撤销交易，RFD表示退货交易，
        /// CIV表示卡信息验证交易
        /// </summary>
        [XElement("txnType")]
        public String TxnType { get; set; }
        /// <summary>
        /// 消息状态，标识当前消息对应的交互状态。
        /// </summary>
        [XElement("interactiveStatus")]
        public String InteractiveStatus { get; set; }
        /// <summary>
        /// 商户端交易时间
        /// </summary>
        [XElement("entryTime")]
        public String EntryTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [XElement("externalRefNumber")]
        public String ExternalRefNumber { get; set; }
        /// <summary>
        /// 交易金额
        /// </summary>
        [XElement("amount")]
        public String Amount { get; set; }
        /// <summary>
        /// 最长 3 字节的字母和数字字符
        /// </summary>
        [XElement("spFlag")]
        public String SpFlag { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [XElement("customerId")]
        public String CustomerId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [XElement("payToken")]
        public String PayToken { get; set; }
        /// <summary>
        /// 回调地址
        /// </summary>
        [XElement("tr3Url")]
        public String NotifyUrl { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [XElement("extMap")]
        public ExtMap ExtMap { get; set; }
    }
}
