﻿using System;
using System.Collections.Generic;
using System.Text;
using ATBase.Serialization;

namespace ATBase.Payment.Bill99.Domain
{
    /// <summary>
    /// 
    /// </summary>
    public class EntrustQueryRequest : MasMessage
    {
        /// <summary>
        /// 
        /// </summary>
        [XElement("QryTxnMsgContent")]
        public EntrustQueryRequestContent EntrustQueryRequestContent { get; set; }
    }
}
