using System;
using System.Collections.Generic;
using System.Text;
using ATBase.Serialization;

namespace ATBase.Payment.Bill99.Domain
{
    /// <summary>
    /// 
    /// </summary>
    public class AgreementQueryRequest : MasMessage
    {
        /// <summary>
        /// 
        /// </summary>
        [XElement("QryTxnMsgContent")]
        public QryTxnMsgRequestContent QryTxnMsgContent { get; set; }
    }
}
