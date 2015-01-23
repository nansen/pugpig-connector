using System;
using System.Linq;
using System.Runtime.Caching;

namespace EPiPugPigConnector.Caching
{
    public class DefaultCacheProvider : ICacheProvider
    {
        #region ICacheProvider
        void ICacheProvider.Set(string key, object data, DateTimeOffset expirationTime)
        {
            Set(key, data, expirationTime);
        }

        bool ICacheProvider.IsSet(string key)
        {
            return IsSet(key);
        }

        void ICacheProvider.Invalidate(string key)
        {
            Invalidate(key);
        }

        object ICacheProvider.Get(string key)
        {
            return Get(key);
        }
        #endregion

        private static ObjectCache Cache
        {
            get
            {
                return MemoryCache.Default;
            }
        }

        public static object Get(string key)
        {
            return Cache[key];
        }

        public static void Set(string key, object data, DateTimeOffset expirationTime)
        {
            CacheItemPolicy policy = new CacheItemPolicy();
            policy.AbsoluteExpiration = expirationTime;

            Cache.Add(new CacheItem(key, data), policy);
        }

        public static bool IsSet(string key)
        {
            return (Cache[key] != null);
        }

        public static void Invalidate(string key)
        {
            Cache.Remove(key);
        }
    }
}
