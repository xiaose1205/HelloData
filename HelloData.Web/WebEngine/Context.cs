using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HelloData.Web 
{
    /// <summary>
    /// WebEngine的请求上下文信息
    /// </summary>
    public class Context
    {
        public Context()
        {
            //RawAbsolutePath = HttpContext.Current.Request.Url.AbsolutePath;
        }

        public static void Init()
        {
            System.Threading.Thread.BeginThreadAffinity();

            s_Current = new Context();
            //HttpContext.Current.Items[Consts.CurrentContext] = new Context();
        }

        [ThreadStatic]
        private static Context s_Current;

        /// <summary>
        /// 获取当前的请求上下文
        /// </summary>
        public static Context Current
        {
            get
            {
                if (s_Current == null)
                    s_Current = new Context();
                return s_Current;// (Context)HttpContext.Current.Items[Consts.CurrentContext];
            }
        }

        private ErrorInfoCollection m_Errors;

        /// <summary>
        /// 获取当前请求上下文中的所有错误信息
        /// </summary>
        public ErrorInfoCollection Errors
        {
            get
            {
                if (m_Errors == null)
                    m_Errors = new ErrorInfoCollection();

                return m_Errors;
            }
        }

        private void ThrowErrorInner<TError>(TError error) where TError : ErrorInfo
        {
            error.Catched = false;
            this.Errors.Add(error);
        }

        /// <summary>
        /// 抛出错误信息（不会中止当前线程继续执行）
        /// </summary>
        /// <typeparam name="TError">错误信息类型</typeparam>
        /// <param name="error">错误信息</param>
        public static void ThrowError<TError>(TError error) where TError : ErrorInfo
        {
            if (Current != null)
                Current.ThrowErrorInner(error);
        }
        
    }
}
