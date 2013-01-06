using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Web;
using System.Reflection;
using HelloData.FrameWork.Logging;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Web.UI.HtmlControls;
using HelloData.FrameWork.Data;

namespace HelloData.Web.Page
{
    public enum Method
    {
        All, Get, Post
    }
    public class RequestVariable
    {
        private const string CacheKeyRequestVariable = "RequestVariable";
        private NameValueCollection _mModifiedForms = null;
        private NameValueCollection _mModifiedQueryStrings = null;
        public RequestVariable(HttpContext context)
        {
            _mContext = context;
            _mRequestType = _mContext.Request.RequestType;
        }
        public static RequestVariable Current
        {
            get
            {
                if (HttpContext.Current == null)
                    return null;

                RequestVariable variable = HttpContext.Current.Items[CacheKeyRequestVariable] as RequestVariable;

                if (variable == null)
                {
                    variable = new RequestVariable(HttpContext.Current);
                    HttpContext.Current.Items[CacheKeyRequestVariable] = variable;
                }

                return variable;
            }
        }

        private string _mRequestType;

        /// <summary>
        /// 请求的类型(POST/GET)
        /// </summary>
        public string RequestType
        {
            get { return _mRequestType; }
            set { _mRequestType = value; }
        }

        /// <summary>
        /// 清除所有请求数据
        /// </summary>
        public void Clear()
        {
            Clear(Method.All);
        }
        /// <summary>
        /// 清除指定类型的请求数据
        /// </summary>
        /// <param name="method"></param>
        public void Clear(Method method)
        {
            if (method == Method.Post || method == Method.All)
            {
                if (_mModifiedForms == null)
                    _mModifiedForms = new NameValueCollection();
                else
                    _mModifiedForms.Clear();

                _mRequestType = "GET";
            }
            if (method != Method.Get && method != Method.All) return;
            if (_mModifiedQueryStrings == null)
                _mModifiedQueryStrings = new NameValueCollection();
            else
                _mModifiedQueryStrings.Clear();
        }
        public void Remove(string key, Method method)
        {
            if (method == Method.Post || method == Method.All)
            {
                if (_mModifiedForms == null)
                    _mModifiedForms = new NameValueCollection(_mContext.Request.Form);

                _mModifiedForms.Remove(key);

                if (_mModifiedForms.Count == 0)
                    _mRequestType = "GET";

            }
            if (method == Method.Get || method == Method.All)
            {
                if (_mModifiedQueryStrings == null)
                    _mModifiedQueryStrings = new NameValueCollection(_mContext.Request.QueryString);

                _mModifiedQueryStrings.Remove(key);
            }
        }

        public void Modify(string key, string value)
        {
            Modify(key, Method.All, value);
        }

        public void Modify(string key, Method method, string value)
        {
            if (method == Method.Post || method == Method.All)
            {
                if (_mModifiedForms == null)
                    _mModifiedForms = new NameValueCollection(_mContext.Request.Form);

                _mModifiedForms[key] = value;

            }
            if (method == Method.Get || method == Method.All)
            {
                if (_mModifiedQueryStrings == null)
                    _mModifiedQueryStrings = new NameValueCollection(_mContext.Request.QueryString);

                _mModifiedQueryStrings[key] = value;
            }
        }

        private readonly HttpContext _mContext;

        public HttpContext Context
        {
            get { return _mContext; }
        }

        /// <summary>
        /// 判断是否点击了指定名称的按钮
        /// </summary>
        /// <param name="buttonName"></param>
        /// <returns></returns>
        public bool IsClick(string buttonName)
        {
            return GetForm(buttonName) != null || (GetForm(buttonName + ".x") != null && GetForm(buttonName + ".y") != null);
        }

        internal string GetForm(string key)
        {
            return _mModifiedForms == null ? _mContext.Request.Form[key] : _mModifiedForms[key];
        }

        internal string GetQueryString(string key)
        {
            return _mModifiedQueryStrings == null ? _mContext.Request.QueryString[key] : _mModifiedQueryStrings[key];
        }

        /// <summary>
        /// 自动创建postform值(name值要跟实体类的属性一致)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T BindFormToObject<T>(string pre) where T : new()
        {
            T data = new T();
            Type tType = typeof(T);
            PropertyInfo[] pInfos = tType.GetProperties();
            try
            {
                SqlCompilation sqlCompilation = new SqlCompilation();
                foreach (PropertyInfo pInfo in pInfos)
                {
                    object getformValue = Get(pre + pInfo.Name);
                    if (pInfo.PropertyType == typeof(bool) || pInfo.PropertyType == typeof(bool?))
                        getformValue = getformValue.ToString() == "on";
                    object value = sqlCompilation.ConverToValue(getformValue, pInfo.PropertyType);

                    if (value != null)
                        pInfo.SetValue(data, value, null);
                }
                return data;
            }
            catch (Exception ex)
            {
                Logger.CurrentLog.Error(ex.Message, ex);
            }
            return default(T);
        }
        /// <summary>
        /// 自动创建postform值(name值要跟实体类的属性一致)
        /// </summary>
        /// <returns></returns>
        public void BindObjectToControl(string pre, Control container, object dataobj)
        {

            Type tType = dataobj.GetType();
            PropertyInfo[] pInfos = tType.GetProperties();
            try
            {
                foreach (var pInfo in pInfos)
                {
                    Control control = container.FindControl(pre + pInfo.Name);
                    object objTextBox = pInfo.GetValue(dataobj, null);
                    if (objTextBox == null) continue;
                    if (control == null) continue;
                    if (control is ListControl)
                    {
                        ListItem listItem = ((ListControl)control).Items.FindByValue(objTextBox.ToString());
                        if (listItem != null) listItem.Selected = true;
                    }
                    else if (control is HtmlSelect)
                    {
                        ListItem listItem = ((HtmlSelect)control).Items.FindByValue(objTextBox.ToString());
                        if (listItem != null) listItem.Selected = true;
                    }
                    else if (control is CheckBox)
                    {
                        if (pInfo.PropertyType == typeof(bool))
                            ((CheckBox)control).Checked = (bool)objTextBox;
                    }
                    else if (control is HtmlInputCheckBox)
                    {
                        if (pInfo.PropertyType == typeof(bool))
                            ((HtmlInputCheckBox)control).Checked = (bool)pInfo.GetValue(dataobj, null);
                    }
                    else if (control is Calendar)
                    {
                        if (pInfo.PropertyType == typeof(DateTime))
                            ((Calendar)control).SelectedDate = (DateTime)pInfo.GetValue(dataobj, null);
                    }
                    else if (control is TextBox)
                        ((TextBox)control).Text = objTextBox.ToString();
                    else if (control is HtmlInputText)
                        ((HtmlInputText)control).Value = objTextBox.ToString();
                    else if (control is HtmlLink)
                        ((HtmlLink)control).Href = objTextBox.ToString();
                    else if (control is HtmlTextArea)
                        ((HtmlTextArea)control).Value = objTextBox.ToString();
                    else if (control is HtmlInputImage)
                        ((HtmlInputImage)control).Src = objTextBox.ToString();
                    else if (control is HtmlImage)

                        ((HtmlImage)control).Src = objTextBox.ToString();
                    else if (control is Label)
                        ((Label)control).Text = objTextBox.ToString();
                    else if (control is HtmlTableCell)
                        ((HtmlTableCell)control).InnerHtml = objTextBox.ToString();
                }

            }
            catch (Exception ex)
            {
                Logger.CurrentLog.Error(ex.Message, ex);
            }

        }

        /// <summary>
        /// 获取当前请求中的表单数据，如果没有该表单项，将返回null(千万不要将返回结果不判断 null 就直接 Trim() 或者其他任何字符串操作)
        /// </summary>
        /// <param name="key">表单名</param>
        /// <returns></returns>
        public string Get(string key)
        {
            if (key != null) return Get(key, Method.All, null);
            return string.Empty;
        }

        /// <summary>
        /// 获取当前请求中的表单数据，如果没有该表单项，将返回null(千万不要将返回结果不判断 null 就直接 Trim() 或者其他任何字符串操作)
        /// </summary>
        /// <param name="key">表单名</param>
        /// <param name="method">数据提交方法</param>
        /// <returns></returns>
        public string Get(string key, Method method)
        {
            return Get(key, method, null);
        }

        /// <summary>
        /// 获取当前请求中的表单数据，如果没有该表单项，将返回传入的默认值。返回值将始终进行Html编码
        /// </summary>
        /// <param name="key">表单名</param>
        /// <param name="method">数据提交方法</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public string Get(string key, Method method, string defaultValue)
        {
            return Get(key, method, defaultValue, true);
        }


        /// <summary>
        /// 获取当前请求中的表单数据，如果没有该表单项，将返回传入的默认值
        /// </summary>
        /// <param name="key">表单名</param>
        /// <param name="method">数据提交方法</param>
        /// <param name="defaultValue">默认值</param>
        /// <param name="encodeHtml">是否对Html进行编码</param>
        /// <returns></returns>
        public string Get(string key, Method method, string defaultValue, bool encodeHtml)
        {
            string value = GetValue(key, method);
            if (value == null)
                value = defaultValue;
            return encodeHtml ? HttpUtility.HtmlEncode(value) : value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="method"></param>
        /// <returns>返回NULL时  当使用默认值</returns>
        internal string GetValue(string key, Method method)
        {
            string value = null;

            string viewState = null;
            if (method == Method.Post || method == Method.All)
            {
                viewState = GetForm(Consts.TemplateInputViewstate);
                value = GetForm(key);
            }

            if (method == Method.Get || (method == Method.All && value == null))
            {
                viewState = GetQueryString(Consts.TemplateInputViewstate);
                value = GetQueryString(key);
            }

            // if (viewState != null)
            //viewState = string.Concat("#", SecurityUtil.Base64Decode(viewState), "#");

            if (value == null
                &&
                (viewState == null || viewState.IndexOf(string.Concat("#", key, "#"), StringComparison.OrdinalIgnoreCase) == -1)
                )
            {
                return null;
            }
            else if (value == null)
            {
                return string.Empty;
            }
            else
            {
                return value;
            }
        }
    }
}
