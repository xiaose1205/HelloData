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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

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
        public static string StripHtml(string Html)
        {
            //从Html中录入 <script> 标签<script[^>]*>[sS]*?</script>
            string scriptregex = @"<script[^>]*>(.*?)</script>";
            System.Text.RegularExpressions.Regex scripts = new System.Text.RegularExpressions.Regex(scriptregex, RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.ExplicitCapture);
            string scriptless = scripts.Replace(Html, " ");

            //从Html中录入 <style> 标签<style[^>]*>(.*?)</style>
            string styleregex = @"<style[^>]*>(.*?)</style>";
            System.Text.RegularExpressions.Regex styles = new System.Text.RegularExpressions.Regex(styleregex, RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.ExplicitCapture);
            string styleless = styles.Replace(scriptless, " ");

            //从Html中录入 <NOSEARCH> 标签(当 NOSEARCH 在 web.config/Preferences 类中<Preferences 类是项目里面,专门用于获取web.config/app.config>)
            //TODO: NOTE: this only applies to INDEXING the text - links are parsed before now, so they aren't "excluded" by the region!! (yet)
            string ignoreless = styleless;

            //从Html中录入 <!--comment--> 标签 
            //string commentregex = @"<!--.*?-->";  
            string commentregex = @"<!(?:--[sS]*?--s*)?>";
            System.Text.RegularExpressions.Regex comments = new System.Text.RegularExpressions.Regex(commentregex, RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.ExplicitCapture);
            string commentless = comments.Replace(ignoreless, " ");

            //从Html中录入 Html 标签
            System.Text.RegularExpressions.Regex objRegExp = new System.Text.RegularExpressions.Regex("<(.| )+?>", RegexOptions.IgnoreCase);

            //替换所有HTML标签以匹配空格字符串
            string output = objRegExp.Replace(commentless, " ");

            //替换所有  < 和 > 为 &lt; 和 &gt;
            output = output.Replace("<", "&lt;");
            output = output.Replace(">", "&gt;");

            objRegExp = null;
            return ReplaceSpace(output);
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
      
    }
}
