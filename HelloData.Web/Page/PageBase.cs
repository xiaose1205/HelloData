using System;
using HelloData.FWCommon.Utils;
using HelloData.Web.Settings;
using HelloData.AppHandlers;
using HelloData.FrameWork.Logging; 
using System.Threading;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using HelloData.FrameWork;

namespace HelloData.Web.Page
{
    /// <summary>
    /// 所有页面的基类
    /// </summary>
    public class PageBase : System.Web.UI.Page
    {
        protected override void OnError(EventArgs e)
        {
            Exception ex = Server.GetLastError();
            Logger.CurrentLog.Error(Page.Title, ex);
            Server.ClearError();
            Response.Redirect("~/error.aspx?error=" + ex.Message.Replace("\r\n", "") + "");

        }
        /// <summary>
        /// 当前页的语言
        /// </summary>
        private bool Islanuage = false;
        protected override void InitializeCulture()
        {
            //this.Context.Profile.SetPropertyValue();
            string lanague = Request.QueryString["curlan"];
            object LanguagePreference = Session["mulitlanguage"];
            //该用户首次访问本站，Profile.LanguagePreference为空时，识别用户浏览器的语言设置
            if (LanguagePreference != null || !string.IsNullOrEmpty(lanague))
            {
                Islanuage = true;
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(lanague);
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(lanague);
            }
            else
            {
                base.InitializeCulture();
            }
        }
        /// <summary>
        /// 是否需要登录
        /// </summary>
        public virtual bool IsNeedLogin { get; set; }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (Islanuage)
            {
                try
                {
                    BindLanguageToControl(this);

                }
                catch (Exception)
                {
                    Server.ClearError();
                }
            }
            CheckVisit();
            CheckLogin();
            ShieldSpider();
            this.Page.Title = GetBasePageTitle();
        }
        void BindLanguageToControl(Control controls)
        {
            foreach (Control control in controls.Controls)
            {
                if (control is WebControl)
                {
                    if (control is Button)
                    {
                        ((Button)control).Text =
                            LanguageOut("" + control.ID + ".text".ToLower());
                    }
                    else if (control is HyperLink)
                    {
                        ((HyperLink)control).Text =
                            LanguageOut("" + control.ID + ".text".ToLower());
                    }
                    else if (control is Label)
                    {
                        ((Label)control).Text =
                            LanguageOut("" + control.ID + ".text".ToLower());
                    }
                }
                else if (control is HtmlControl)
                {
                    //匹配所有的基于Html原始组件加runset=server的情况
                    if (control is HtmlForm||control is HtmlHead)
                    {

                    }
                    else if (control is HtmlContainerControl)
                    {
                        ((HtmlContainerControl)control).InnerText =
                            LanguageOut("" + control.ID + ".text".ToLower());
                    }
                    else if (control is HtmlInputControl)
                    {
                        ((HtmlInputControl)control).Value =
                            LanguageOut("" + control.ID + ".text".ToLower());
                    }

                }
                BindLanguageToControl(control);
            }

        }
        /// <summary>
        /// 获取当前资源文件的值
        /// </summary>
        /// <param name="resouceName"></param>
        /// <returns></returns>
        public string LanguageOut(string resouceName)
        {
            object obj = GetLocalResourceObject(resouceName);
            if (obj != null)
                return obj.ToString();
            return string.Empty;
        }

        /// <summary>
        /// 获取当前全局资源文件的值
        /// </summary>
        /// <param name="fileName"> </param>
        /// <param name="resouceName"></param>
        /// <returns></returns>
        public string GLoabLanguageOut(string fileName, string resouceName)
        {
            object obj = GetGlobalResourceObject(fileName, resouceName);
            if (obj != null)
                return obj.ToString();
            return string.Empty;
        }
        private void CheckVisit()
        {
        }
        private void CheckLogin()
        {

        }
        public MessageDisplay Display = null;
        /// <summary>
        /// 在当前界面上面显示错误的信息，错误信息的id必须为：info_msg
        /// </summary>
        /// <param name="names"></param>
        public void DisplayMsg(params string[] names)
        {
            Display = new MessageDisplay(null, names);
        }
        /// <summary>
        /// 处理搜索引擎
        /// </summary>
        private void ShieldSpider()
        {
        }
        private RequestVariable _mRequest;

        public RequestVariable _Request
        {
            get
            {
                if (_mRequest == null)
                    _mRequest = RequestVariable.Current;

                return _mRequest;
            }
        }
        protected virtual string InfoPageSrc
        {
            get
            {
                return string.Empty;
            }
        }

        protected void AlertMessage(string message)
        {
            ShowInfo(message, AlertIconInfo.info, null);
        }
        protected void AlertMessage(string message, AlertIconInfo icon)
        {
            ShowInfo(message, icon, null);
        }
        protected void AlertMessage(string message, AlertIconInfo icon, string AfterScript)
        {
            ShowInfo(message, icon, AfterScript);
        }
        /// <summary>
        /// 具体操作的对话框
        /// </summary>
        internal protected void ShowInfo(string message, AlertIconInfo mode, string AfterScript)
        {
            string icon = "alert";
            if (string.IsNullOrEmpty(message))
                switch (mode)
                {
                    case AlertIconInfo.success:
                        message = "当前操作成功";
                        icon = "succeed";
                        break;
                    case AlertIconInfo.error:
                        message = "当前操作操作错误";
                        icon = "error";
                        break;
                    case AlertIconInfo.waring:
                        break;
                    default:
                        break;
                }
            else
                switch (mode)
                {
                    case AlertIconInfo.success:
                        icon = "succeed";
                        break;
                    case AlertIconInfo.error:
                        icon = "error";
                        break;
                    case AlertIconInfo.waring:
                        break;
                    default:
                        break;
                }
            string content = message;
            message = StringPlus.HtmlEncode(message);
            ClientScript.RegisterStartupScript(this.GetType(), "", @"<script>art.dialog({
                icon: '" + icon + "',  content: '" + content + "',ok:function(){ " + (string.IsNullOrEmpty(AfterScript) ? "return true;" : AfterScript) + "}  });</script>");
        }

        protected void MessageShow()
        {
            ReturnMessageShow(AlertIconInfo.success, string.Empty, null, false, null, 0, false, null);
        }

        protected void MessageShow(ErrorInfo error)
        {
            ReturnMessageShow(AlertIconInfo.error, error.Message, string.Empty, error.IsHtmlEncodeMsg, null, 0, false, null);
        }


        protected void MessageShow(ErrorInfo error, string title)
        {
            ReturnMessageShow(AlertIconInfo.error, error.Message, title, error.IsHtmlEncodeMsg, null, 0, false, null);
        }
        protected void MessageShow(ErrorInfo error, string title, string returnUrl, int autoJumpSeconds)
        {
            ReturnMessageShow(AlertIconInfo.error, error.Message, title, error.IsHtmlEncodeMsg, returnUrl, autoJumpSeconds, false, null);
        }

        protected void MessageShow(ErrorInfo error, string title, string returnUrl)
        {
            ReturnMessageShow(AlertIconInfo.error, error.Message, title, error.IsHtmlEncodeMsg, returnUrl, 0, false, null);
        }

        protected void MessageShow(string message)
        {
            MessageShow(message, true);
        }

        protected void MessageShow(string message, string title, string returnUrl, bool tipLogin)
        {
            ReturnMessageShow(AlertIconInfo.error, message, title, false, returnUrl, 0, tipLogin, null);
        }
        protected void MessageShow(string message, string title, bool htmlEncode)
        {
            ReturnMessageShow(AlertIconInfo.error, message, title, htmlEncode, null, 0, false, null);
        }

        protected void MessageShow(bool htmlEncodeMessage, string message)
        {
            ReturnMessageShow(AlertIconInfo.success, message, string.Empty, htmlEncodeMessage, null, 0, false, null);
        }

        protected void MessageShow(string message, object returnValue)
        {
            ReturnMessageShow(AlertIconInfo.success, message, string.Empty, true, null, 2, false, returnValue);
        }
        protected void MessageShow(string message, string returnUrl)
        {
            ReturnMessageShow(AlertIconInfo.success, message, string.Empty, true, returnUrl, 2, false, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="message"></param>
        /// <param name="msgtitle"></param>
        /// <param name="htmlEncodeMessage"></param>
        /// <param name="returnUrls"></param>
        /// <param name="autoJumpSeconds"></param>
        /// <param name="tipLogin"></param>
        /// <param name="returnValue"></param>
        internal protected void ReturnMessageShow(
            AlertIconInfo mode,
            string message,
            string msgtitle,
            bool htmlEncodeMessage,
            string returnUrls,
            int autoJumpSeconds,
            bool tipLogin,
            object returnValue)
        {
            var icon = "alert";
            if (string.IsNullOrEmpty(message))
                switch (mode)
                {
                    case AlertIconInfo.success:
                        message = "当前操作成功";
                        icon = "success";
                        break;
                    case AlertIconInfo.error:
                        message = "当前操作发送错误";
                        icon = "error";
                        break;
                    case AlertIconInfo.waring:
                        break;
                }
            else
                switch (mode)
                {
                    case AlertIconInfo.success:
                        icon = "success";
                        break;
                    case AlertIconInfo.error:
                        icon = "error";
                        break;
                    case AlertIconInfo.waring:
                        break;
                }
            Session["title"] = string.IsNullOrEmpty(Title) ? "网站温馨提示" : Title;
            if (htmlEncodeMessage)
                message = StringPlus.HtmlEncode(message);
            Session["content"] = message;
            if (!string.IsNullOrEmpty(returnUrls))
                Session["content"] = "<a href='" + returnUrls + "'>" + message + "（如果没有自动跳转，请点击）</a>";

            this.Context.Response.Redirect("MessageShow.aspx?icon=" + icon + "");
        }

        /// <summary>
        /// 浏览器标题栏
        /// </summary>
        protected virtual string PageTitle
        {
            get
            {
                return string.Empty;
            }
        }
        protected string GetBasePageTitle()
        {
            string attach = "Diougens管理系统";
            if (string.IsNullOrEmpty(PageTitle))
            {
                if (string.IsNullOrEmpty(PageTitleAttach) == false)
                    return string.Concat(WebName, " - ", PageTitleAttach,MetaDescription + attach);

                return WebName + MetaDescription + attach;
            }
            return PageTitle + MetaDescription + attach;
        }
        /// <summary>
        /// 网站名称
        /// </summary>
        protected string WebName
        {
            get
            {

                return SiteSettings.WebName;
            }
        }
        /// <summary>
        /// 浏览器标题栏附加的文字
        /// </summary>
        protected virtual string PageTitleAttach
        {
            get { return SiteSettings.TitleAttach; }
        }

        /// <summary>
        /// meta中的关键字
        /// </summary>
        protected virtual string MetaKeywords
        {
            get { return SiteSettings.MetaKeywords; }
        }

        /// <summary>
        /// meta中的简介
        /// </summary>
        protected virtual string MetaDescription
        {
            get { return SiteSettings.MetaDescription; }
        }

        /// <summary>
        /// 是否需要输出base64js 通常在需要加密链接的URL的页面 需要输出
        /// </summary>
        protected virtual bool IncludeBase64Js
        {
            get
            {
                return false;
            }
        }
        /// <summary>
        /// 当前35ToGroup程序的版本
        /// </summary>
        protected string Version
        {
            get { return Globals.Version; }
        }
        /// <summary>
        /// 创建结果
        /// </summary>
        /// <param name="resultCode"></param>
        /// <param name="Message"></param>
        /// <returns></returns>
        public string CreateResult(int resultCode, object Message)
        {
            HandlerResult result = new HandlerResult();
            result.Result = resultCode;
            result.Message = Message;
            return result.ToString();
        }
        /// <summary>
        /// 比较obj与obj1,返回msg或者msg1
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="obj1"></param>
        /// <param name="msg"></param>
        /// <param name="msg1"></param>
        public void WriteResponse(object obj, object obj1, string msg, string msg1)
        {
            if (!obj.Equals(obj1))
            {
                Response.Write(CreateResult(0, msg1));
            }
            else
                Response.Write(CreateResult(1, msg));
        }
        /// <summary>
        /// 默认与0比较
        /// </summary>
        /// <param name="result"></param>
        /// <param name="msg"></param>
        public void WriteResponse(int result, string msg, string msg1)
        {
            if (result > 0)
            {
                Response.Write(CreateResult(1, msg));
            }
            else
                Response.Write(CreateResult(0, msg1));
        }
        public void WriteResponse(string msg)
        {
            Response.Write(msg);
        }
    }
    public enum AlertIconInfo
    {
        /// <summary>
        /// 成功
        /// </summary>
        success,
        /// <summary>
        /// 失败
        /// </summary>
        error,
        /// <summary>
        /// 警告
        /// </summary>
        waring,
        /// <summary>
        /// 信息
        /// </summary>
        info
    }

}
