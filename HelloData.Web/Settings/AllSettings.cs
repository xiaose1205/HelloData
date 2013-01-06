using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HelloData.Web.Settings
{
    /// <summary>
    /// 系统中所有的设置
    /// </summary>
    public class AllSettings
    {
        /// <summary>
        /// 当前程序所有设置
        /// </summary>
        public static AllSettings Current
        {
            get;
            internal set;
        }
    }
}
