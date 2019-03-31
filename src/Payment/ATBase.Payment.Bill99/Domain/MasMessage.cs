using System;
using System.Collections.Generic;
using System.Text;
using ATBase.Serialization;

namespace ATBase.Payment.Bill99.Domain
{
    /// <summary>
    /// 
    /// </summary>
    [XElement("MasMessage", "http://www.99bill.com/mas_cnp_merchant_interface")]
    public abstract class MasMessage
    {
        /// <summary>
        /// 
        /// </summary>
        protected MasMessage() { }
        /// <summary>
        /// 
        /// </summary>
        [XElement("version")]
        public String Version { get; set; }
    }
}
