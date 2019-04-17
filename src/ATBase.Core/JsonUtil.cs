using System;
using System.Collections.Generic;
using System.Text;
using ATBase.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ATBase.Core
{
    /// <summary>
    /// 
    /// </summary>
    public static class JsonUtil
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public static XResult<String> SerializeObject(Object value)
        {
            if (value == null)
            {
                return new XResult<String>(null, new ArgumentNullException(nameof(value)));
            }

            try
            {
                return new XResult<String>(JsonConvert.SerializeObject(value));
            }
            catch (Exception ex)
            {
                return new XResult<String>(null, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        public static XResult<T> DeserializeObject<T>(String value)
        {
            if (value.IsNullOrWhiteSpace())
            {
                return new XResult<T>(default(T), new ArgumentNullException(nameof(value)));
            }

            try
            {
                return new XResult<T>(JsonConvert.DeserializeObject<T>(value));
            }
            catch (Exception ex)
            {
                return new XResult<T>(default(T), ex);
            }
        }

        private static readonly JToken DefaultJToken = JToken.Parse("{}");

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jsonString"></param>
        public static JToken GetJToken(String jsonString)
        {
            if (jsonString.IsNullOrWhiteSpace())
            {
                return DefaultJToken;
            }

            try
            {
                return JToken.Parse(jsonString, new JsonLoadSettings()
                {
                    LineInfoHandling = LineInfoHandling.Ignore
                });
            }
            catch (Exception)
            {
                return DefaultJToken;
            }
        }
    }
}
