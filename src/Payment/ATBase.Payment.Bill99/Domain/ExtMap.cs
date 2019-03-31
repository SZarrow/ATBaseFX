using System;
using System.Collections.Generic;
using System.Text;
using ATBase.Serialization;

namespace ATBase.Payment.Bill99.Domain
{
    /// <summary>
    /// 
    /// </summary>
    [XElement("extMap")]
    public class ExtMap
    {
        /// <summary>
        /// 
        /// </summary>
        [XCollection]
        public IEnumerable<ExtDate> ExtDates { get; set; }
    }
}
