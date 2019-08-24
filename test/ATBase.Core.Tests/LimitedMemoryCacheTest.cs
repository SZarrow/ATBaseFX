using System;
using System.Threading;
using ATBase.Core.Caching;
using Xunit;

namespace ATBase.Core.Tests
{
    public class LimitedMemoryCacheTest
    {
        private static readonly LimitedMemoryCache _cache = new LimitedMemoryCache(1000);

        [Fact]
        public void TestGet()
        {
            _cache.Set("1", "123", DateTime.Now.AddSeconds(6));
            Thread.Sleep(5 * 1000);
            var existed = _cache.Get<String>("1");
            Assert.Equal("123", existed);

            _cache.Set("1", "123", DateTime.Now.AddSeconds(9));
            Thread.Sleep(10 * 1000);
            existed = _cache.Get<String>("1");
            Assert.Null(existed);
        }
    }
}
