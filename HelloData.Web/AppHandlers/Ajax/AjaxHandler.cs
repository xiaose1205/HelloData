#region Version Info
/* ========================================================================
* 【本类功能概述】
* 
* 作者：王军 时间：2013/3/19 20:52:19
* 文件名：Ajax
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
using System.Threading;
using System.Web;
using System.Web.SessionState;
using HelloData.AppHandlers;
using HelloData.FrameWork;

namespace HelloData.Web.AppHandlers
{
    /// <summary>
    /// Ajax 的摘要说明
    /// </summary>
    public class AjaxHandler : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                context.Response.ContentType = "text/plain";
                HttpRequest Request = context.Request;
                HttpResponse Response = context.Response;

                //防止数据泄露，以后可以去掉
                if (Request.RequestType.Trim().ToLower() == "get" && Request.QueryString["controller"] != "gloab" && Request.QueryString["controller"] != "checkcode")
                {
                    HandlerResponse hresult = new HandlerResponse { Result = -1, Message = "不支持GET请求" };
                    Response.Write(hresult.ToString());
                }
                else
                {
                    string handlerName = Request.Params["controller"];

                    if (string.IsNullOrEmpty(handlerName) && string.IsNullOrEmpty(Request.QueryString["controller"]))
                        return;
                    AppHandlerManager.ExecuteHandler(handlerName, HttpContext.Current, Request.QueryString["action"]);
                }
                context.ApplicationInstance.CompleteRequest();

            }
            catch (Exception ex)
            {
                if (ex is ThreadAbortException)
                { }

                //HelloData.FrameWork.Logging.Logger.CurrentLog.Error(ex.Message, ex);
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }

}
