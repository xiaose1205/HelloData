using System;
using System.Collections;
using System.Collections.Generic;


namespace HelloData.FWCommon.Cache
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
        public  static bool IsOpenCache { get; set; }

        /// <summary>
        /// 缓存对象 
        /// </summary>
        public static ICache Cache { get; set; }


        /// <summary>
        /// 使用相对过期时间
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void Insert(string key, object value)
        {
            if (Cache == null)
                return;
            Cache.Insert(key, value);
        }

        /// <summary>
        /// 获取key缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T Get<T>(string key)
        {
            if (Cache == null)
                return default(T);
           
            object obj = Cache.Get<T>(key);

            if (obj == null)
                return default(T);
            return (T)obj;
        }

        /// <summary>
        /// 获取所有缓存
        /// </summary>
        public static List<string> GetKeys()
        {
            if (Cache == null)
                return null;
            return Cache.GetKeys();
        }


        /// <summary>
        /// 清除所有缓存
        /// </summary>
        public static void RemoveAll()
        {
            if (Cache == null)
                return;
           
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
            if (Cache == null)
                return;
           
            Cache.Remove(key);
        }


        /// <summary>
        /// 根据Key的前缀删除缓存
        /// </summary>
        public static void RemoveByPreFix(string prefix)
        {
            if (Cache == null)
                return;
           
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