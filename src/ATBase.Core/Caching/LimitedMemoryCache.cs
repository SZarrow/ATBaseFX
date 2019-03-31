using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Microsoft.Extensions.Caching.Memory;

namespace ATBase.Core.Caching
{
    /// <summary>
    /// 受限的内存缓存
    /// </summary>
    public class LimitedMemoryCache : ICache, IDisposable
    {
        private readonly MemoryCache _cache;
        private readonly Int32 _limitCount;

        /// <summary>
        /// 初始化LimitedMemoryCache的实例
        /// </summary>
        /// <param name="limitCount">设置当前缓存实例最大缓存的个数</param>
        public LimitedMemoryCache(Int32 limitCount) : this(limitCount, new TimeSpan(0, 0, 1)) { }

        /// <summary>
        /// 初始化LimitedMemoryCache的实例
        /// </summary>
        /// <param name="limitCount">设置当前缓存实例最大缓存的个数</param>
        /// <param name="expirationScanFrequency">设置每隔多长时间扫描一次过期缓存</param>
        public LimitedMemoryCache(Int32 limitCount, TimeSpan expirationScanFrequency)
        {
            if (limitCount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(limitCount));
            }

            _limitCount = limitCount;
            _cache = new MemoryCache(new MemoryCacheOptions()
            {
                ExpirationScanFrequency = expirationScanFrequency
            });
        }

        /// <summary>
        /// 
        /// </summary>
        public Int32 Count
        {
            get
            {
                return _cache.Count;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            _cache.Dispose();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        public void Remove(Object key)
        {
            if (key != null)
            {
                _cache.Remove(key);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="absoluteExpiration"></param>
        public void Set<T>(Object key, T value, DateTime absoluteExpiration)
        {
            if (_cache.Count < _limitCount && key != null)
            {
                _cache.Set(key, value, new DateTimeOffset(absoluteExpiration));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="absoluteExpirationRelativeToNow"></param>
        public void Set<T>(Object key, T value, TimeSpan absoluteExpirationRelativeToNow)
        {
            if (_cache.Count < _limitCount && key != null)
            {
                _cache.Set(key, value, absoluteExpirationRelativeToNow);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public Boolean TryGetValue(Object key, out Object value)
        {
            if (key == null)
            {
                value = null;
                return false;
            }

            return _cache.TryGetValue(key, out value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        public T Get<T>(Object key)
        {
            Object value;
            if (TryGetValue(key, out value) && value is T)
            {
                return (T)value;
            }

            return default(T);
        }
    }
}
