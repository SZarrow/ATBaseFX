using System;
using System.Collections.Generic;
using System.Text;
using ATBase.Serialization;

namespace ATBase.Payment.Bill99.Domain
{
    /// <summary>
    /// 
    /// </summary>
    public class EntrustPayResponse:MasMessage
    {
        /// <summary>
        /// 
        /// </summary>
        [XElement("TxnMsgContent")]
        public EntrustPayResponseContent EntrustPayResponseContent { get; set; }
    }
}
