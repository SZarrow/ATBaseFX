using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using ATBase.Serialization;

namespace ATBase.Payment.Bill99.Domain
{
    /// <summary>
    /// 
    /// </summary>
    public class AgreementApplyResponse : MasMessage
    {
        /// <summary>
        /// 
        /// </summary>
        [XElement("ErrorMsgContent")]
        public ErrorMsgContent ErrorMsgContent { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [XElement("indAuthContent")]
        public IndAuthResponseContent IndAuthContent { get; set; }
    }
}
