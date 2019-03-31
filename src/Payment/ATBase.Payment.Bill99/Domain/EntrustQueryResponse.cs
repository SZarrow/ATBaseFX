using System;
using System.Collections.Generic;
using System.Text;
using ATBase.Serialization;

namespace ATBase.Payment.Bill99.Domain
{
    /// <summary>
    /// 
    /// </summary>
    public class EntrustQueryResponse : MasMessage
    {
        /// <summary>
        /// 
        /// </summary>
        [XElement("QryTxnMsgContent")]
        public EntrustQueryQryTxnMsgContent EntrustQueryQryTxnMsgContent { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [XElement("TxnMsgContent")]
        public EntrustQueryTxnMsgContent EntrustQueryTxnMsgContent { get; set; }
    }
}
