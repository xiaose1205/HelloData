using System;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using HelloData.Util;
using System.Web;
using System.Collections;
using System.Xml;
using System.Net;

/// <summary>
///SystemCms 的摘要说明
/// </summary>
namespace HelloData.Util
{
    public class SystemCms
    {
        public SystemCms()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }

        /// <summary>
        /// News the XML.
        /// </summary>
        /// <param name="page">The page.页面 this</param>
        /// <param name="XmlPath">The XML path.XML 或者config 路径</param>
        /// <param name="RootText">示例 ROYcms/xxx </param>
        /// <returns></returns>
        public static bool NewXml(System.Web.UI.Page page,string XmlPath,string RootText)
        {
            try
            {
                string[] RootT = RootText.Split('/');
                XmlDocument doc = new XmlDocument();
                XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", "utf-8", null);
                doc.AppendChild(dec);
                //创建一个根节点（一级） 
                XmlElement root = doc.CreateElement(RootT[0]);
                doc.AppendChild(root);
                //创建节点（二级） 
                XmlNode node = doc.CreateElement(RootT[1]);
                XmlAttribute attr = null;
                attr = doc.CreateAttribute("Time");
                attr.Value = DateTime.Now.ToString();
                node.Attributes.SetNamedItem(attr);
                for (int i = 0; i < page.Request.Form.Count; i++)
                {
                    if (!page.Request.Form.GetKey(i).Contains("__"))
                    {
                        //带有"<![CDATA[   ]]>"的元素 
                        XmlElement element = doc.CreateElement(page.Request.Form.GetKey(i));
                        XmlCDataSection idata = doc.CreateCDataSection("item");
                        idata.InnerText = page.Request.Form.Get(i);
                        element.AppendChild(idata);
                        node.AppendChild(element);
                    }
                }
                root.AppendChild(node);
                doc.Save(XmlPath);
                Console.Write(doc.OuterXml);
                return true;
            }
            catch
            {
                return false;
            }
        
        }
        /// <summary>
        /// Gets the XML value. 
        /// </summary>
        /// <param name="XmlPath">The XML path.</param>
        /// <param name="Xpath">The xpath. ROYcms/RootName/ItemName</param>
        /// <returns></returns>
        public static string GetXmlValue(string XmlPath, string Xpath)
        {
            try
            {
               XmlControl XmlControl = new XmlControl(XmlPath);
                return XmlControl.GetText(Xpath);
            }
            catch
            {
                return "";
            }
        }
        public static string GetEncod(string web_url)
        {
            //存放网页的源文件   
            string Encod = null;

            HttpWebRequest all_codeRequest = (HttpWebRequest)WebRequest.Create(web_url);
            WebResponse all_codeResponse = all_codeRequest.GetResponse();

            string contenttype = all_codeResponse.Headers["Content-Type"];//得到结果为"text/html;   charset=utf-8"      

            //网页编码判断   
            if (contenttype.Contains("gb2312"))
            {
                Encod = "gb2312";
            }
            else if (contenttype.Contains("gbk"))
            {
                Encod = "gbk";
            }
            else if (contenttype.Contains("utf-8"))
            {
                Encod = "utf-8";
            }
            return Encod;

        }
                /// <summary>
        /// 错误日志记录
        /// </summary>
        /// <returns></returns>
        public static void InsertErrLog(string TXT,string path)
        {
            try
            {
                System.DateTime currentTime = System.DateTime.Now;
                string strYMD = currentTime.ToString("yyyy-MM-dd");
                string FILE_NAME = HttpContext.Current.Server.MapPath("~/app_data/LOG/" + strYMD + ".txt");//每天按照日期建立一个不同的文件名
                StreamWriter sr;
                if (!File.Exists(FILE_NAME)) //如果文件存在,则创建File.AppendText对象
                {
                    sr = File.CreateText(FILE_NAME);
                }
                else  //如果文件不存在,则创建File.CreateText对象
                {
                    sr = File.AppendText(FILE_NAME);
                }

                sr.WriteLine(TXT + "|" + path + "|" + DateTime.Now.ToString("yyyy-mm-dd hh:mm:ss") + "\r\n");//将传入的字符串加上时间写入文本文件一行
                sr.Flush();
                sr.Close();
            }
            catch (Exception ex)
            {
                throw ex;
               // HttpContext.Current.Response.Write("<!--地址:" + path + "===" + ex + "[ROYcms!NT 运行错误]-->");
            }

            HttpContext.Current.Response.Write("<!--" + TXT + "地址:" + path + "[ROYcms!NT 运行错误]-->"); 
        }


        /// <summary>
        /// 清除所有缓存
        /// </summary>
        /// <returns></returns>

        public static void RemoveAllCache()
        {
            System.Web.Caching.Cache _cache = HttpRuntime.Cache;
            IDictionaryEnumerator CacheEnum = _cache.GetEnumerator();
            ArrayList al = new ArrayList();
            while (CacheEnum.MoveNext())
            {
                al.Add(CacheEnum.Key);
            }
            foreach (string key in al)
            {
                _cache.Remove(key);
            }
        }
        /// <summary>
        /// 获得当前WEB应用程序的物理路径
        /// </summary>
        /// <returns></returns>
        public static string GetApplicationPath()
        {
            return GetApplicationPath();
        }
        /// <summary>
        /// 返回应用程序根目录的URL
        /// </summary>
        /// <returns></returns>
        public static string getapppath()
        {
            string AppPath = System.Web.HttpContext.Current.Request.ApplicationPath.Trim();
            if (AppPath.Length > 1)
            {
                AppPath = AppPath + "/";
            }
            return ("http://" + System.Web.HttpContext.Current.Request.Url.Authority + AppPath);
        }
        /// <summary>
        /// 创建文件
        /// </summary>
        /// <param name="filePath">路径</param>
        /// <param name="text">内容</param>
        public static void CreateFile(string filePath, string text, string Encod)
        {
            StreamWriter sw = null;
            //Business.Folder.Files.CreateFolder(filePath.Substring(0, filePath.LastIndexOf("\\") + 1));
            try
            {
                sw = new StreamWriter(filePath, false, Encoding.GetEncoding(Encod));
                sw.WriteLine(text);
                sw.Flush();
                sw.Close();
            }
            catch (Exception ex)
            {
                //logger.Error("创建文件出错 文件地址:" + filePath, ex);
                throw ex;
                //sw.Close();
            }

        }
        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="filePath">路径</param>

        public static void DeleteFile(string filePath)
        {

            try
            {
                File.Delete(filePath);
            }
            catch (Exception ex)
            {
                //logger.Error("创建文件出错 文件地址:" + filePath, ex);
                throw ex;
            }
        }
        /// <summary>
        /// File_datas the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public static string Read_File(string path, string Encod)
        {
            //读取 外部文件
            StringBuilder HTM = new StringBuilder();
            try
            {
                using (StreamReader reader = new StreamReader(path, System.Text.Encoding.GetEncoding(Encod)))
                {
                    while (reader.Peek() >= 0)
                    {
                        HTM.Append(((char)reader.Read()).ToString());
                    }
                }
            }
            catch { return null; }
            return HTM.ToString();
        }
        #region 压缩Html文件
        /// <summary>
        /// 压缩Html文件
        /// </summary>
        /// <param name="html">Html文件</param>
        /// <returns></returns>
        public static string ZipHtml(string Html)
        {

            Html = Regex.Replace(Html, @">\s+?<", "><");//去除Html中的空白字符.
            Html = Regex.Replace(Html, @"\r\n\s*", "");
            Html = Regex.Replace(Html, @"<body([\s|\S]*?)>([\s|\S]*?)</body>", @"<body$1>$2</body>", RegexOptions.IgnoreCase);
            return Html;
        }
        #endregion
        /// <summary>
        /// 去掉所有HTML标签
        /// </summary>
        /// <param name="strHtml"></param>
        /// <returns></returns>
        public static string DropHTML(string strHtml)
        {
            return Regex.Replace(strHtml, "<[^>]*>", "");
        }
        /// <summary>
        /// Gets the filelist.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns></returns>
        public static FileInfo[] GetFilelist(string filePath)//返回文件列表，filePath为根目录路径
        {
            //int filecounts = 0;//定义一个变量，表示文件数目
            FileInfo[] NewFileInfo;//定义一个数组，储存找到的文件并作为返回值
            DirectoryInfo FatherDirectory = new DirectoryInfo(filePath); //创建一个当前目录的实例
            //NewFileInfo = FatherDirectory.GetFiles("*.*", SearchOption.AllDirectories);
            NewFileInfo = FatherDirectory.GetFiles("*.*");
            //得到文件集，包括目录及其子目录下的所有txt文件，如果用"*.*"就可以获得全部文件了
            //为了得到其他类型的文件，又写了下面的语句
             //AddNewArray<FileInfo>(ref NewFileInfo, FatherDirectory.GetFiles("*.mp3", SearchOption.AllDirectories));
            //把另一个查询记录并入到这个文件列表里
            return NewFileInfo;//返回文件列表
        }

        /// <summary>
        /// 发送一份邮件
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="url">The URL.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public static bool SendMail(string title, string content, string ToEmail)
        {
            try
            {
                System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
                client.Host = "smtp.qq.com";
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential("roycms@qq.com", "xiaohui123");
                client.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage("roycms@qq.com", ToEmail);
                message.Subject = title;
                message.Body = content;
                message.BodyEncoding = System.Text.Encoding.UTF8;
                message.IsBodyHtml = true;
                try
                {
                    client.Send(message);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}