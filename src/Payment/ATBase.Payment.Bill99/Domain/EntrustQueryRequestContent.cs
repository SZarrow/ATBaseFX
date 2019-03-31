using System;
using System.Collections.Generic;
using System.Text;
using ATBase.Serialization;

namespace ATBase.Payment.Bill99.Domain
{
    /// <summary>
    /// 
    /// </summary>
    [XElement("QryTxnMsgContent")]
    public class EntrustQueryRequestContent
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
        /// 
        /// </summary>
        [XElement("externalRefNumber")]
        public String ExternalRefNumber { get; set; }
    }
}
