using System;
using System.Configuration;
using System.Reflection;
using System.Web;
using System.Xml;
namespace HelloData.Util
{
    /// <summary>
    /// webconfig 文件操作类
    /// </summary>
    public enum ConfigFileType
    {
        /// <summary>
        /// 
        /// </summary>
        WebConfig,
        /// <summary>
        /// 
        /// </summary>
        custom,
        /// <summary>
        /// 
        /// </summary>
        AppConfig
    }

    /// <summary>
    /// 
    /// </summary>
    public class ReadWriteConfig
    {
        public string docName = String.Empty;
        private XmlNode node = null;
        private int _configType;
        private string _ConfigPath;
        public string message;
        public int ConfigType
        {
            get { return _configType; }
            set { _configType = value; }
        }
        public string ConfigPath
        {
            get { return _ConfigPath; }
            set { _ConfigPath = value; }
        }

        #region GetValue
        public string GetValue(string key)
        {   //读取
            try
            {
                XmlDocument cfgDoc = new XmlDocument();
                loadConfigDoc(cfgDoc);
                // retrieve the appSettings node  
                node = cfgDoc.SelectSingleNode("//appSettings");
                if (node == null)
                {
                    throw new InvalidOperationException("appSettings section not found");
                }
                // XPath select setting "add" element that contains this key to remove      
                XmlElement addElem = (XmlElement)node.SelectSingleNode("//add[@key='" + key + "']");
                //XmlElement addElem = (XmlElement)node;
                if (addElem == null)
                {
                    message = "此key不存在！";
                    return message;
                }
                message = System.Configuration.ConfigurationManager.AppSettings[key];
                //message = addElem.GetAttribute(key).ToString();
                return message;
            }
            catch
            {
                message = "操作异常，获取value值失败！";
                return message;
            }
        }
        #endregion

        #region SetValue
        public string SetValue(string key, string value)
        {  //增加
            XmlDocument cfgDoc = new XmlDocument();
            loadConfigDoc(cfgDoc);
            // retrieve the appSettings node   
            node = cfgDoc.SelectSingleNode("//appSettings");
            if (node == null)
            {
                throw new InvalidOperationException("appSettings section not found");
            }
            try
            {
                // XPath select setting "add" element that contains this key       
                XmlElement addElem = (XmlElement)node.SelectSingleNode("//add[@key='" + key + "']");
                if (addElem != null)
                {
                    message = "此key已经存在！";
                    return message;
                }
                // not found, so we need to add the element, key and value   
                else
                {
                    XmlElement entry = cfgDoc.CreateElement("add");
                    entry.SetAttribute("key", key);
                    entry.SetAttribute("value", value);
                    node.AppendChild(entry);
                }
                //save it   
                saveConfigDoc(cfgDoc, docName);
                message = "添加成功！";
                return message;
            }
            catch
            {
                message = "操作异常，添加失败！";
                return message;
            }
        }

        #endregion

        #region saveConfigDoc
        private void saveConfigDoc(XmlDocument cfgDoc, string cfgDocPath)
        {
            try
            {
                XmlTextWriter writer = new XmlTextWriter(cfgDocPath, null);
                writer.Formatting = Formatting.Indented;
                cfgDoc.WriteTo(writer);
                writer.Flush();
                writer.Close();
                return;
            }
            catch
            {
                throw;
            }
        }

        #endregion

        #region removeElement
        public string removeElement(string elementKey)
        { // 删除
            try
            {
                XmlDocument cfgDoc = new XmlDocument();
                loadConfigDoc(cfgDoc);
                // retrieve the appSettings node  
                node = cfgDoc.SelectSingleNode("//appSettings");
                if (node == null)
                {

                    throw new InvalidOperationException("appSettings section not found");
                }

                XmlElement addElem = (XmlElement)node.SelectSingleNode("//add[@key='" + elementKey + "']");
                if (addElem == null)
                {
                    message = "此key不存在！";
                    return message;
                }
                // XPath select setting "add" element that contains this key to remove      
                node.RemoveChild(node.SelectSingleNode("//add[@key='" + elementKey + "']"));
                saveConfigDoc(cfgDoc, docName);
                message = "删除成功！";
                return message;
            }
            catch
            {
                message = "操作异常，删除失败！";
                return message;
            }
        }
        #endregion

        #region modifyElement
        public string modifyElement(string elementKey, string elementValue)
        { //修改
            try
            {
                XmlDocument cfgDoc = new XmlDocument();
                loadConfigDoc(cfgDoc);
                // retrieve the appSettings node  
                node = cfgDoc.SelectSingleNode("//appSettings");
                if (node == null)
                {
                    throw new InvalidOperationException("appSettings section not found");
                }
                // XPath select setting "add" element that contains this key to remove      
                XmlElement addElem = (XmlElement)node.SelectSingleNode("//add[@key='" + elementKey + "']");
                if (addElem == null)
                {
                    message = "此key不存在！";
                    return message;
                }

                addElem.SetAttribute("value", elementValue);
                //save it   
                saveConfigDoc(cfgDoc, docName);
                message = "修改成功！";
                return message;
            }
            catch
            {
                message = "操作异常，修改失败！";
                return message;
            }
        }
        #endregion

        #region connectionStrings
        public string connectionStrings_modifyElement(string elementKey, string elementValue)
        { //修改
            try
            {
                XmlDocument cfgDoc = new XmlDocument();
                loadConfigDoc(cfgDoc);
                // retrieve the appSettings node  
                node = cfgDoc.SelectSingleNode("//connectionStrings");
                if (node == null)
                {
                    throw new InvalidOperationException("connectionString section not found");
                }
                // XPath select setting "add" element that contains this key to remove      
                XmlElement addElem = (XmlElement)node.SelectSingleNode("//add[@name='" + elementKey + "']");
                if (addElem == null)
                {
                    message = "此key不存在！";
                    return message;
                }

                addElem.SetAttribute("connectionString", elementValue);
                //save it   
                saveConfigDoc(cfgDoc, docName);
                message = "修改成功！";
                return message;
            }
            catch
            {
                message = "操作异常，修改失败！";
                return message;
            }
        }
        #endregion
        #region loadConfigDoc
        private XmlDocument loadConfigDoc(XmlDocument cfgDoc)
        {
            // load the config file   
            if (Convert.ToInt32(ConfigType) == Convert.ToInt32(ConfigFileType.AppConfig))
            {
                docName = ((Assembly.GetEntryAssembly()).GetName()).Name;
                docName += ".exe.config";
            }
            else if (Convert.ToInt32(ConfigType) == Convert.ToInt32(ConfigFileType.custom))
            {
                docName = HttpContext.Current.Server.MapPath(ConfigPath);
            }
            else
            {
                docName = HttpContext.Current.Server.MapPath("~/web.config");
            }
            cfgDoc.Load(docName);
            return cfgDoc;
        }
        #endregion
    }

}