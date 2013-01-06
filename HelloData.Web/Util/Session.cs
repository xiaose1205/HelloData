
using System;
using System.Web;
using System.Text;

namespace HelloData.Util
{
    /// <summary>
    /// Session操作类
    /// </summary>
    public static class Session
    {
      
        public static void SetSession(string strSessionName, object strValue)
        {
            HttpContext.Current.Session[strSessionName] = strValue;         
        }

        /// <summary>
        /// 添加Session
        /// </summary>
        /// <param name="strSessionName">Session对象名称</param>
        /// <param name="strValue">Session值</param>
        /// <param name="iExpires">调动有效期（分钟）</param>
        public static void SetSession(string strSessionName, string strValue, int iExpires)
        {
            HttpContext.Current.Session[strSessionName] = strValue;
            HttpContext.Current.Session.Timeout = iExpires;
        }
      
        /// <summary>
        /// 读取某个Session对象值
        /// </summary>
        /// <param name="strSessionName">Session对象名称</param>
        /// <returns>Session对象值</returns>
        public static object GetSession(string strSessionName)
        {
            if (HttpContext.Current.Session[strSessionName] == null)
            {
                return null;
            }
            else
            {
                return HttpContext.Current.Session[strSessionName];
            }
        }

        /// <summary>
        /// 删除某个Session对象
        /// </summary>
        /// <param name="strSessionName">Session对象名称</param>
        public static void DelSession(string strSessionName)
        {
            HttpContext.Current.Session[strSessionName] = null;
        }
    }
    /// <summary>
    /// Application操作类
    /// </summary>
    public static class Application
    {

        public static void SetApplication(string strApplicationName, object strValue)
        {
            HttpContext.Current.Application[strApplicationName] = strValue;
        }

        /// <summary>
        /// 添加Application
        /// </summary>
        /// <param name="strApplicationName">Application对象名称</param>
        /// <param name="strValue">Application值</param>
        /// <param name="iExpires">调动有效期（分钟）</param>
        public static void SetApplication(string strApplicationName, string strValue, int iExpires)
        {
            HttpContext.Current.Application[strApplicationName] = strValue; 
        }

        /// <summary>
        /// 读取某个Application对象值
        /// </summary>
        /// <param name="strApplicationName">Application对象名称</param>
        /// <returns>Application对象值</returns>
        public static object GetApplication(string strApplicationName)
        {
            if (HttpContext.Current.Application[strApplicationName] == null)
            {
                return null;
            }
            else
            {
                return HttpContext.Current.Application[strApplicationName];
            }
        }

        /// <summary>
        /// 删除某个Application对象
        /// </summary>
        /// <param name="strApplicationName">Application对象名称</param>
        public static void DelApplication(string strApplicationName)
        {
            HttpContext.Current.Application[strApplicationName] = null;
        }
    }
}
