using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Web;

namespace HelloData.Util
{
    /// <summary>
    /// 获取URL地址的网页内容
    /// </summary>
    public class HttpNet
    {


        /// <summary>
        /// 获取指定 指定URL的HTML源代码
        /// </summary>
        /// <param name="url"></param>
        /// <param name="encoding">如果为NULL 则自动去获取编码</param>
        /// <returns></returns>
        public static bool GetImage(string url)
        {
            try
            {
                HttpWebRequest hwr = (HttpWebRequest)HttpWebRequest.Create(url);
                HttpWebResponse res;

                try
                {
                    res = (HttpWebResponse)hwr.GetResponse();
                }
                catch
                {
                    return false;
                }

                if (res.StatusCode == HttpStatusCode.OK)
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
            return false;
        }

        /// <summary>
        /// 获取指定 指定URL的HTML源代码
        /// </summary>
        /// <param name="url"></param>
        /// <param name="encoding">如果为NULL 则自动去获取编码</param>
        /// <returns></returns>
        public static string GetHtml(string url, Encoding encoding)
        {
            try
            {
                HttpWebRequest hwr = (HttpWebRequest)HttpWebRequest.Create(url);
                HttpWebResponse res;

                try
                {
                    res = (HttpWebResponse)hwr.GetResponse();
                }
                catch
                {
                    return string.Empty;
                }

                if (res.StatusCode == HttpStatusCode.OK)
                {
                    using (Stream mystream = res.GetResponseStream())
                    {
                        //没有指定编码，猜
                        if (encoding == null)
                        {
                            return DecodeData(mystream, res);
                        }
                        //指定了编码
                        else
                        {
                            using (StreamReader reader = new StreamReader(mystream, encoding))
                            {
                                return reader.ReadToEnd();
                            }
                        }
                    }
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 用当前用户的身份转发当前页面的内容到远程页面， 并返回远程返回的HTML。
        /// </summary>
        /// <param name="Request"></param>
        /// <param name="url"></param>
        /// <param name="headFields"></param>
        /// <returns></returns>
        public static HttpWebResponse PostToRemoteUrl(HttpRequest Request, string url, params KeyValuePair<string, string>[] headFields)
        {
            string newUrl = url;
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(newUrl);

            request.Method = "POST";
            request.ContentType = "application/octet-stream";

            if (headFields != null)
            {
                foreach (KeyValuePair<string, string> s in headFields)
                {
                    request.Headers.Set(s.Key, s.Value);
                }
            }

            byte[] avatarData = new byte[Request.InputStream.Length];

            Request.InputStream.Read(avatarData, 0, avatarData.Length);

            HttpCookie userCookie = Request.Cookies["My_web"];

            if (userCookie != null)
            {
                request.Headers.Set(HttpRequestHeader.Cookie, "My_web" + "=" + userCookie.Value);
            }

            using (Stream str = request.GetRequestStream())
            {
                str.Write(avatarData, 0, avatarData.Length);
                str.Close();
            }

            return (HttpWebResponse)request.GetResponse();
        }

        private static string DecodeData(Stream responseStream, HttpWebResponse response)
        {
            string name = null;
            string text2 = response.Headers["content-type"];
            if (text2 != null)
            {
                int index = text2.IndexOf("charset=");
                if (index != -1)
                {
                    name = text2.Substring(index + 8);
                }
            }
            MemoryStream stream = new MemoryStream();
            byte[] buffer = new byte[0x400];
            for (int i = responseStream.Read(buffer, 0, buffer.Length); i > 0; i = responseStream.Read(buffer, 0, buffer.Length))
            {
                stream.Write(buffer, 0, i);
            }
            responseStream.Close();
            if (name == null)
            {
                MemoryStream stream3 = stream;
                stream3.Seek((long)0, SeekOrigin.Begin);
                string text3 = new StreamReader(stream3, Encoding.ASCII).ReadToEnd();
                if (text3 != null)
                {
                    int startIndex = text3.IndexOf("charset=");
                    int num4 = -1;
                    if (startIndex != -1)
                    {
                        num4 = text3.IndexOf("\"", startIndex);
                        if (num4 != -1)
                        {
                            int num5 = startIndex + 8;
                            name = text3.Substring(num5, (num4 - num5) + 1).TrimEnd(new char[] { '>', '"' });
                        }
                    }
                }
            }
            Encoding aSCII = null;
            if (name == null)
            {
                aSCII = Encoding.GetEncoding("gb2312");
            }
            else
            {
                try
                {
                    if (name == "GBK")
                    {
                        name = "GB2312";
                    }
                    aSCII = Encoding.GetEncoding(name);
                }
                catch
                {
                    aSCII = Encoding.GetEncoding("gb2312");
                }
            }
            stream.Seek((long)0, SeekOrigin.Begin);
            StreamReader reader2 = new StreamReader(stream, aSCII);
            return reader2.ReadToEnd();
        }

    }
}
