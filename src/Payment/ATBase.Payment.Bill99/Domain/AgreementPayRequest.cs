using System;
using System.Collections.Generic;
using System.Text;
using ATBase.Serialization;

namespace ATBase.Payment.Bill99.Domain
{
    /// <summary>
    /// 
    /// </summary>
    public class AgreementPayRequest : MasMessage
    {
        /// <summary>
        /// 
        /// </summary>
        [XElement("TxnMsgContent")]
        public TxnMsgRequestContent TxnMsgContent { get; set; }
    }
}
