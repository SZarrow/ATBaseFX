﻿using System;
using System.ComponentModel;
using System.Reflection;

namespace ATBase.Core
{
    /// <summary>
    /// String扩展类
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// 将当前字符串安全地转换成Int32类型
        /// </summary>
        /// <param name="str"></param>
        public static Int32 ToInt32(this String str)
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
        public static Int64 ToInt64(this String str)
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
        public static Decimal ToDecimal(this String str)
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

        /// <summary>
        /// 将当前字符串安全地转换成DateTime类型
        /// </summary>
        /// <param name="str"></param>
        public static DateTime? ToDateTime(this String str)
        {
            if (str.HasValue() && DateTime.TryParse(str, out DateTime value))
            {
                return value;
            }

            return null;
        }

        /// <summary>
        /// 判断当前字符串是否为null，空格或空字符串。
        /// </summary>
        /// <param name="str"></param>
        public static Boolean IsNullOrWhiteSpace(this String str)
        {
            return String.IsNullOrWhiteSpace(str);
        }

        /// <summary>
        /// 判断当前字符串是否有值
        /// </summary>
        /// <param name="str"></param>
        public static Boolean HasValue(this String str)
        {
            return !str.IsNullOrWhiteSpace();
        }

        /// <summary>
        /// 获取当前枚举类字面值的描述信息
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        public static String GetDescription<TEnum>(this String literal)
        {
            var descAttr = typeof(TEnum).GetField(literal).GetCustomAttribute<DescriptionAttribute>();

            if (descAttr != null)
            {
                return descAttr.Description;
            }

            return literal;
        }
    }
}
