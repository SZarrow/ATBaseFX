using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ATBase.Core
{
    /// <summary>
    /// 错误码描述器
    /// </summary>
    public static class ErrorCodeDescriptor
    {
        private static readonly Dictionary<Int32, String> _errorDic = new Dictionary<Int32, String>(100);

        /// <summary>
        /// 
        /// </summary>
        public static String GetDescription(Int32 errorCode)
        {
            if (_errorDic.TryGetValue(errorCode, out String value))
            {
                return value;
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        public static void AddErrorCodeTypes(params Type[] errorCodeTypes)
        {
            if (errorCodeTypes != null && errorCodeTypes.Length > 0)
            {
                foreach (var errorCodeType in errorCodeTypes)
                {
                    ParseDescription(errorCodeType);
                }
            }
        }

        /// <summary>
        /// 获取指定状态码对应的描述信息
        /// </summary>
        /// <param name="errorCodeType"></param>
        private static void ParseDescription(Type errorCodeType)
        {
            var fields = (from t0 in errorCodeType.GetFields(BindingFlags.Public | BindingFlags.Static)
                          select t0);

            if (fields != null)
            {
                foreach (var field in fields)
                {
                    var cusAttr = field.GetCustomAttribute<DescriptionAttribute>();
                    if (cusAttr != null && !String.IsNullOrWhiteSpace(cusAttr.Description))
                    {
                        if (TryGetFieldValue(field, out Int32 errorCode))
                        {
                            _errorDic[errorCode] = cusAttr.Description;
                        }
                    }
                }
            }
        }

        private static Boolean TryGetFieldValue(FieldInfo fieldInfo, out Int32 value)
        {
            value = 1;
            if (fieldInfo == null) { return false; }
            var o = fieldInfo.XGetValue(null);
            if (o == null) { return false; }
            if (Int32.TryParse(o.ToString(), out Int32 result))
            {
                value = result;
                return true;
            }
            return false;
        }
    }
}
