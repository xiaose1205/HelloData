using System;
using System.Collections.Generic;
using HelloData.FWCommon.Cache; 
using ServiceStack.Redis;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using ServiceStack.Redis.Support;

namespace HelloData.FWExtend
{
    public class RedisCache:ICache
    {
        private RedisClient redisClient = new RedisClient("127.0.0.1", 6379);//
        public void Insert(string key, object value)
        {
         ObjectSerializer serializer=new ObjectSerializer();

         redisClient.Set(key, value);
        }

        public void Set(string key, object entry)
        {
            redisClient.Set(key, entry);
        }

        public T Get<T>(string key)
        {
            object obj = redisClient.Get(key);
            if (obj == null)
                return default(T);
            return (T)obj;
        }

        public List<string> GetKeys()
        {
            return redisClient.GetAllKeys();
        }

        public void RemoveAll()
        {
            List<string> keys = GetKeys(); 
            redisClient.RemoveAll(GetKeys()); 
        }

        public void Remove(string key)
        {
            redisClient.Remove(key); 
        }

        public void RemoveByPreFix(string prefix)
        {
            List<string> keys = GetKeys().FindAll(k => k.Contains(prefix));
            redisClient.RemoveAll(keys); 
        }
    }
}
