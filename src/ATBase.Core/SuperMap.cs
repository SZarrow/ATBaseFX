using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace ATBase.Core
{
    /// <summary>
    /// 
    /// </summary>
    public static class SuperMap
    {
        private static ConcurrentDictionary<Type, Object> _cache = new ConcurrentDictionary<Type, Object>();

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keyValues">键值对</param>
        public static T Map<T>(IDictionary<String, Object> keyValues) where T : new()
        {
            var func = GetSuperFunc<T>();
            if (func != null)
            {
                return func(keyValues);
            }

            return default(T);
        }

        private static Func<IDictionary<String, Object>, T> GetSuperFunc<T>()
        {
            if (!_cache.TryGetValue(typeof(T), out Object value))
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("");
            }

            return value as Func<IDictionary<String, Object>, T>;
        }
    }
}
