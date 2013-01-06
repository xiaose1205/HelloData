 
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace HelloData.AppHandlers
{
    public interface IAppHandler
    {
        /// <summary>
        /// 唯一实例
        /// </summary>
        /// <returns></returns>
        IAppHandler CreateInstance();

        /// <summary>
        /// 请求handler的名称，每个handler尽量不要一样
        /// </summary>
        string HandlerName { get; }
        /// <summary>
        /// 获取的请求处理
        /// </summary>
        /// <param name="context"></param>
        void ProcessRequest(HttpContext context);

        /// <summary>
        /// 当前方法支持的所在的页面，防止恶意操作
        /// </summary>
        List<string> HomePageList { get; set; }

    }
}