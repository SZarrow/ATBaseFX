using System;
using System.Collections.Generic;
using System.Text;
using ATBase.Serialization;

namespace ATBase.Payment.Bill99.Domain
{
    /// <summary>
    /// 
    /// </summary>
    public class AgreementBindRequest : MasMessage
    {
        /// <summary>
        /// 
        /// </summary>
        [XElement("indAuthDynVerifyContent")]
        public IndAuthDynVerifyRequestContent IndAuthDynVerifyContent { get; set; }
    }
}
