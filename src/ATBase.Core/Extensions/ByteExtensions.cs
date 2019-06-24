using System;
using System.Collections.Generic;
using System.Text;

namespace ATBase.Core
{
    /// <summary>
    /// 
    /// </summary>
    public static class ByteExtensions
    {
        /// <summary>
        /// 将指定的Byte[]转换成16进制字符串
        /// </summary>
        /// <param name="value"></param>
        public static String ToHex(this Byte[] value)
        {
            if (value == null)
            {
                return null;
            }

            if (value.Length == 0)
            {
                return String.Empty;
            }

            StringBuilder sb = new StringBuilder();
            foreach (var b in value)
            {
                sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }
    }
}
