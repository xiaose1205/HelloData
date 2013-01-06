#region Version Info
/* ========================================================================
* 【本类功能概述】
* 
* 作者：王军 时间：2012/12/30 21:39:23
* 文件名：HttpHelper
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
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Xml;

////HttpHelper httpHelper = new HttpHelper();
//           //string html = httpHelper.MethodGetHttpStr(ChapterUrl);
//           WebClient web = new WebClient();
//           web.Credentials = CredentialCache.DefaultCredentials;
//           byte[] pageData = web.DownloadData(ChapterUrl);
//           string html = Encoding.UTF8.GetString(pageData);
namespace HelloData.FWCommon.Utils
{
    /// <summary>
    /// get,post的cookies
    /// </summary>
    public class CookiesModel
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public string Domain { get; set; }
    }
    /// <summary>
    /// 请求或者返回的头文件
    /// </summary>
    public class HttpHeaderModel
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
    public class HttpHelper
    {
        public string NameSpace { get; set; }
        public Encoding WEncoding = Encoding.UTF8;

        public List<HttpHeaderModel> HttpHeaders = new List<HttpHeaderModel>();
        public List<HttpHeaderModel> ResponseHttpHeaders = new List<HttpHeaderModel>();
        public List<CookiesModel> DicCookies = new List<CookiesModel>();

        public XmlDocument ResultPamrs(string result, string method)
        {
            int index = result.IndexOf("<?xml");
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(result.Substring(index, result.Length - index));
            return doc;
        }
        public HttpHelper()
        {
            IniStalling();
        }
        public void IniStalling()
        {
            HttpHeaders = new List<HttpHeaderModel>();
            ResponseHttpHeaders = new List<HttpHeaderModel>();
            DicCookies = new List<CookiesModel>();
            AddHttpHeader("Accept", "text/html, application/xhtml+xml, */*");
            AddHttpHeader("Accept-Language", "zh-Hans-CN,zh-Hans;q=0.5");
            // AddHttpHeader("User-Agent", "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Win64; x64; Trident/5.0)");
            //AddHttpHeader("UA-CPU", "AMD64");
            AddHttpHeader("Connection", "Keep-Alive");
            AddHttpHeader("User-Agent", "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident/6.0)");
            AddHttpHeader("Accept-Encoding", " deflate");
        }

        /// <summary>
        /// 添加HTTP头
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void AddHttpHeader(string key, string value)
        {
            foreach (HttpHeaderModel httpHeaderModel in HttpHeaders)
            {
                if (httpHeaderModel.Key == key)
                {
                    httpHeaderModel.Value = value;
                    return;
                }
            }
            HttpHeaders.Add(new HttpHeaderModel()
            {
                Key = key,
                Value = value
            });
        }

        public string MethodGetHttpStr(string url)
        {

            return GetHttpByte(url, null);
        }

        public string MethodPostHttpStr(string url, string data)
        {

            return GetHttpByte(url, data);
        }

        /// <summary>
        /// 设置命名空间，请在地址后面加上wsdl获取。
        /// </summary>


        private int index = 0;
        public string CreateSoap(object obj)
        {
            StringBuilder sb = new StringBuilder();
            Type tType = obj.GetType();
            PropertyInfo[] pInfos = tType.GetProperties();
            sb.AppendLine("<test" + index + " xsi:type=\"m" + index + ":" + tType.Name + "\">");
            foreach (PropertyInfo pInfo in pInfos)
            {
                sb.AppendLine(string.Format(" <{0}>{1}</{0}>", pInfo.Name, pInfo.GetValue(obj, null)));
            }
            sb.AppendLine("</test" + index + ">");
            index++;
            return sb.ToString();
        }

        public string CreateSoap(Dictionary<string, string> MethodParms)
        {
            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<string, string> keyValuePair in MethodParms)
            {
                sb.AppendLine(string.Format(" <{0}>{1}</{0}>", keyValuePair.Key, keyValuePair.Value));
            }
            return sb.ToString();
        }

        public string GetWebServiceStr(string url, string MethodName, string soap)
        {
            index = 0;
            if (string.IsNullOrEmpty(NameSpace))
                throw new MissingFieldException("请输入NameSpace");
            if (url.Contains("asmx"))
                AddHttpHeader("SOAPAction", "\"" + NameSpace.TrimEnd('/') + "/" + MethodName + "\"");
            else
                AddHttpHeader("SOAPAction", "\"\"");

            AddHttpHeader("Content-Type", "text/xml; charset=utf-8");
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            sb.AppendLine("<soap:Envelope xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\">");
            sb.AppendLine("<soap:Body>");
            sb.AppendLine(string.Format("<" + MethodName + " xmlns=\"" + NameSpace + "\">"));
            sb.Append(soap);
            sb.AppendLine(string.Format("</" + MethodName + ">"));
            sb.AppendLine("</soap:Body>");
            sb.AppendLine("</soap:Envelope>");
            return MethodPostHttpStr(url, sb.ToString());
        }

        public string GetHttpByte(string url, string data = "")
        {
            bool methodPost = !string.IsNullOrEmpty(data);
            if (methodPost)
            {
                byte[] sendBytes = WEncoding.GetBytes(data);
                AddHttpHeader("Content-Length", sendBytes.Length.ToString());
            }
            string cookies =
                DicCookies.Aggregate(string.Empty,
                (current, cookie) => current + string.Format("{0}:{1};", cookie.Key, cookie.Value));
            string[] urlspils = url.Replace("http://", "").Split('/');
            string host = urlspils[0];
            string methodurl = url.Replace("http://", "").Remove(0, host.Length);
            string[] ipport = host.Split(':');
            string ip = "127.0.0.1";
            string post = "80";
            if (ipport.Length > 1)
            {
                host = ipport[0];
                post = ipport[1];
            }
            IPAddress[] addressList = Dns.GetHostAddresses(host);

            if (addressList.Length > 0)
            {
                ip = addressList[0].ToString();
            }

            Socket httpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint serverHost = new IPEndPoint(IPAddress.Parse(ip), int.Parse(post));

            StringBuilder httpHeader = new StringBuilder();
            httpHeader.Append((methodPost ? "POST" : "GET") + " " + methodurl + " HTTP/1.1\r\n");
            AddHttpHeader("Host", host);
            if (!string.IsNullOrEmpty(cookies))
                AddHttpHeader("Cookie", cookies);
            foreach (var item in HttpHeaders)
            {
                httpHeader.Append(string.Format("{0}: {1}\r\n", item.Key, item.Value));
            }
            string httpData = string.Format("{0}\r\n{1}", httpHeader, data);
            // Console.WriteLine(httpData);

            httpSocket.Connect(serverHost);
            if (!httpSocket.Connected)
                throw new WebException("连接不上服务器");
            byte[] bytesSend = WEncoding.GetBytes(httpData);

            #region Socket

            //httpSocket.Send(bytesSend);

            //byte[] bytesReceive = new byte[8192];
            //string getresult = string.Empty;
            //while (true)
            //{
            //    int receiveLen = httpSocket.Receive(bytesReceive, bytesReceive.Length, SocketFlags.None);
            //    getresult += WEncoding.GetString(bytesReceive, 0, receiveLen);
            //    if ((receiveLen) == 0 || receiveLen < bytesReceive.Length)
            //        break;
            //    Thread.Sleep(10);
            //}
            //return getresult;

            #endregion

            #region networkstrem

            using (var stream = new NetworkStream(httpSocket))
            {
                stream.Write(bytesSend, 0, bytesSend.Length);
                while (true)
                {
                    var line = ReadLine(stream);
                    if (line.Length == 0)
                        break;

                    if (line.Contains("HTTP/1.1"))
                        continue;

                    int index = line.IndexOf(':');
                    ResponseHttpHeaders.Add(new HttpHeaderModel()
                    {
                        Key = line.Substring(0, index),
                        Value = line.Substring(index + 2)
                    });
                }
                Stream responseStream = stream;
                bool ischunked = GetFromResponseHeader("Transfer-Encoding").Count == 1;
                List<string> conlengt = GetFromResponseHeader("Content-Length");
                long contentlenght = 0;
                if (conlengt.Count > 0)
                    contentlenght = long.Parse(conlengt[0]);
                List<string> contentEncodings = GetFromResponseHeader("Content-Encoding");

                if (ischunked)
                {
                    StringBuilder sbReadstr = new StringBuilder();

                    //  var respBuffer = new byte[contentlenght + 1024];
                    int readlinecount = 1;
                    int length = 0;
                    while (true)
                    {
                        if (readlinecount % 2 == 0)
                        {
                            if (contentEncodings.Count == 1)
                            {
                                if (contentEncodings[0].Equals("gzip"))
                                {
                                    responseStream = new GZipStream(responseStream, CompressionMode.Decompress);
                                }
                                else if (contentEncodings[0].Equals("deflate"))
                                {
                                    responseStream = new DeflateStream(responseStream, CompressionMode.Decompress);
                                }
                            }
                            string readstr = ReadLine(responseStream, length);

                            sbReadstr.AppendLine(readstr);
                            readlinecount++;
                            break;
                        }
                        var line = ReadLine(responseStream);
                        if (line == null)
                        {
                            break;
                        }
                        int lenght = 0;
                        if (readlinecount % 2 == 0)
                        {
                            sbReadstr.AppendLine(line);
                        }
                        else if (line.Length == 1 && int.TryParse(line, out lenght) && readlinecount % 2 == 1 && readlinecount != 1)
                        {
                            if (lenght == 0)
                                break;
                        }
                        else
                        {

                            length = Convert.ToInt32(line, 16);
                        }
                        readlinecount++;
                    }
                    //var strbytes = WEncoding.GetBytes(sbReadstr.ToString());
                    //memStream.Write(strbytes, 0, strbytes.Length);
                    return sbReadstr.ToString();
                }
                else
                {
                    var respBuffer = new byte[contentlenght + 1024];

                    try
                    {
                        int bytesRead = responseStream.Read(respBuffer, 0, respBuffer.Length);
                        {
                            return WEncoding.GetString(respBuffer, 0, bytesRead);
                        }
                    }
                    finally
                    {
                        responseStream.Close();
                    }
                }
            }

            #endregion
            try
            { }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (httpSocket.Connected)
                    httpSocket.Close();
            }
        }
        public List<string> GetFromResponseHeader(string key)
        {
            return (from item in ResponseHttpHeaders where item.Key == key select item.Value).ToList();
        }
        string ReadLine(Stream stream, int length)
        {
            byte[] bytes = new byte[length];
            stream.Read(bytes, 0, length);
            StreamReader srdPreview = new StreamReader(stream);
            String temp = string.Empty;
            while (srdPreview.Peek() > -1)
            {
                String input = srdPreview.ReadLine();
                temp += input;
            }

            return temp;

        }
        string ReadLine(Stream stream)
        {
            var lineBuffer = new List<byte>();
            while (true)
            {
                int b = stream.ReadByte();
                if (b == -1)
                {
                    return null;
                }
                if (b == 10)
                {
                    break;
                }
                if (b != 13)
                {
                    lineBuffer.Add((byte)b);
                }
            }
            return WEncoding.GetString(lineBuffer.ToArray());
        }

        public string[] ParMTReport(XmlDocument xmldoc)
        {
            string[] SnedSplitWJX = null;
            if (xmldoc == null)
                return null;
            XmlNodeList xn = xmldoc.SelectNodes("/*/*/*/*");
            for (int i = 0; i < xn.Count; i++)
            {
                XmlNode item = xn[i];
                SnedSplitWJX = item.InnerText.ToString().Split(':');
            }
            return SnedSplitWJX;

        }

    }
}
