#region Version Info
/* ========================================================================
* 【本类功能概述】
* 
* 作者：王军 时间：2013/3/19 20:50:58
* 文件名：BaseHandler
* 版本：V1.0.1
* 联系方式：511522329  
*
* 修改者： 时间： 
* 修改说明：
* ========================================================================
*/
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using HelloData.AppHandlers;
using HelloData.FrameWork;

namespace HelloData.Web.AppHandlers
{
    public abstract class BaseHandler : IAppHandler
    {
        public abstract IAppHandler CreateInstance();

        public abstract string HandlerName { get; }


        HttpResponse Response
        {
            get { return HttpContext.Response; }
        }

        private HttpRequest Request
        {
            get { return HttpContext.Request; }
        }



        public List<string> HomePageList { get; set; }

        /// <summary>
        /// 当前请求的操作handler
        /// </summary>
        /// <param name="result"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public HandlerResult CreateHandler(int result, object message)
        {
            return new HandlerResult() { Result = result, Message = message };
        }


        public List<System.Reflection.MethodInfo> ActionMethods { get; set; }



        public HttpContext HttpContext { get; set; }

    }
}
