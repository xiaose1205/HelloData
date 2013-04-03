using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HelloData.FrameWork.Data
{
    /// <summary>
    /// 全局唯一实例化，并自动创建部分代码，最主要是重写下BaseLogic里面的CreateOtherParms,如：CreateOtherParms(),后在调用base.save();
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TU"></typeparam>
    public class BaseManager<T, TU> : BaseLogic<TU>, IDisposable
        where T : BaseManager<T, TU>, new()
        where TU : BaseEntity, new()
    {

        private static T _sInstance = null;
        /// <summary>
        /// 获取当前业务逻辑对象的实例
        /// </summary>
        public static T Instance
        {
            get { return _sInstance ?? (_sInstance = new T()); }
        }
        public void SetBaseEntity()
        {
            base.Entity = new TU();
        }

        /// <summary>
        /// 创建结果
        /// </summary>
        /// <param name="resultCode"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public string CreateResult(int resultCode, object message)
        {
            HandlerResponse result = new HandlerResponse { Result = resultCode, Message = message };
            return result.ToString();
        }


        public void Dispose()
        {

        }
    }
    /// <summary>
    /// logic的生成类，唯一实例。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseManager<T> : IDisposable
        where T : BaseManager<T>, new()
    {

        private static T _sInstance = null;
        /// <summary>
        /// 获取当前业务逻辑对象的实例
        /// </summary>
        public static T Instance
        {
            get { return _sInstance ?? (_sInstance = new T()); }
        }
        /// <summary>
        /// 创建结果
        /// </summary>
        /// <param name="resultCode"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public string CreateResult(int resultCode, object message)
        {
            HandlerResponse result = new HandlerResponse { Result = resultCode, Message = message };
            return result.ToString();
        }


        public void Dispose()
        {

        }
    }
}
