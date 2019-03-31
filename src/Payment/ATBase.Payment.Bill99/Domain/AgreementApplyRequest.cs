using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using ATBase.Serialization;

namespace ATBase.Payment.Bill99.Domain
{
    /// <summary>
    /// 
    /// </summary>
    public class AgreementApplyRequest : MasMessage
    {
        /// <summary>
        /// 
        /// </summary>
        [XElement("indAuthContent")]
        public IndAuthRequestContent IndAuthContent { get; set; }
    }
}
