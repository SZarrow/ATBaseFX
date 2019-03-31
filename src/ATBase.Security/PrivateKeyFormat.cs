using System;
using System.Collections.Generic;
using System.Text;

namespace ATBase.Security
{
    /// <summary>
    /// 私钥编码格式，默认为PKCS1，此格式常用于.NET环境。
    /// PKCS8为Java环境的私钥格式，如果想用此类库处理Java环境提供的私钥，
    /// 请使用PKCS8格式。
    /// </summary>
    public enum PrivateKeyFormat
    {
        /// <summary>
        /// .NET支持的私钥格式
        /// </summary>
        PKCS1,
        /// <summary>
        /// Java支持的私钥格式
        /// </summary>
        PKCS8
    }
}
