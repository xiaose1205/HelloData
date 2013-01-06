using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Caching;

namespace HelloData.Web.Cache
{
    public static class CacheHelper
    {

        /// <summary>
        /// 设置相对过期时间，默认是30分钟
        /// </summary>
        public static double Minutes { get; set; }

        static CacheHelper()
        {
            Minutes = 30d;
            IsOpenCache = true;
        }

        /// <summary>
        /// 是否全局开启缓存
        /// </summary>
        public static bool IsOpenCache { get; set; }

        /// <summary>
        /// 缓存对象 
        /// </summary>
        public static System.Web.Caching.Cache Cache
        {
            get { return HttpRuntime.Cache; }
        }


        /// <summary>
        /// 使用相对过期时间
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void Insert(string key, object value)
        {
            if (value == null)
                return;
            Cache.Insert(key, value, null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(Minutes));
        }

        /// <summary>
        /// 设置相对过期时间
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="dependencies"></param>
        public static void Insert(string key, object value, CacheDependency dependencies)
        {
            if (value == null)
                return;
            Cache.Insert(key, value, dependencies, System.Web.Caching.Cache.NoAbsoluteExpiration,
                         TimeSpan.FromMinutes(Minutes));
        }


        /// <summary>
        /// 插入缓存，使用相对过期时间
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="dependency"></param>
        /// <param name="priority"></param>
        /// <param name="callback"></param>
        public static void Insert(string key, object value, CacheDependency dependency, CacheItemPriority priority,
                                  CacheItemRemovedCallback callback)
        {
            if (value == null)
            {
                return;
            }

            Cache.Insert(key, value, dependency, System.Web.Caching.Cache.NoAbsoluteExpiration,
                         TimeSpan.FromMinutes(Minutes), priority, callback);
        }


        /// <summary>
        /// 获取key缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T Get<T>(string key)
        {
            if (!IsOpenCache) return default(T);
            object obj = Cache.Get(key);

            if (obj == null)
                return default(T);
            return (T)obj;
        }

        /// <summary>
        /// 获取所有缓存
        /// </summary>
        public static List<string> GetKeys()
        {

            List<string> keys = new List<string>();
            if (!IsOpenCache) return keys;
            IDictionaryEnumerator e = Cache.GetEnumerator();
            while (e.MoveNext())
            {
                keys.Add(e.Key.ToString());
            }
            return keys;

        }


        /// <summary>
        /// 清除所有缓存
        /// </summary>
        public static void RemoveAll()
        {
            if (!IsOpenCache) return;
            List<string> keys = GetKeys();
            if (keys.Count > 0)
            {
                foreach (string item in keys)
                {
                    Cache.Remove(item);
                }
            }
        }

        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="key"></param>
        public static void Remove(string key)
        {
            if (!IsOpenCache) return;
            Cache.Remove(key);
        }


        /// <summary>
        /// 根据Key的前缀删除缓存
        /// </summary>
        public static void RemoveByPreFix(string prefix)
        {
            if (!IsOpenCache) return;
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
