using System;

namespace EPiPugPigConnector.Caching
{
    public interface ICacheProvider
    {
        object Get(string key);
        void Set(string key, object data, DateTimeOffset expirationTime);
        bool IsSet(string key);
        void Invalidate(string key);
    }
}
