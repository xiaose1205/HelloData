using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HelloData.FrameWork.Cache
{
    /// <summary>
    /// 缓存的接口
    /// </summary>
    public interface ICache
    {
 
        /// <summary>
        /// 插入缓存，使用相对过期时间
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void Insert(string key, object value);
        /// <summary>
        /// 设置key的缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="entry"></param>
        void Set(string key, object entry);
        /// <summary>
        /// 获取key缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        T Get<T>(string key);
        /// <summary>
        /// 获取所有缓存
        /// </summary>
        List<string> GetKeys();

        /// <summary>
        /// 清除所有缓存
        /// </summary>
        void RemoveAll();
        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="key"></param>
        void Remove(string key);
        /// <summary>
        /// 根据Key的前缀删除缓存
        /// </summary>
        void RemoveByPreFix(string prefix);
    }
    [Serializable]
    public class CacheItem
    {
        public DateTime ExpiryDate;
        public object Item;

        public CacheItem(object entry, DateTime utcExpiry)
        {
            Item = entry;
            ExpiryDate = utcExpiry;
        }
    }
}
