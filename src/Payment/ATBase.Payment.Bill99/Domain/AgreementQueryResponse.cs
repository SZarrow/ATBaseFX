using System;
using System.Collections.Generic;
using System.Text;
using ATBase.Serialization;

namespace ATBase.Payment.Bill99.Domain
{
    /// <summary>
    /// 
    /// </summary>
    public class AgreementQueryResponse : MasMessage
    {
        /// <summary>
        /// 
        /// </summary>
        [XElement("ErrorMsgContent")]
        public ErrorMsgContent ErrorMsgContent { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [XElement("TxnMsgContent")]
        public QryTxnMsgResponseContent QryTxnMsgContent { get; set; }
    }
}
