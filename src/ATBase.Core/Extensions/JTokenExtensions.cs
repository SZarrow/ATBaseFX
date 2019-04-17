using System;
using Newtonsoft.Json.Linq;

namespace ATBase.Core
{
    /// <summary>
    /// 
    /// </summary>
    public static class JTokenExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="token"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static T GetValue<T>(this JToken token, String name)
        {
            if (name.HasValue())
            {
                try
                {
                    return token.Value<T>(name);
                }
                catch { }
            }

            return default(T);
        }
    }
}
