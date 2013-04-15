using System;
using System.Collections.Generic;
using System.Text;

namespace HelloData.FWCommon.AOP.Metadata
{
    /// <summary>
    /// 用于保存Exception相关信息
    /// </summary>
    public class ExceptionMetadata
    {
        /// <summary>
        /// 保存异常信息
        /// </summary>
        Exception _ex;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ex">初始化异常</param>
        public ExceptionMetadata(Exception ex)
        {
            _ex = ex;
        }

        /// <summary>
        /// Property：异常信息
        /// </summary>
        public virtual Exception Ex
        {
            get;
            set;
        }
    }
}
