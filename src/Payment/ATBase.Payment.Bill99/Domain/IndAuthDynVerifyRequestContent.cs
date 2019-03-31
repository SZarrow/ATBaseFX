using System;
using System.Collections.Generic;
using System.Text;
using ATBase.Serialization;

namespace ATBase.Payment.Bill99.Domain
{
    /// <summary>
    /// 
    /// </summary>
    [XElement("indAuthDynVerifyContent")]
    public class IndAuthDynVerifyRequestContent
    {
        /// <summary>
        /// 
        /// </summary>
        [XElement("customerId")]
        public String CustomerId { get; set; }
        /// <summary>
        /// 外部跟踪号
        /// </summary>
        [XElement("externalRefNumber")]
        public String ExternalRefNumber { get; set; }
        /// <summary>
        /// 银行卡号
        /// </summary>
        [XElement("pan")]
        public String Pan { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        [XElement("phoneNO")]
        public String PhoneNO { get; set; }
        /// <summary>
        /// 验证码
        /// </summary>
        [XElement("validCode")]
        public String ValidCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [XElement("token")]
        public String Token { get; set; }
        /// <summary>
        /// 绑定类型，默认为0
        /// </summary>
        [XElement("bindType")]
        public String BindType { get; set; }
    }
}
