using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Caching;
using System.Collections;
namespace HelloData.FrameWork.Cache
{
    /// <summary>
    /// 利用web.caching使用cache
    /// </summary>
    public class WebCache : ICache
    {
        /// <summary>
        /// 缓存对象 
        /// </summary>
        public System.Web.Caching.Cache Cache
        {
            get { return HttpRuntime.Cache; }
        }

        public void Insert(string key, object value)
        {
            Cache.Insert(key, value, null, System.Web.Caching.Cache.NoAbsoluteExpiration,
                                     TimeSpan.FromMinutes(CacheHelper.Minutes));
        }

        public void Set(string key, object entry)
        {
            Cache.Insert(key, entry, null, System.Web.Caching.Cache.NoAbsoluteExpiration,
                                    TimeSpan.FromMinutes(CacheHelper.Minutes));
        }

        public T Get<T>(string key)
        {
            object obj = Cache.Get(key);
            if (obj == null)
                return default(T);
            return (T)obj;
        }

        public List<string> GetKeys()
        {
            List<string> keys = new List<string>();

            IDictionaryEnumerator e = Cache.GetEnumerator();
            while (e.MoveNext())
            {
                keys.Add(e.Key.ToString());
            }
            return keys;
        }

        public void RemoveAll()
        {

            List<string> keys = GetKeys();
            if (keys.Count > 0)
            {
                foreach (string item in keys)
                {
                    Cache.Remove(item);
                }
            }
        }

        public void Remove(string key)
        {
            Cache.Remove(key);
        }

        public void RemoveByPreFix(string prefix)
        {

            List<string> keys = GetKeys().FindAll(k => k.Contains(prefix));
            if (keys.Count > 0)
            {
                foreach (var item in keys)
                {
                    Cache.Remove(item);
                }
            }
        }
    }
}
