using System;
using System.Collections.Generic;
using System.Text;
using ATBase.Serialization;

namespace ATBase.Payment.Bill99.Domain
{
    /// <summary>
    /// 
    /// </summary>
    public class PCIQueryRequest : MasMessage
    {
        /// <summary>
        /// 
        /// </summary>
        [XElement("PciQueryContent")]
        public PCIQueryRequestContent PCIQueryContent { get; set; }
    }
}
