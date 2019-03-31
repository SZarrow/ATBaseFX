using System;
using System.Collections.Generic;
using System.Text;

namespace ATBase.Core.Caching
{
    /// <summary>
    /// 缓存接口
    /// </summary>
    public interface ICache
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="absoluteExpireTime"></param>
        void Set<T>(Object key, T value, DateTime absoluteExpireTime);
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="absoluteExpirationRelativeToNow"></param>
        void Set<T>(Object key, T value, TimeSpan absoluteExpirationRelativeToNow);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        Boolean TryGetValue(Object key, out Object value);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        T Get<T>(Object key);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        void Remove(Object key);
    }
}
