using System;
using System.Collections.Generic;
using System.Text;
using ATBase.Serialization;

namespace ATBase.Payment.Bill99.Domain
{
    /// <summary>
    /// 
    /// </summary>
    public class EntrustPayRequest : MasMessage
    {
        /// <summary>
        /// 
        /// </summary>
        [XElement("TxnMsgContent")]
        public EntrustPayRequestContent EntrustPayRequestContent { get; set; }
    }
}
