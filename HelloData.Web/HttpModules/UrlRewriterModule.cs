using System;
using System.Collections;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Xml;

namespace HelloData.Web.HttpModules
{
    /// <summary>
    /// 网站HttpModule类，重写Url接口实现
    /// </summary>
    public class UrlRewriterModule : System.Web.IHttpModule
    {
        static Timer _eventTimer;

        /// <summary>
        /// 实现接口的Init方法
        /// </summary>
        /// <param name="context"></param>
        public void Init(HttpApplication context)
        {
            //GeneralConfigInfo si = GeneralConfigs.GetConfig();
            //if (si == null || si.UrlRewriterProvider == "asp.net")
            context.BeginRequest += new EventHandler(ReUrl_BeginRequest);
        }

        public void Application_OnError(Object sender, EventArgs e)
        {
            HttpApplication application = (HttpApplication)sender;
            HttpContext context = application.Context;
            //if (context.Server.GetLastError().GetBaseException() is MyException)
            {
                //MyException ex = (MyException) context.Server.GetLastError().GetBaseException();
                context.Response.Write("<html><body style=\"font-size:14px;\">");
                context.Response.Write("Error:<br />");
                context.Response.Write("<textarea name=\"errormessage\" style=\"width:80%; height:200px; word-break:break-all\">");
                context.Response.Write(System.Web.HttpUtility.HtmlEncode(context.Server.GetLastError().ToString()));
                context.Response.Write("</textarea>");
                context.Response.Write("</body></html>");
                context.Response.End();
            }

        }

        /// <summary>
        /// 实现接口的Dispose方法
        /// </summary>
        public void Dispose()
        {
            _eventTimer = null;
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
            if (requestPath == "" || requestPath == "/" || requestPath == "/default.aspx")
                return;

            foreach (SiteUrls.UrlRewrite url in SiteUrls.GetSiteUrls().Urls)
            {
                if (Regex.IsMatch(requestPath, url.Pattern, RegexOptions.IgnoreCase))
                {
                    if (url.Page != "")
                    {
                        string newQueryString = Regex.Replace(requestPath, url.Pattern, url.QueryString, RegexOptions.IgnoreCase);
                        string oldQuerystring = "";
                        if (context.Request.RawUrl.IndexOf("?", System.StringComparison.Ordinal) >= 0)
                            oldQuerystring = context.Request.RawUrl.Substring(context.Request.RawUrl.IndexOf("?") + 1);
                        if (oldQuerystring.Length > 0)
                        {
                            if (newQueryString.Length > 0)
                                newQueryString += "&" + oldQuerystring;
                            else
                                newQueryString = oldQuerystring;
                        }
                        context.RewritePath(url.Page, string.Empty, newQueryString);
                    }
                    return;
                }
            }
        }
    }

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// 站点伪Url信息类
    /// </summary>
    public class SiteUrls
    {
        #region 内部属性和方法
        private static readonly object lockHelper = new object();
        private static volatile SiteUrls _instance = null;

        readonly string _siteUrlsFile = HttpContext.Current.Server.MapPath("~/config/urls.config");
        public ArrayList Urls { get; set; }

        public NameValueCollection Paths { get; set; }

        private SiteUrls()
        {
            if (Web.Cache.CacheHelper.IsOpenCache)
            {
                Urls = Web.Cache.CacheHelper.Get<ArrayList>("rewrite");
                if (Urls == null)
                {
                    Urls = new ArrayList();
                    Paths = new NameValueCollection();
                    XmlDocument xml = new XmlDocument();
                    xml.Load(_siteUrlsFile);
                    XmlNode root = xml.SelectSingleNode("urls");
                    if (root != null)
                        foreach (XmlNode n in root.ChildNodes)
                        {
                            if (n.NodeType != XmlNodeType.Comment && n.Name.ToLower() == "rewrite")
                            {
                                XmlAttribute name = n.Attributes["name"];
                                XmlAttribute path = n.Attributes["path"];
                                XmlAttribute page = n.Attributes["page"];
                                XmlAttribute querystring = n.Attributes["querystring"];
                                XmlAttribute pattern = n.Attributes["pattern"];

                                if (name != null && path != null && page != null && querystring != null &&
                                    pattern != null)
                                {
                                    Paths.Add(name.Value, path.Value);
                                    Urls.Add(new UrlRewrite(name.Value, pattern.Value, page.Value.Replace("^", "&"),
                                                            querystring.Value.Replace("^", "&")));
                                }
                            }
                        }
                    Web.Cache.CacheHelper.Insert("rewrite", Urls);
                }
            }
            else
            {
                Urls = new ArrayList();
                Paths = new NameValueCollection();
                XmlDocument xml = new XmlDocument();
                xml.Load(_siteUrlsFile);
                XmlNode root = xml.SelectSingleNode("urls");
                if (root != null)
                    foreach (XmlNode n in root.ChildNodes)
                    {
                        if (n.NodeType != XmlNodeType.Comment && n.Name.ToLower() == "rewrite")
                        {
                            XmlAttribute name = n.Attributes["name"];
                            XmlAttribute path = n.Attributes["path"];
                            XmlAttribute page = n.Attributes["page"];
                            XmlAttribute querystring = n.Attributes["querystring"];
                            XmlAttribute pattern = n.Attributes["pattern"];

                            if (name != null && path != null && page != null && querystring != null &&
                                pattern != null)
                            {
                                Paths.Add(name.Value, path.Value);
                                Urls.Add(new UrlRewrite(name.Value, pattern.Value, page.Value.Replace("^", "&"),
                                                        querystring.Value.Replace("^", "&")));
                            }
                        }
                    }
            }
        }
        #endregion

        public static SiteUrls GetSiteUrls()
        {
            if (_instance == null)
                lock (lockHelper)
                    if (_instance == null)
                        _instance = new SiteUrls();
            return _instance;

        }

        public static void SetInstance(SiteUrls anInstance)
        {
            if (anInstance != null)
                _instance = anInstance;
        }

        public static void SetInstance()
        {
            SetInstance(new SiteUrls());
        }


        /// <summary>
        /// 重写伪地址
        /// </summary>
        public class UrlRewrite
        {
            #region 成员变量

            public string Name { get; set; }

            public string Pattern { get; set; }

            public string Page { get; set; }

            public string QueryString { get; set; }

            #endregion

            #region 构造函数
            public UrlRewrite(string name, string pattern, string page, string querystring)
            {
                Name = name;
                Pattern = pattern;
                Page = page;
                QueryString = querystring;
            }
            #endregion
        }

    }

}
