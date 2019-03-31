using System;
using System.Collections.Generic;
using System.Text;
using ATBase.Serialization;

namespace ATBase.Payment.Bill99.Domain
{
    /// <summary>
    /// 
    /// </summary>
    [XElement("pciInfos")]
    public class PCIInfos
    {
        /// <summary>
        /// 
        /// </summary>
        [XCollection]
        public IEnumerable<PCIInfo> Infos { get; set; }
    }
}
