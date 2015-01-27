//using System;
//using System.Collections.Specialized;
//using System.Linq;
//using System.Runtime.Caching;

//namespace EPiPugPigConnector.Caching
//{
//    public class CacheProvider : MemoryCache, ICacheProvider
//    {
//        private CacheProvider _instance;
//        private const string PUGPIG_CACHE_INSTANCE_NAME = "PugPigConnector_Cache";

//        public CacheProvider Instance
//        {
//            get { return _instance; }
//            set { _instance = value; }
//        }


//        private CacheProvider() 
//            : this(PUGPIG_CACHE_INSTANCE_NAME, null)
//        {
            
//        }

//        private CacheProvider(string name, NameValueCollection config) 
//            : base(name, config)
//        {

//        }

//        public object Get(string key)
//        {
//            return this[key];
//        }

//        public void Set(string key, object data, DateTimeOffset expirationTime)
//        {
//            CacheItemPolicy policy = new CacheItemPolicy();
//            policy.AbsoluteExpiration = expirationTime;

//            this.Add(new CacheItem(key, data), policy);
//        }

//        public bool IsSet(string key)
//        {
//            return (this[key] != null);
//        }

//        public void Invalidate(string key)
//        {
//            this.Remove(key);
//        }

//        public void ClearAllCache()
//        {
//            this.Dispose();
//            new CacheProvider();
//        }


//    }

//}
