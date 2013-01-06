using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Text.RegularExpressions;

namespace HelloData.Web.Util
{
    public class StringPlus
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
        /// <summary>
        /// 对字符串进行Html解码
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string HtmlDecode(object content)
        {
            if (content != null && !string.IsNullOrEmpty(content.ToString()))
                return HttpUtility.HtmlDecode(content.ToString());
            return string.Empty;
        }

        /// <summary>
        /// 对字符串进行Html编码
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string HtmlEncode(string content)
        {
            return HttpUtility.HtmlEncode(content);
        }

        public static string[] GetStrArray(string str)
        {
            return str.Split(new char[',']);
        }

        public static string GetArrayStr(List<string> list, string speater)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < list.Count; i++)
            {
                if (i == list.Count - 1)
                {
                    sb.Append(list[i]);
                }
                else
                {
                    sb.Append(list[i]);
                    sb.Append(speater);
                }
            }
            return sb.ToString();
        }
        #region 删除最后一个字符之后的字符

        /// <summary>
        /// 删除最后结尾的一个逗号
        /// </summary>
        public static string DelLastComma(string str)
        {
            return str.Substring(0, str.LastIndexOf(","));
        }
        /// <summary>
        /// 判断字符串是否是中文
        /// </summary>
        public static bool IsZn(string input)
        {
            int code = 0;
            int chfrom = Convert.ToInt32("4e00", 16);    //范围（0x4e00～0x9fff）转换成int（chfrom～chend）
            int chend = Convert.ToInt32("9fff", 16);
            if (input != "")
            {
                //code = Char.ConvertToUtf32(input, index);//参数 待处理字符串，长度
                code = Char.ConvertToUtf32(input, 16);    //获得字符串input中指定索引index处字符unicode编码

                if (code >= chfrom && code <= chend)
                {
                    return true;     //当code在中文范围内返回true

                }
                else
                {
                    return false;    //当code不在中文范围内返回false
                }
            }
            return false;
        }
        /// <summary>
        /// 删除最后结尾的指定字符后的字符
        /// </summary>
        public static string DelLastChar(string str, string strchar)
        {
            return str.Substring(0, str.LastIndexOf(strchar));
        }
        /// <summary>
        /// 生成指定长度的字符串,即生成strLong个str字符串
        /// </summary>
        /// <param name="strLong">生成的长度</param>
        /// <param name="str">以str生成字符串</param>
        /// <returns></returns>
        public static string StringOfChar(int strLong, string str)
        {
            string ReturnStr = "";
            for (int i = 0; i < strLong; i++)
            {
                ReturnStr += str;
            }

            return ReturnStr;
        }

        /// <summary>
        /// 生成日期随机码
        /// </summary>
        /// <returns></returns>
        public static string GetRamCode()
        {
            #region
            return DateTime.Now.ToString("yyyyMMddHHmmssffff");
            #endregion
        }
        #endregion

        /// <summary>
        /// <函数：Decode>
        ///作用：将16进制数据编码转化为字符串，是Encode的逆过程
        /// </summary>
        /// <param name="strDecode"></param>
        /// <returns></returns>
        public static string HexDecode(string strDecode)
        {
            if (strDecode.IndexOf(@"\u") == -1)
                return strDecode;

            int startIndex = 0;
            if (strDecode.StartsWith(@"\u") == false)
            {
                startIndex = 1;
            }

            string[] codes = Regex.Split(strDecode, @"\\u");

            StringBuilder result = new StringBuilder();
            if (startIndex == 1)
                result.Append(codes[0]);
            for (int i = startIndex; i < codes.Length; i++)
            {
                try
                {
                    if (codes[i].Length > 4)
                    {
                        result.Append((char)short.Parse(codes[i].Substring(0, 4), global::System.Globalization.NumberStyles.HexNumber));
                        result.Append(codes[i].Substring(4));
                    }
                    else
                    {
                        result.Append((char)short.Parse(codes[i].Substring(0, 4), global::System.Globalization.NumberStyles.HexNumber));
                    }
                }
                catch
                {
                    result.Append(codes[i]);
                }
            }

            return result.ToString();
        }
        public static string GetStrWithNull(string obj)
        {
            if (string.IsNullOrEmpty(obj))
                return "";
            else
                return obj;
        }

        /// <summary>
        /// 将字符串按,分割，并返回int类型的数组
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string[] Split(string input)
        {
            if (string.IsNullOrEmpty(input))
                return new string[0];
            return input.Split(',');
        }

        /// <summary>
        /// 将字符串按固定分隔符分割，并返回int类型的数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string[] Split(string input, char separator)
        {
            if (!string.IsNullOrEmpty(input))
            {
                return input.Split(separator);
            }
            return new string[0];
        }


        public static T[] Split<T>(string input)
        {
            return Split<T>(input, ',');
        }

        /// <summary>
        /// 将字符串按固定分隔符分割，并返回int类型的数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static T[] Split<T>(string input, char separator)
        {
            if (string.IsNullOrEmpty(input))
            {
                return new T[0];
            }
            string[] items = input.Split(new char[] { separator }, StringSplitOptions.RemoveEmptyEntries);
            T[] result = new T[items.Length];

            for (int i = 0; i < items.Length; i++)
            {
                if (!TryParse<T>(items[i], out result[i]))
                    return new T[0];
            }

            return result;
        }

        public static List<T> Split2<T>(string input)
        {
            return Split2<T>(input, ',');
        }

        /// <summary>
        /// 将字符串按固定分隔符分割，并返回int类型的数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static List<T> Split2<T>(string input, char separator)
        {
            string[] items = input.Split(new char[] { separator }, StringSplitOptions.RemoveEmptyEntries);
            List<T> result = new List<T>();

            for (int i = 0; i < items.Length; i++)
            {
                T value;
                if (!TryParse<T>(items[i], out value))
                    return new List<T>();

                result.Add(value);
            }

            return result;
        }
        public static bool TryParse<T>(string value, out T result)
        {
            object r = null;
            if (TryParse(typeof(T), value, out r))
            {
                result = (T)r;
                return true;
            }

            result = default(T);
            return false;
        }
        /// 尝试解析字符串
        /// </summary>
        /// <param name="type">所要解析成的类型</param>
        /// <param name="value">字符串</param>
        /// <param name="result">解析结果，解析失败将返回null</param>
        /// <returns>解析失败将返回具体错误消息，否则将返回null，解析结果通过result获得</returns>
        public static bool TryParse(Type type, string value, out object result)
        {
            if (value == null)
            {

                result = null;

                return false;
            }

            bool succeed = false;
            object parsedValue = null;

            bool isNullable = type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);

            if (isNullable)
                type = type.GetGenericArguments()[0];

            if (type.IsEnum)
            {
                try
                {
                    parsedValue = Enum.Parse(type, value, true);
                    succeed = true;
                }
                catch
                {
                }
            }
            else if (type == typeof(Guid))
            {
                //TODO:此处需要改善性能

                try
                {
                    parsedValue = new Guid(value);
                    succeed = true;
                }
                catch
                {
                }
            }
            else
            {
                TypeCode typeCode = Type.GetTypeCode(type);

                switch (typeCode)
                {
                    case TypeCode.String:
                        parsedValue = value;
                        succeed = true;
                        break;
                    case TypeCode.Boolean:
                        {
                            if (value == "1")
                            {
                                parsedValue = true;
                                succeed = true;
                            }
                            else if (value == "0")
                            {
                                parsedValue = false;
                                succeed = true;
                            }
                            else
                            {
                                Boolean temp;
                                succeed = Boolean.TryParse(value, out temp);

                                if (succeed)
                                    parsedValue = temp;
                            }
                        }
                        break;
                    case TypeCode.Byte:
                        {
                            Byte temp;
                            succeed = Byte.TryParse(value, out temp);

                            if (succeed)
                                parsedValue = temp;
                        }
                        break;
                    case TypeCode.Decimal:
                        {
                            Decimal temp;
                            succeed = Decimal.TryParse(value, out temp);

                            if (succeed)
                                parsedValue = temp;
                        }
                        break;
                    case TypeCode.Double:
                        {
                            Double temp;
                            succeed = Double.TryParse(value, out temp);

                            if (succeed)
                                parsedValue = temp;

                        }
                        break;
                    case TypeCode.Int16:
                        {
                            Int16 temp;
                            succeed = Int16.TryParse(value, out temp);

                            if (succeed)
                                parsedValue = temp;

                        }
                        break;
                    case TypeCode.Int32:
                        {
                            Int32 temp;
                            succeed = Int32.TryParse(value, out temp);

                            if (succeed)
                                parsedValue = temp;

                        }
                        break;
                    case TypeCode.Int64:
                        {
                            Int64 temp;
                            succeed = Int64.TryParse(value, out temp);

                            if (succeed)
                                parsedValue = temp;

                        }
                        break;
                    case TypeCode.SByte:
                        {
                            SByte temp;
                            succeed = SByte.TryParse(value, out temp);

                            if (succeed)
                                parsedValue = temp;

                        }
                        break;
                    case TypeCode.Single:
                        {
                            Single temp;
                            succeed = Single.TryParse(value, out temp);

                            if (succeed)
                                parsedValue = temp;

                        }
                        break;
                    case TypeCode.UInt16:
                        {
                            UInt16 temp;
                            succeed = UInt16.TryParse(value, out temp);

                            if (succeed)
                                parsedValue = temp;

                        }
                        break;
                    case TypeCode.UInt32:
                        {
                            UInt32 temp;
                            succeed = UInt32.TryParse(value, out temp);

                            if (succeed)
                                parsedValue = temp;

                        }
                        break;
                    case TypeCode.UInt64:
                        {
                            UInt64 temp;
                            succeed = UInt64.TryParse(value, out temp);

                            if (succeed)
                                parsedValue = temp;

                        }
                        break;
                    case TypeCode.DateTime:
                        {
                            DateTime temp;
                            succeed = DateTime.TryParse(value, out temp);
                            if (succeed)
                            {
                                parsedValue = temp;
                            }

                        }
                        break;
                }
            }

            result = parsedValue;

            return succeed;
        }
        /// <summary>
        /// 将字符串编码为Base64字符串
        /// </summary>
        /// <param name="str">要编码的字符串</param>
        public static string Base64Encode(string str)
        {
            byte[] barray = Encoding.Default.GetBytes(str);
            return Convert.ToBase64String(barray);
        }

        /// <summary>
        /// 将Base64字符串解码为普通字符串
        /// </summary>
        /// <param name="str">要解码的字符串</param>
        public static string Base64Decode(string str)
        {
            byte[] barray = Convert.FromBase64String(str);
            return Encoding.Default.GetString(barray);
        }
    }
}
