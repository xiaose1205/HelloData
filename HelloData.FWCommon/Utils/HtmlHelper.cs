#region Version Info
/* ========================================================================
* 【本类功能概述】
* 
* 作者：王军 时间：2013/1/1 00:32:40
* 文件名：HtmlHelper
* 版本：V1.0.1
* 联系方式：511522329  
*
* 修改者： 时间： 
* 修改说明：
* ========================================================================
*/
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace HelloData.FWCommon.Utils
{
    public class HtmlHelper
    {
        private const string Pattern = @"(?i)<img\b[^>]*?src=(['""]?)([^'""\s>]+)\1[^>]*>";
        /// <summary>
        /// 获取日志内容中的第一张图
        /// </summary>
        /// <param name="content">日志内容</param>
        /// <returns></returns>
        public static string GetBlogArticleThumbnail(string content)
        {
            MatchCollection imageMatches = Regex.Matches(content, Pattern);
            if (imageMatches.Count > 0)
                return imageMatches[0].Groups[2].Value;
            return string.Empty;
        }
        ///   <summary> 
        ///   移除HTML标签 
        ///   </summary> 
        ///   <param   name="HTMLStr">HTMLStr</param> 
        public static string ParseTags(string HTMLStr)
        {
            return StripHtml(HTMLStr);
            //  return System.Text.RegularExpressions.Regex.Replace(HTMLStr, "<[^>]*>", "");
        }
        public static string StripHtml(string Htmlstring)
        {
            //删除脚本   
            Htmlstring = Regex.Replace(Htmlstring, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
            //删除HTML   
            Htmlstring = Regex.Replace(Htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"-->", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);

            Htmlstring = Regex.Replace(Htmlstring, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(nbsp|#160);", "   ", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&#(\d+);", "", RegexOptions.IgnoreCase);

            Htmlstring.Replace("<", "");
            Htmlstring.Replace(">", "");
            Htmlstring.Replace("\r\n", "");

            return Htmlstring;
        }


        /// <summary>
        /// 转到JS用的string
        /// </summary>
        /// <param name="text">文本</param>
        /// <returns></returns>
        public static string ReplaceSpace(string text)
        {
            StringBuilder buffer = new StringBuilder(text);
            buffer.Replace("\\", "");
            buffer.Replace("\t", "");
            buffer.Replace("\n", "");
            buffer.Replace("\r", "");
            buffer.Replace("\"", "");
            buffer.Replace("\'", "");
            buffer.Replace("/", "");
            buffer.Replace(" ", "");
            return buffer.ToString();
        }
        /// <summary>
        /// 转到JS用的string
        /// </summary>
        /// <param name="text">文本</param>
        /// <returns></returns>
        public static string ToJavaScriptString(string text)
        {
            StringBuilder buffer = new StringBuilder(text);
            buffer.Replace("\\", @"\\");
            buffer.Replace("\t", @"\t");
            buffer.Replace("\n", @"\n");
            buffer.Replace("\r", @"\r");
            buffer.Replace("\"", @"\""");
            buffer.Replace("\'", @"\'");
            buffer.Replace("/", @"\/");
            return buffer.ToString();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="html">获取的html</param>
        /// <param name="openurl">原始的url</param>
        /// <param name="spliderdeep">扫描网站的深度</param>
        /// <param name="isnowithparms">是否支持参数url的扫描</param>
        /// <returns></returns>
        public static Dictionary<string, string> GetAllLink(string html, string openurl, int spliderdeep, bool isnowithparms)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            char[] buffer = html.ToCharArray();
            int state = 0;
            String a = "";
            string linkcontent = string.Empty;
            string linkurl = string.Empty;
            Hashtable ht = new Hashtable();
            int index = 0;
            for (int i = 0; i < buffer.Length; i++)
            {
                switch (state)
                {
                    case 0: // 状态0 
                        if (buffer[i] == '<') // 读入的是'<' 
                        {
                            a += buffer[i];
                            state = 1; // 切换到状态1 
                        }
                        if (!string.IsNullOrEmpty(linkurl))
                            if (buffer[i] != '<')
                                linkcontent += buffer[i];
                            else
                            {
                                if (string.IsNullOrEmpty(linkcontent))
                                    continue;
                                if (!dictionary.ContainsKey(linkurl.Trim()))
                                    dictionary.Add(linkurl.Trim(), linkcontent.Trim());
                                linkurl = string.Empty;
                                linkcontent = string.Empty;
                            }
                        break;
                    case 1: // 状态1 
                        if (buffer[i] == 'a' || buffer[i] == 'A') // 读入是'a'或'A' 
                        {
                            a += buffer[i];
                            state = 2; // 切换到状态2 
                        }
                        else
                        {
                            a = "";
                            state = 0; // 切换到状态0 
                        }
                        break;
                    case 2: // 状态2 
                        if (buffer[i] == ' ' || buffer[i] == '\t') // 读入的是空格或'\t' 
                        {
                            a += buffer[i];
                            state = 3;
                        }
                        else
                        {
                            a = "";
                            state = 0; // 切换到状态0 
                        }
                        break;
                    case 3: // 状态3 
                        if (buffer[i] == '>') // 读入的是'>'，已经成功获得一个 
                        {
                            a += buffer[i];
                            try
                            {
                                string url = getUrl(getHref(a), openurl); // 获得中的href属性的值 

                                if (url != null)
                                {
                                    if (!url.Contains("javascript") && !url.Contains("%") && !url.Contains("..") &&
                                        !url.Contains("#") && !url.Contains("(") && !url.Contains(")") &&
                                        !url.Contains("amp") && !url.Contains(".gif") && !url.Contains(".jpg"))
                                    {
                                        bool isRight = false;
                                        if (!ht.ContainsValue(url.Trim()) &&
                                            GetUrlDomainName(url) == GetUrlDomainName(openurl))
                                        {
                                            if (isnowithparms && (!url.Contains("?") && !url.Contains("&")))
                                                isRight = true;
                                            if (!isnowithparms)
                                                isRight = true;

                                            //if (GetUrlDeep(url) <= spliderdeep)
                                            //    isRight = true;
                                            if (isRight)
                                            {
                                                ht.Add(index, url);
                                                linkurl = url;
                                            }
                                        }
                                    }
                                }
                                index++;
                            }
                            catch
                            {
                            }
                            state = 0; // 在获得一个后，重新切换到状态0 
                        }
                        else
                            a += buffer[i];
                        break;
                }
            }
            return dictionary;
        }

        private static int GetUrlDeep(string url)
        {
            return url.Split('/').Length - 2;
        }

        private static String getHref(string a)
        {
            try
            {
                string p = @"href\s*=\s*('[^']*'|""[^""]*""|\S+\s+)"; // 获得Href的正则表达式 
                MatchCollection matches = Regex.Matches(a, p,
                RegexOptions.IgnoreCase |
                RegexOptions.ExplicitCapture);
                foreach (Match nextMatch in matches)
                {
                    return nextMatch.Value; // 返回href 
                }
                return null;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public static string GetUrlDomainName(string strHtmlPagePath)
        {
            string p = @"http://(?<domain>[^/]*)";
            Regex reg = new Regex(p, RegexOptions.IgnoreCase);
            Match m = reg.Match(strHtmlPagePath);
            return m.Groups["domain"].Value;
        }
        private static String getUrl(string href, string firsturl)
        {
            try
            {
                if (href == null) return href;
                int n = href.IndexOf('='); // 查找'='位置 
                String s = href.Substring(n + 1);
                int begin = 0, end = 0;
                string sign = "";
                if (s.Contains("\"")) // 第一种情况 
                    sign = "\"";
                else if (s.Contains("'")) // 第二种情况 
                    sign = "'";
                else // 第三种情况 
                    return getFullUrl(s.Trim(), firsturl);
                begin = s.IndexOf(sign);
                end = s.LastIndexOf(sign);
                return getFullUrl(s.Substring(begin + 1, end - begin - 1).Trim(), firsturl);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        private static String getFullUrl(string url, string firsturl)
        {
            try
            {
                if (url == null) return url;
                //   if (processPattern(url)) return null; // 过滤不想下载的url 
                // 如果url前有http://或https://，为绝对路径，按原样返回 
                if (url.ToLower().StartsWith("http://") || url.ToLower().StartsWith("https://"))
                    return url;
                Uri parentUri = new Uri(firsturl);
                string port = "";
                if (!parentUri.IsDefaultPort)
                    port = ":" + parentUri.Port.ToString();
                if (url.StartsWith("/")) // url以"/"开头，直接放在host后面 
                    return parentUri.Scheme + "://" + parentUri.Host + port + url;
                else // url不以"/"开头，放在url的路径后面 
                {
                    string s = "";
                    s = parentUri.LocalPath.Substring(0, parentUri.LocalPath.LastIndexOf("/"));
                    return parentUri.Scheme + "://" + parentUri.Host + port + s + "/" + url;
                }
                // return null;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
