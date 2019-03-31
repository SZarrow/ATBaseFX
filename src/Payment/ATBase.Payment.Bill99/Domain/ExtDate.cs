using System;
using System.Collections.Generic;
using System.Text;
using ATBase.Serialization;

namespace ATBase.Payment.Bill99.Domain
{
    /// <summary>
    /// 
    /// </summary>
    [XElement("extDate")]
    public class ExtDate
    {
        /// <summary>
        /// 
        /// </summary>
        [XElement("key")]
        public String Key { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [XElement("value")]
        public String Value { get; set; }
    }
}
