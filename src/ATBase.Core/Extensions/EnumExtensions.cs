using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;

namespace ATBase.Core.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T GetValue<T>(this Enum obj)
        {
            if (obj == null)
            {
                return default(T);
            }

            var field = obj.GetType().GetField(obj.ToString());
            var cusAttr = field.GetCustomAttribute<EnumValueAttribute>();
            if (cusAttr != null)
            {
                if (cusAttr.Value is T)
                {
                    return (T)cusAttr.Value;
                }

                if (typeof(T) == typeof(String))
                {
                    Object value = cusAttr.Value != null ? cusAttr.Value.ToString() : obj.ToString();
                    return (T)value;
                }
            }

            return default(T);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static String GetValue(this Enum obj)
        {
            return GetValue<String>(obj);
        }

        /// <summary>
        /// 获取枚举类的描述信息
        /// </summary>
        /// <param name="eo"></param>
        /// <param name="literal">字面值</param>
        public static String GetDescription(this Enum eo, String literal = null)
        {
            var descAttr = eo.GetType().GetField(literal ?? eo.ToString()).GetCustomAttribute<DescriptionAttribute>();

            if (descAttr != null)
            {
                return descAttr.Description;
            }

            return eo.ToString();
        }
    }
}
