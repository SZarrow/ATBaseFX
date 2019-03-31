using System;
using System.Collections.Generic;
using System.Text;
using ATBase.Serialization;

namespace ATBase.Payment.Bill99.Domain
{
    /// <summary>
    /// 
    /// </summary>
    [XElement("PciQueryContent")]
    public class PCIQueryRequestContent
    {
        /// <summary>
        /// 
        /// </summary>
        [XElement("customerId")]
        public String CustomerId { get; set; }

        /// <summary>
        /// 卡类型，4 位定长的数字字符，
        /// 0000表示银行卡，0001表示信用卡，0002表示借记卡
        /// </summary>
        [XElement("cardType")]
        public String CardType { get; set; }

        /// <summary>
        /// 可不传
        /// </summary>
        [XElement("payToken")]
        public String PayToken { get; set; }

        /// <summary>
        /// 银行简称，最长 6 个字节的字母，可不传
        /// </summary>
        [XElement("bankId")]
        public String BankId { get; set; }
    }
}
