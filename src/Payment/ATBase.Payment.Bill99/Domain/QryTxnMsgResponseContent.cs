﻿using System;
using System.Collections.Generic;
using System.Text;
using ATBase.Serialization;

namespace ATBase.Payment.Bill99.Domain
{
    /// <summary>
    /// 
    /// </summary>
    [XElement("TxnMsgContent")]
    public class QryTxnMsgResponseContent
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
        /// 交易状态：S表示交易成功，F表示交易失败，P表示交易挂起
        /// </summary>
        [XElement("txnStatus")]
        public String TxnStatus { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [XElement("amount")]
        public String Amount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [XElement("merchantId")]
        public String MerchantId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [XElement("terminalId")]
        public String TerminalId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [XElement("entryTime")]
        public String EntryTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [XElement("externalRefNumber")]
        public String ExternalRefNumber { get; set; }
        /// <summary>
        /// 交易传输时间
        /// </summary>
        [XElement("transTime")]
        public String TransTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [XElement("voidFlag")]
        public String VoidFlag { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [XElement("refNumber")]
        public String RefNumber { get; set; }
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
        /// 
        /// </summary>
        [XElement("responseTextMessage")]
        public String ResponseTextMessage { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [XElement("authorizationCode")]
        public String AuthorizationCode { get; set; }
    }
}
