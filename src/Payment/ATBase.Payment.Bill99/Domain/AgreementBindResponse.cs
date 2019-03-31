using System;
using System.Collections.Generic;
using System.Text;
using ATBase.Serialization;

namespace ATBase.Payment.Bill99.Domain
{
    /// <summary>
    /// 
    /// </summary>
    public class AgreementBindResponse : MasMessage
    {
        /// <summary>
        /// 
        /// </summary>
        [XElement("ErrorMsgContent")]
        public ErrorMsgContent ErrorMsgContent { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [XElement("indAuthDynVerifyContent")]
        public IndAuthDynVerifyResponseContent IndAuthDynVerifyContent { get; set; }
    }
}
