using System;
using System.Collections.Generic;
using System.Text;
using ATBase.Core.Extensions;
using ATBase.Serialization;

namespace ATBase.Payment.Bill99.Domain
{
    /// <summary>
    /// 
    /// </summary>
    [XElement("TxnMsgContent")]
    public class EntrustPayRequestContent
    {
        /// <summary>
        /// 
        /// </summary>
        [XElement("interactiveStatus")]
        public String InteractiveStatus { get; set; }
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
        /// 商户端交易时间
        /// </summary>
        [XElement("entryTime")]
        public String EntryTime { get; set; }
        /// <summary>
        /// 银行卡号
        /// </summary>
        [XElement("cardNo")]
        public String CardNo { get; set; }
        /// <summary>
        /// 交易金额
        /// </summary>
        [XElement("amount")]
        public String Amount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [XElement("externalRefNumber")]
        public String ExternalRefNumber { get; set; }
        /// <summary>
        /// 持卡人姓名
        /// </summary>
        [XElement("cardHolderName")]
        public String CardHolderName { get; set; }
        /// <summary>
        /// 持卡客户的证件类型，0表示身份证
        /// </summary>
        [XElement("idType")]
        public String IdType { get; set; }
        /// <summary>
        /// 客户身份证号
        /// </summary>
        [XElement("cardHolderId")]
        public String CardHolderId { get; set; }
        /// <summary>
        /// 扩展字段
        /// </summary>
        [XElement("extMap")]
        public ExtMap ExtMap { get; set; }
    }
}
