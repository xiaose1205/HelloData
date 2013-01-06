using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Web;

namespace HelloData.Web
{

    /// <summary>
    /// 消息显示器，在web项目中用来收集需要显示的错误信息或者成功信息，并统一显示
    /// </summary>
    public class MessageDisplay
    {

        #region MessageItem 和 MessageCollection

        public class MessageItem
        {
            public string Name { get; set; }
            public int Index { get; set; }
            public string Message { get; set; }
        }

        public class MessageCollection : Collection<MessageItem>
        {
            public void Add(string name, int index, string message)
            {
                MessageItem item = new MessageItem();
                item.Name = name;
                item.Index = index;
                item.Message = message;
                this.Add(item);
            }

            public MessageItem GetFirst(string name)
            {
                foreach (MessageItem item in this)
                {
                    if (string.Compare(item.Name, name, true) == 0)
                        return item;
                }
                return null;
            }

            public MessageItem GetFirst(int index)
            {
                foreach (MessageItem item in this)
                {
                    if (item.Index == index)
                        return item;
                }
                return null;
            }

            public MessageItem GetFirst(string name, int index)
            {
                foreach (MessageItem item in this)
                {
                    if (string.Compare(item.Name, name, true) == 0 && item.Index == index)
                        return item;
                }
                return null;
            }

            public MessageCollection GetAll(string name)
            {
                MessageCollection all = new MessageCollection();
                foreach (MessageItem item in this)
                {
                    if (string.Compare(item.Name, name, true) == 0)
                        all.Add(item);
                }
                return all;
            }

            public MessageCollection GetAll(int index)
            {
                MessageCollection all = new MessageCollection();
                foreach (MessageItem item in this)
                {
                    if (item.Index == index)
                        all.Add(item);
                }
                return all;
            }

            public MessageCollection GetAll(string name, int index)
            {
                MessageCollection all = new MessageCollection();
                foreach (MessageItem item in this)
                {
                    if (string.Compare(item.Name, name, true) == 0 && item.Index == index)
                        all.Add(item);
                }
                return all;
            }

            public MessageItem GetLast(string name)
            {
                MessageCollection all = GetAll(name);
                if (all.Count > 0)
                    return all[all.Count - 1];

                return null;
            }

            public MessageItem GetLast(int index)
            {
                MessageCollection all = GetAll(index);
                if (all.Count > 0)
                    return all[all.Count - 1];

                return null;
            }

            public MessageItem GetLast(string name, int index)
            {
                MessageCollection all = GetAll(name, index);
                if (all.Count > 0)
                    return all[all.Count - 1];

                return null;
            }
        }

        #endregion

        private const string CacheKeyCurrentMessageDisplay = "MessageDisplay-Current-{0}";
        internal const string KeyUnnamedTarget = "_maxtarget_";
        internal const string KeyDefaultForm = "_maxform_";

        private string _mForm = null;
        private MessageCollection m_Errors = new MessageCollection();
        private string[] _mNamedErrors = null;

        public MessageDisplay(string form, string[] names)
        {
            if (string.IsNullOrEmpty(form))
                form = KeyDefaultForm;

            _mForm = form;
            DeclareNamedErrors(names);

            string cachekey = string.Format(CacheKeyCurrentMessageDisplay, form);
            HttpContext.Current.Items[cachekey] = this;
        }

        public string Form { get { return _mForm; } set { _mForm = value; } }

        public void DeclareNamedErrors(params string[] names)
        {
            _mNamedErrors = names;
        }

        #region AddError

        public void AddError(ErrorInfo error)
        {
            AddError(error.TatgetName, error.TargetLine, error.Message);
        }
         
        public void AddError(string name, int index, string message)
        {

            if (string.IsNullOrEmpty(name))
                name = KeyUnnamedTarget;

            bool named = false;
            if (_mNamedErrors != null)
            {
                foreach (string namedError in _mNamedErrors)
                {
                    if (string.Compare(name, namedError, true) == 0)
                    {
                        named = true;
                        break;
                    }
                }
            }
            m_Errors.Add(named ? name : KeyUnnamedTarget, index, message);
        }

        public void AddError(string name, string message)
        {
            AddError(name, -1, message);
        }

        public void AddError(string message)
        {
            AddError(null, -1, message);
        }

        public void AddException(Exception exception)
        {
            // LogManager.LogException(exception);
            AddError(exception.Message);
        }

        #endregion

        #region GetError

        //获取指定的错误中的第一条错误
        public MessageItem GetFirstError(string name)
        {
            return m_Errors.GetFirst(name);
        }

        public MessageItem GetFirstError(int index)
        {
            return m_Errors.GetFirst(index);
        }

        public MessageItem GetFirstError(string name, int index)
        {
            return m_Errors.GetFirst(name, index);
        }

        public MessageItem GetFirstUnnamedError()
        {
            return m_Errors.GetFirst(KeyUnnamedTarget);
        }

        //获取指定的错误中的最后一条错误

        public MessageItem GetLastError(string name)
        {
            return m_Errors.GetLast(name);
        }

        public MessageItem GetLastError(int index)
        {
            return m_Errors.GetLast(index);
        }

        public MessageItem GetLastError(string name, int index)
        {
            return m_Errors.GetLast(name, index);
        }

        public MessageItem GetLastUnnamedError()
        {
            return m_Errors.GetLast(KeyUnnamedTarget);
        }

        //获取指定的错误，如果包含多个错误，将使用,分割

        public MessageCollection GetErrors(string name)
        {
            return m_Errors.GetAll(name);
        }

        public MessageCollection GetErrors(int index)
        {
            return m_Errors.GetAll(index);
        }

        public MessageCollection GetErrors(string name, int index)
        {
            return m_Errors.GetAll(name, index);
        }

        public MessageCollection GetUnnamedErrors()
        {
            return m_Errors.GetAll(KeyUnnamedTarget);
        }

        public MessageCollection GetAllErrors()
        {
            return m_Errors;
        }

        #endregion

        #region HasError

        //获取是否发生了指定的错误
        public bool HasError(object name)
        {
            return (m_Errors.GetFirst(name.ToString()) != null);
        }

        public bool HasUnnamedError()
        {
            return (m_Errors.GetFirst(KeyUnnamedTarget) != null);
        }

        public bool HasAnyError()
        {
            return m_Errors.Count > 0;
        }

        #endregion 

        #region GetFrom

        public static MessageDisplay GetFrom(string form)
        {
            if (string.IsNullOrEmpty(form))
                form = KeyDefaultForm;

            string cachekey = string.Format(CacheKeyCurrentMessageDisplay, form);
            return HttpContext.Current.Items[cachekey] as MessageDisplay;
        }

        public static MessageDisplay GetFrom()
        {
            return GetFrom(null);
        }

        #endregion

    }
}
