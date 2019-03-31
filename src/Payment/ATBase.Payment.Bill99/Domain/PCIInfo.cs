using System;
using System.Collections.Generic;
using System.Text;
using ATBase.Serialization;

namespace ATBase.Payment.Bill99.Domain
{
    /// <summary>
    /// 
    /// </summary>
    [XElement("pciInfo")]
    public class PCIInfo
    {
        /// <summary>
        /// 银行简称，最长 6 个字节的字母，可不传
        /// </summary>
        [XElement("bankId")]
        public String BankId { get; set; }
        /// <summary>
        /// 缩略银行卡号
        /// </summary>
        [XElement("storablePan")]
        public String StorablePan { get; set; }
        /// <summary>
        /// 缩略手机号
        /// </summary>
        [XElement("shortPhoneNo")]
        public String ShortPhoneNo { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        [XElement("phoneNO")]
        public String PhoneNO { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [XElement("payToken")]
        public String PayToken { get; set; }
    }
}
