using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Primitives;

namespace ATBase.Core
{
    /// <summary>
    /// StringValues扩展类
    /// </summary>
    public static class StringValuesExtensions
    {
        /// <summary>
        /// 将当前字符串安全地转换成Int32类型
        /// </summary>
        /// <param name="str"></param>
        public static Int32 ToInt32(this StringValues str)
        {
            if (!String.IsNullOrWhiteSpace(str))
            {
                Int32 value;
                if (Int32.TryParse(str, out value))
                {
                    return value;
                }
            }

            return 0;
        }

        /// <summary>
        /// 将当前字符串安全地转换成Int64类型
        /// </summary>
        /// <param name="str"></param>
        public static Int64 ToInt64(this StringValues str)
        {
            if (!String.IsNullOrWhiteSpace(str))
            {
                Int64 value;
                if (Int64.TryParse(str, out value))
                {
                    return value;
                }
            }

            return 0;
        }

        /// <summary>
        /// 将当前字符串安全地转换成Decimal类型
        /// </summary>
        /// <param name="str"></param>
        public static Decimal ToDecimal(this StringValues str)
        {
            if (!String.IsNullOrWhiteSpace(str))
            {
                Decimal value;
                if (Decimal.TryParse(str, out value))
                {
                    return value;
                }
            }

            return 0;
        }
    }
}
