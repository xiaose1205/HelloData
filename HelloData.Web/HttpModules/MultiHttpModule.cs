using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Configuration;
using HelloData.Util;
namespace HelloData.Web.HttpModules
{
    public class MultiHttpModule : System.Web.IHttpModule
    {

        /// <summary>
        /// 实现接口的Init方法
        /// </summary>
        /// <param name="context"></param>
        public void Init(HttpApplication context)
        {
            //GeneralConfigInfo si = GeneralConfigs.GetConfig();
            //if (si == null || si.UrlRewriterProvider == "asp.net")
            context.BeginRequest += ReUrl_BeginRequest;
        }
        /// <summary>
        /// 重写Url
        /// </summary>
        /// <param name="sender">事件的源</param>
        /// <param name="e">包含事件数据的 EventArgs</param>
        private void ReUrl_BeginRequest(object sender, EventArgs e)
        {
            HttpContext context = ((HttpApplication)sender).Context;

            string requestPath = context.Request.Path.ToLower();
            string lags = ConfigurationManager.AppSettings["lanague"];
            if (string.IsNullOrEmpty(lags))
                return;
            string[] lanarray = lags.Split(';');
            foreach (string s in lanarray)
            {
                if (!requestPath.Contains(s)) continue;
                string[] questings = context.Request.RawUrl.Split('?');
                string question = questings.Length > 1 ? "&" + questings[1] : string.Empty;
                string realpaht = requestPath.Replace(s + "/", "");
                if (context.Request.RawUrl.ToLower().Contains("curlan"))
                    context.RewritePath(realpaht, string.Empty, question);
                else
                    context.RewritePath(realpaht, string.Empty, "curlan=" + s + question);
                return;
            }
            if (lanarray.Length > 0)
            {
                string s = lanarray[0];
                string[] questings = context.Request.RawUrl.Split('?');
                string question = questings.Length > 1 ? "&" + questings[1] : string.Empty;
                string realpaht = requestPath.Replace(s + "/", "");
                if (context.Request.RawUrl.ToLower().Contains("curlan"))
                    context.RewritePath(realpaht, string.Empty, question);
                else
                    context.RewritePath(realpaht, string.Empty, "curlan=" + s + question);
            }

        }


        public void Dispose()
        {

        }
    }
}