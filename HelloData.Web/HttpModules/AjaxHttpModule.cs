#region Version Info
/* ========================================================================
* 【本类功能概述】
* 
* 作者：王军 时间：2013/3/25 20:27:33
* 文件名：AjaxHttpModule
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

namespace HelloData.Web.HttpModules
{
    public class AjaxHttpModule : IHttpModule
    {
        public void Dispose()
        {

        }

        public void Init(HttpApplication context)
        {
            context.BeginRequest += AjaxUrl_BeginRequest;

        }

        private void AjaxUrl_BeginRequest(object sender, EventArgs e)
        {
            HttpContext context = ((HttpApplication)sender).Context;
            string requestPath = context.Request.Path.Trim('/').ToLower();
            if (requestPath.Contains("ajax/"))
            {//符合ajax请求的内容

                string[] controls = requestPath.Substring(0, requestPath.IndexOf('?')).Split('/');
                context.Response.ContentType = "text/plain";
                HttpRequest Request = context.Request;
                HttpResponse Response = context.Response;

                //防止数据泄露，以后可以去掉
                if (Request.RequestType.Trim().ToLower() == "get" && controls[1] != "gloab" &&
                   controls[1] != "checkcode")
                {
                    HandlerResponse hresult = new HandlerResponse { Result = -1, Message = "不支持GET请求" };
                    Response.Write(hresult.ToString());
                }
                else
                {
                    /*control的操作对象*/
                    string handlerName = controls[1];

                    if (string.IsNullOrEmpty(handlerName))
                        return;
                    AppHandlerManager.ExecuteHandler(handlerName, HttpContext.Current, controls[2]);
                }
                context.ApplicationInstance.CompleteRequest();
            }
        }

    }
}
