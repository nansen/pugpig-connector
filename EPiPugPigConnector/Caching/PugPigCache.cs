using System;
using System.Text.RegularExpressions;
using EPiServer.Core;
using EPiPugPigConnector.EPiExtensions;
using EPiPugPigConnector.Helpers;
using EPiPugPigConnector.Logging;

namespace EPiPugPigConnector.Caching
{
    public static class PugPigCache
    {
        private const string PUGPIG_CACHE_PREFIX = "PUGPIG";

        public static void Set(PugPigCacheType cacheType, string urlCacheKey, object data)
        {
            string cacheKey = GetCacheKeyFrom(cacheType, urlCacheKey);
            DateTimeOffset expirationTime = GetCacheExpirationTime(cacheType);

            DefaultCacheProvider.Set(cacheKey, data, expirationTime);
        }

        public static object Get(PugPigCacheType cacheType, string urlCacheKey)
        {
            string cacheKey = GetCacheKeyFrom(cacheType, urlCacheKey);
            return DefaultCacheProvider.Get(cacheKey);
        }

        public static bool IsSet(PugPigCacheType cacheType, string urlCacheKey)
        {
            string cacheKey = GetCacheKeyFrom(cacheType, urlCacheKey);
            return DefaultCacheProvider.IsSet(cacheKey);
        }

        public static void Invalidate(PugPigCacheType cacheType, string urlCacheKey)
        {
            string cacheKey = GetCacheKeyFrom(cacheType, urlCacheKey);
            DefaultCacheProvider.Invalidate(cacheKey);
        }

        private static DateTimeOffset GetCacheExpirationTime(PugPigCacheType cacheType)
        {
            switch (cacheType)
            {
                case PugPigCacheType.Manifest:
                    return GetExpirationOffset(new TimeSpan(days: 1, hours: 0, minutes: 0, seconds: 0));
                case PugPigCacheType.Feed:
                    return GetExpirationOffset(new TimeSpan(days: 1, hours: 0, minutes: 0, seconds: 0));
                default:
                    return GetExpirationOffset(new TimeSpan(days: 1, hours: 0, minutes: 0, seconds: 0));
            }
        }

        private static DateTimeOffset GetExpirationOffset(TimeSpan timeSpan)
        {
            DateTimeOffset localServerTime = System.DateTimeOffset.Now;
            TimeSpan expirationTimeSpan = timeSpan;
            DateTimeOffset expirationTime = localServerTime.Add(expirationTimeSpan);
            return expirationTime;
        }

        /// <summary>
        /// Gets the unique pugpig type of cache key.
        /// Avoid conflicting keys with the standard episerver website, with prefix etc.
        /// </summary>
        private static string GetCacheKeyFrom(PugPigCacheType cacheType, string urlCacheKey)
        {
            string cacheTypeString = cacheType.ToString();
            urlCacheKey = GetCleanCacheKeyStringFrom(urlCacheKey);
            urlCacheKey = string.Format("{0}_{1}_{2}", PUGPIG_CACHE_PREFIX, cacheTypeString.ToUpper(), urlCacheKey);
            return urlCacheKey;
        }

        private static string GetCleanCacheKeyStringFrom(string urlCacheKey)
        {
            //Regex rgx = new Regex("[^a-zA-Z0-9 -]"); //just keep letters and numbers
            //urlCacheKey = rgx.Replace(urlCacheKey, "");
            urlCacheKey = urlCacheKey.Trim().Replace(" ", string.Empty);
            urlCacheKey = urlCacheKey.TrimEnd(new[] {'/'}); //removes ending / if present.
            return urlCacheKey;
        }

        /// <summary>
        /// Invalidates cache for the connected manifest file.
        /// </summary>
        public static void InvalidateManifestCache(PageData page)
        {
            if (!PageHelper.IsEditionsContainerOrEditionPage(page))
            {
                var cacheType = PugPigCacheType.Manifest;
                string cacheKey = GetCacheKeyFrom(cacheType, page.GetFriendlyUrl(includeHost: true)); 
                
                if (DefaultCacheProvider.IsSet(cacheKey)) //avoids error logging
                {
                    DefaultCacheProvider.Invalidate(cacheKey);
                    LogHelper.Log("Manifest cache invalidated, cachekey: " + cacheKey, LogLevel.Debug);
                }
            }
        }

        /// <summary>
        /// Invalidates cache for the xml edition or editions feed. 
        /// </summary>
        public static void InvalidateFeedCache(PageData page)
        {
            if (PageHelper.IsEditionsContainerOrEditionPage(page))
            {
                var cacheType = PugPigCacheType.Feed;
                string cacheKey = GetCacheKeyFrom(cacheType, page.ContentLink.ID.ToString());

                if (DefaultCacheProvider.IsSet(cacheKey)) //avoids error logging
                {
                    DefaultCacheProvider.Invalidate(cacheKey);
                    LogHelper.Log("Feed xml cache invalidated, cachekey: " + cacheKey, LogLevel.Debug);
                }
            }
        }
    }
}