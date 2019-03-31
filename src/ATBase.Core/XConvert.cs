using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace ATBase.Core
{
    /// <summary>
    /// 超级转换类。
    /// Code copied from DotNetWheels.Core.
    /// </summary>
    public static class XConvert
    {
        private readonly static Dictionary<String, MethodInfo> _tryParseMethodCache = new Dictionary<String, MethodInfo>(15);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="targetType"></param>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        public static ObjectResult TryParse(Type targetType, Object value, Object defaultValue)
        {
            return TryParseCore(targetType, value, GetSystemDefaultValue(targetType, defaultValue));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        public static T TryParse<T>(Object value, T defaultValue = default(T))
        {
            var result = TryParseCore(typeof(T), value, defaultValue);
            return result.FirstException != null ? defaultValue : ((result.Value is T) ? (T)result.Value : defaultValue);
        }

        private static ObjectResult TryParseCore(Type targetType, Object value, Object defaultValue = null)
        {
            if (targetType.Name == "Nullable`1")
            {
                targetType = targetType.GenericTypeArguments[0];
            }

            if (targetType.IsEnum && value != null)
            {
                String strValue = value.ToString();
                if (Regex.IsMatch(strValue, @"^\d+$", RegexOptions.IgnoreCase))
                {
                    Int32 intValue;
                    if (Int32.TryParse(strValue, out intValue))
                    {
                        try
                        {
                            var obj = Enum.ToObject(targetType, intValue);
                            if (obj.GetType() == targetType)
                            {
                                return new ObjectResult(obj);
                            }
                        }
                        catch (Exception) { }
                    }
                }
                else
                {
                    var enumValues = Enum.GetValues(targetType);
                    for (var i = 0; i < enumValues.Length; i++)
                    {
                        var enumValue = enumValues.GetValue(i);
                        if (String.Compare(enumValue.ToString(), strValue, true) == 0)
                        {
                            return new ObjectResult(enumValue);
                        }
                    }
                    return new ObjectResult(defaultValue, new InvalidCastException($"无法将{strValue}转换成{targetType.ToString()}"));
                }
            }

            if (!_tryParseMethodCache.ContainsKey(targetType.FullName))
            {
                var tryParseMethodInfo = targetType.GetMethod("TryParse", new Type[] { typeof(String), targetType.MakeByRefType() });

                if (tryParseMethodInfo != null)
                {
                    _tryParseMethodCache[targetType.FullName] = tryParseMethodInfo;
                }
            }

            if (_tryParseMethodCache.ContainsKey(targetType.FullName))
            {
                var methodInfo = _tryParseMethodCache[targetType.FullName];
                if (methodInfo != null)
                {
                    Object[] args = new Object[] { value != null ? value.ToString() : null, null };

                    try
                    {
                        Boolean convertSuccess = (Boolean)methodInfo.XInvoke(null, args);
                        return new ObjectResult(convertSuccess ? args[1] : defaultValue);
                    }
                    catch (Exception ex)
                    {
                        return new ObjectResult(defaultValue, ex);
                    }
                }
            }

            if (targetType == typeof(String))
            {
                return new ObjectResult(value == null ? defaultValue : value.ToString());
            }

            return new ObjectResult(value == null ? defaultValue : value);
        }

        private static Object GetSystemDefaultValue(Type targetType, Object defaultValue)
        {
            if (targetType.Name == "Nullable`1")
            {
                return defaultValue;
            }

            if (defaultValue != null
                && defaultValue.GetType().FullName == targetType.FullName)
            {
                return defaultValue;
            }

            var typeCode = Type.GetTypeCode(targetType);
            switch (typeCode)
            {
                case TypeCode.Boolean:
                    return default(Boolean);
                case TypeCode.Byte:
                    return default(Byte);
                case TypeCode.Char:
                    return default(Char);
                case TypeCode.DateTime:
                    return DateTime.Parse("1900-01-01");
                case TypeCode.Decimal:
                    return default(Decimal);
                case TypeCode.Double:
                    return default(Double);
                case TypeCode.Int16:
                    return default(Int16);
                case TypeCode.Int32:
                    return default(Int32);
                case TypeCode.Int64:
                    return default(Int64);
                case TypeCode.SByte:
                    return default(SByte);
                case TypeCode.Single:
                    return default(Single);
                case TypeCode.UInt16:
                    return default(UInt16);
                case TypeCode.UInt32:
                    return default(UInt32);
                case TypeCode.UInt64:
                    return default(UInt64);
                default:
                    return defaultValue;
            }
        }
    }
}
