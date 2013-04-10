#region Version Info
/* ========================================================================
* 【本类功能概述】
* 
* 作者：王军 时间：2013/3/25 20:31:04
* 文件名：ActionExcute
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
using System.IO;
using System.Reflection;
using System.Web;
using System.Web.Script.Serialization;
using HelloData.AppHandlers;
using HelloData.FWCommon;
using HelloData.FWCommon.Reflection;


namespace HelloData.Web.AppHandlers
{
    public class ActionExcute
    {
        public HandlerResponse BindParamToAction(MethodInfo methodInfo, HttpContext context, IAppHandler instance)
        {
            ParameterInfo[] parameterInfos = methodInfo.GetParameters();
            /*获取ajax请求的数据*/
            StreamReader reader = new StreamReader(context.Request.InputStream);
            string bodyText = reader.ReadToEnd();
            // string bodyText = "{  \"result\":{ \"Result\":-1,\"Message\":\"不支持GET请求\",\"PostTime\":\"2012-2-2\"},\"ido\":233}";
            // string bodyText = "{ \"Result\":-1,\"Message\":\"不支持GET请求\",\"PostTime\":\"2012-2-2\",\"ido\":236}";
            if (String.IsNullOrEmpty(bodyText))
                return new HandlerResponse().GetDefaultResponse();
            /*将数据转换到字典*/
            JavaScriptSerializer jss = new JavaScriptSerializer();
            Dictionary<string, object> dictionary = jss.Deserialize<Dictionary<string, object>>(bodyText);
            object[] parameters = new object[parameterInfos.Length];
            int index = 0;
            foreach (ParameterInfo info in parameterInfos)
            {
                parameters[index] = AddValueToPamars(info.Name, info.ParameterType, dictionary);
                index++;
            }
            var invoker = FastReflectionCaches.MethodInvokerCache.Get(methodInfo);
            object result = invoker.Invoke(instance, parameters);
            return (HandlerResponse)result;
        }

        /// <summary>
        /// dictinonary又肯能有参数的名称也有可能没有
        /// </summary>
        /// <param name="pType"></param>
        /// <param name="dictionary"></param>
        /// <returns></returns>
        public object AddValueToPamars(string infoName, Type pType, Dictionary<string, object> dictionary)
        {
            PropertyInfo[] pInfos = pType.GetProperties();

            if (dictionary.ContainsKey(infoName))
            {
                if (pType.Equals(typeof(Int32))
                    || pType.Equals(typeof(Int16))
                    || pType.Equals(typeof(Int64))
                    || pType.Equals(typeof(string))
                    || pType.Equals(typeof(DateTime))
                    || pType.Equals(typeof(decimal))
                    || pType.Equals(typeof(Guid))
                    || pType.Equals(typeof(bool))
                    || pType.Equals(typeof(bool?))
                    || pType.Equals(typeof(Int32?))
                      || pType.Equals(typeof(object))
                    )
                {
                    foreach (string key in dictionary.Keys)
                    {

                        if (key == infoName)
                            return ConvertValue(pType, dictionary[key]);
                    }
                }
                else
                {
                    Object theObj = System.Activator.CreateInstance(pType);
                    foreach (var property in pInfos)
                    {
                        property.SetValue(theObj, AddValueToPamars(property.Name, property.PropertyType, dictionary), null);
                    }
                    return theObj;
                }
            }
            else
            {
                foreach (KeyValuePair<string, object> keyValuePair in dictionary)
                {

                    Dictionary<string, object> d = keyValuePair.Value as Dictionary<string, object>;
                    if (d != null)
                    {
                        if (d.ContainsKey(infoName))
                            return AddValueToPamars(infoName, pType, d);
                    }
                    Object theObj = System.Activator.CreateInstance(pType);
                    foreach (var property in pInfos)
                    {
                        property.SetValue(theObj, AddValueToPamars(property.Name, property.PropertyType, dictionary), null);
                    }
                    return theObj;
                }
            }
            return null;
        }

        public object ConvertValue(Type type, object value)
        {
            if ((type.Equals(typeof(Int32)) || type.Equals(typeof(int?))))
            {
                int intvalue = 0;
                if (int.TryParse(value.ToString(), out intvalue))
                    return intvalue;
                return intvalue;
            }
            else if ((type.Equals(typeof(bool)) || type.Equals(typeof(bool?))))
            {
                bool boolvalue;
                if (bool.TryParse(value.ToString(), out boolvalue))
                    return boolvalue;
                return boolvalue;
            }
            else if (type.Equals(typeof(Guid)))
            {
                string gudistr = value.ToString();
                value = new Guid(gudistr);
            }
            else if (type.Equals(typeof(bool)))
            {
                value = (int)value > 0;
            }
            else if (type.Equals(typeof(string)))
            {
                value = value.ToString().Trim().Replace("\0", "");
            }
            else if (type.Equals(typeof(decimal)))
            {
                value = Convert.ChangeType(value, type);
            }
            else if ((type.Equals(typeof(DateTime)) || type.Equals(typeof(DateTime?))))
            {
                DateTime dateTime = DateTime.Now;
                DateTime.TryParse(value.ToString(), out dateTime);
                value = dateTime;
            }
            else if (type.Equals(typeof(string)))
            {
                value = value.ToString().Trim().Replace("\0", "");
            }
            return value;
        }
    }
}
