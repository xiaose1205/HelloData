#region Version Info
/* ========================================================================
* 【本类功能概述】
* 
* 作者：王军 时间：2012/12/30 17:19:41
* 文件名：StringPlus
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
using System.Web;

namespace HelloData.FWCommon.Utils
{
    
    public class StringPlus
    {
        /// <summary>
        /// 把字符串按照分隔符转换成 List
        /// </summary>
        /// <param name="str">源字符串</param>
        /// <param name="speater">分隔符</param>
        /// <param name="toLower">是否转换为小写</param>
        /// <returns></returns>
        public static List<string> GetStrArray(string str, char speater, bool toLower)
        {
            List<string> list = new List<string>();
            string[] ss = str.Split(speater);
            foreach (string s in ss)
            {
                if (!string.IsNullOrEmpty(s) && s != speater.ToString())
                {
                    string strVal = s;
                    if (toLower)
                    {
                        strVal = s.ToLower();
                    }
                    list.Add(strVal);
                }
            }
            return list;
        }
        /// <summary>
        /// 把字符串转 按照, 分割 换为数据
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string[] GetStrArray(string str)
        {
            return str.Split(new Char[] { ',' });
        }
        /// <summary>
        /// 把 List<string> 按照分隔符组装成 string
        /// </summary>
        /// <param name="list"></param>
        /// <param name="speater"></param>
        /// <returns></returns>
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
        /// <summary>
        /// 得到数组列表以逗号分隔的字符串
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string GetArrayStr(List<int> list)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < list.Count; i++)
            {
                if (i == list.Count - 1)
                {
                    sb.Append(list[i].ToString());
                }
                else
                {
                    sb.Append(list[i]);
                    sb.Append(",");
                }
            }
            return sb.ToString();
        }
        /// <summary>
        /// 得到数组列表以逗号分隔的字符串
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string GetArrayValueStr(Dictionary<int, int> list)
        {
            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<int, int> kvp in list)
            {
                sb.Append(kvp.Value + ",");
            }
            if (list.Count > 0)
            {
                return DelLastComma(sb.ToString());
            }
            else
            {
                return "";
            }
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
        /// 删除最后结尾的指定字符后的字符
        /// </summary>
        public static string DelLastChar(string str, string strchar)
        {
            return str.Substring(0, str.LastIndexOf(strchar));
        }

        #endregion




        /// <summary>
        /// 转全角的函数(SBC case)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToSBC(string input)
        {
            //半角转全角：
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 32)
                {
                    c[i] = (char)12288;
                    continue;
                }
                if (c[i] < 127)
                    c[i] = (char)(c[i] + 65248);
            }
            return new string(c);
        }

        /// <summary>
        ///  转半角的函数(SBC case)
        /// </summary>
        /// <param name="input">输入</param>
        /// <returns></returns>
        public static string ToDBC(string input)
        {
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 12288)
                {
                    c[i] = (char)32;
                    continue;
                }
                if (c[i] > 65280 && c[i] < 65375)
                    c[i] = (char)(c[i] - 65248);
            }
            return new string(c);
        }

        /// <summary>
        /// 把字符串按照指定分隔符装成 List 去除重复
        /// </summary>
        /// <param name="o_str"></param>
        /// <param name="sepeater"></param>
        /// <returns></returns>
        public static List<string> GetSubStringList(string o_str, char sepeater)
        {
            List<string> list = new List<string>();
            string[] ss = o_str.Split(sepeater);
            foreach (string s in ss)
            {
                if (!string.IsNullOrEmpty(s) && s != sepeater.ToString())
                {
                    list.Add(s);
                }
            }
            return list;
        }


        #region 将字符串样式转换为纯字符串
        /// <summary>
        ///  将字符串样式转换为纯字符串
        /// </summary>
        /// <param name="StrList"></param>
        /// <param name="SplitString"></param>
        /// <returns></returns>
        public static string GetCleanStyle(string StrList, string SplitString)
        {
            string RetrunValue = "";
            //如果为空，返回空值
            if (StrList == null)
            {
                RetrunValue = "";
            }
            else
            {
                //返回去掉分隔符
                string NewString = "";
                NewString = StrList.Replace(SplitString, "");
                RetrunValue = NewString;
            }
            return RetrunValue;
        }
        #endregion

        #region 将字符串转换为新样式
        /// <summary>
        /// 将字符串转换为新样式
        /// </summary>
        /// <param name="StrList"></param>
        /// <param name="NewStyle"></param>
        /// <param name="SplitString"></param>
        /// <param name="Error"></param>
        /// <returns></returns>
        public static string GetNewStyle(string StrList, string NewStyle, string SplitString, out string Error)
        {
            string ReturnValue = "";
            //如果输入空值，返回空，并给出错误提示
            if (StrList == null)
            {
                ReturnValue = "";
                Error = "请输入需要划分格式的字符串";
            }
            else
            {
                //检查传入的字符串长度和样式是否匹配,如果不匹配，则说明使用错误。给出错误信息并返回空值
                int strListLength = StrList.Length;
                int NewStyleLength = GetCleanStyle(NewStyle, SplitString).Length;
                if (strListLength != NewStyleLength)
                {
                    ReturnValue = "";
                    Error = "样式格式的长度与输入的字符长度不符，请重新输入";
                }
                else
                {
                    //检查新样式中分隔符的位置
                    string Lengstr = "";
                    for (int i = 0; i < NewStyle.Length; i++)
                    {
                        if (NewStyle.Substring(i, 1) == SplitString)
                        {
                            Lengstr = Lengstr + "," + i;
                        }
                    }
                    if (Lengstr != "")
                    {
                        Lengstr = Lengstr.Substring(1);
                    }
                    //将分隔符放在新样式中的位置
                    string[] str = Lengstr.Split(',');
                    foreach (string bb in str)
                    {
                        StrList = StrList.Insert(int.Parse(bb), SplitString);
                    }
                    //给出最后的结果
                    ReturnValue = StrList;
                    //因为是正常的输出，没有错误
                    Error = "";
                }
            }
            return ReturnValue;
        }
        #endregion

        /// <summary>
        /// 分割字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="splitstr"></param>
        /// <returns></returns>
        public static string[] SplitMulti(string str, string splitstr)
        {
            string[] strArray = null;
            if ((str != null) && (str != ""))
            {
                strArray = new Regex(splitstr).Split(str);
            }
            return strArray;
        }
        public static string SqlSafeString(string String, bool IsDel)
        {
            if (IsDel)
            {
                String = String.Replace("'", "");
                String = String.Replace("\"", "");
                return String;
            }
            String = String.Replace("'", "&#39;");
            String = String.Replace("\"", "&#34;");
            return String;
        }

        #region 获取正确的Id，如果不是正整数，返回0
        /// <summary>
        /// 获取正确的Id，如果不是正整数，返回0
        /// </summary>
        /// <param name="_value"></param>
        /// <returns>返回正确的整数ID，失败返回0</returns>
        public static int StrToId(string _value)
        {
            if (IsNumberId(_value))
                return int.Parse(_value);
            else
                return 0;
        }
        #endregion
        #region 检查一个字符串是否是纯数字构成的，一般用于查询字符串参数的有效性验证。
        /// <summary>
        /// 检查一个字符串是否是纯数字构成的，一般用于查询字符串参数的有效性验证。(0除外)
        /// </summary>
        /// <param name="_value">需验证的字符串。。</param>
        /// <returns>是否合法的bool值。</returns>
        public static bool IsNumberId(string _value)
        {
            return QuickValidate("^[1-9]*[0-9]*$", _value);
        }
        #endregion
        #region 快速验证一个字符串是否符合指定的正则表达式。
        /// <summary>
        /// 快速验证一个字符串是否符合指定的正则表达式。
        /// </summary>
        /// <param name="_express">正则表达式的内容。</param>
        /// <param name="_value">需验证的字符串。</param>
        /// <returns>是否合法的bool值。</returns>
        public static bool QuickValidate(string _express, string _value)
        {
            if (_value == null) return false;
            Regex myRegex = new Regex(_express);
            if (_value.Length == 0)
            {
                return false;
            }
            return myRegex.IsMatch(_value);
        }
        #endregion

        /// <summary>
        /// 字符串如果操过指定长度则将超出的部分用指定字符串代替
        /// </summary>
        /// <param name="p_SrcString">要检查的字符串</param>
        /// <param name="p_Length">指定长度</param>
        /// <param name="p_TailString">用于替换的字符串</param>
        /// <returns>截取后的字符串</returns>
        public static string GetSubString(string p_SrcString, int p_Length, string p_TailString)
        {
            return GetSubString(p_SrcString, 0, p_Length, p_TailString);
        }

        public static string GetUnicodeSubString(string str, int len, string p_TailString)
        {
            string result = string.Empty;// 最终返回的结果
            int byteLen = System.Text.Encoding.Default.GetByteCount(str);// 单字节字符长度
            int charLen = str.Length;// 把字符平等对待时的字符串长度
            int byteCount = 0;// 记录读取进度
            int pos = 0;// 记录截取位置
            if (byteLen > len)
            {
                for (int i = 0; i < charLen; i++)
                {
                    if (Convert.ToInt32(str.ToCharArray()[i]) > 255)// 按中文字符计算加2
                        byteCount += 2;
                    else// 按英文字符计算加1
                        byteCount += 1;
                    if (byteCount > len)// 超出时只记下上一个有效位置
                    {
                        pos = i;
                        break;
                    }
                    else if (byteCount == len)// 记下当前位置
                    {
                        pos = i + 1;
                        break;
                    }
                }

                if (pos >= 0)
                    result = str.Substring(0, pos) + p_TailString;
            }
            else
                result = str;

            return result;
        }

        /// <summary>
        /// 取指定长度的字符串
        /// </summary>
        /// <param name="p_SrcString">要检查的字符串</param>
        /// <param name="p_StartIndex">起始位置</param>
        /// <param name="p_Length">指定长度</param>
        /// <param name="p_TailString">用于替换的字符串</param>
        /// <returns>截取后的字符串</returns>
        public static string GetSubString(string p_SrcString, int p_StartIndex, int p_Length, string p_TailString)
        {
            string myResult = p_SrcString;

            Byte[] bComments = Encoding.UTF8.GetBytes(p_SrcString);
            foreach (char c in Encoding.UTF8.GetChars(bComments))
            {    //当是日文或韩文时(注:中文的范围:\u4e00 - \u9fa5, 日文在\u0800 - \u4e00, 韩文为\xAC00-\xD7A3)
                if ((c > '\u0800' && c < '\u4e00') || (c > '\xAC00' && c < '\xD7A3'))
                {
                    //if (System.Text.RegularExpressions.Regex.IsMatch(p_SrcString, "[\u0800-\u4e00]+") || System.Text.RegularExpressions.Regex.IsMatch(p_SrcString, "[\xAC00-\xD7A3]+"))
                    //当截取的起始位置超出字段串长度时
                    if (p_StartIndex >= p_SrcString.Length)
                        return "";
                    else
                        return p_SrcString.Substring(p_StartIndex,
                                                       ((p_Length + p_StartIndex) > p_SrcString.Length) ? (p_SrcString.Length - p_StartIndex) : p_Length);
                }
            }

            if (p_Length >= 0)
            {
                byte[] bsSrcString = Encoding.Default.GetBytes(p_SrcString);

                //当字符串长度大于起始位置
                if (bsSrcString.Length > p_StartIndex)
                {
                    int p_EndIndex = bsSrcString.Length;

                    //当要截取的长度在字符串的有效长度范围内
                    if (bsSrcString.Length > (p_StartIndex + p_Length))
                    {
                        p_EndIndex = p_Length + p_StartIndex;
                    }
                    else
                    {   //当不在有效范围内时,只取到字符串的结尾

                        p_Length = bsSrcString.Length - p_StartIndex;
                        p_TailString = "";
                    }

                    int nRealLength = p_Length;
                    int[] anResultFlag = new int[p_Length];
                    byte[] bsResult = null;

                    int nFlag = 0;
                    for (int i = p_StartIndex; i < p_EndIndex; i++)
                    {
                        if (bsSrcString[i] > 127)
                        {
                            nFlag++;
                            if (nFlag == 3)
                                nFlag = 1;
                        }
                        else
                            nFlag = 0;

                        anResultFlag[i] = nFlag;
                    }

                    if ((bsSrcString[p_EndIndex - 1] > 127) && (anResultFlag[p_Length - 1] == 1))
                        nRealLength = p_Length + 1;

                    bsResult = new byte[nRealLength];

                    Array.Copy(bsSrcString, p_StartIndex, bsResult, 0, nRealLength);

                    myResult = Encoding.Default.GetString(bsResult);
                    myResult = myResult + p_TailString;
                }
            }

            return myResult;
        }

        /// <summary>
        /// 自定义的替换字符串函数
        /// </summary>
        public static string ReplaceString(string SourceString, string SearchString, string ReplaceString, bool IsCaseInsensetive)
        {
            return Regex.Replace(SourceString, Regex.Escape(SearchString), ReplaceString, IsCaseInsensetive ? RegexOptions.IgnoreCase : RegexOptions.None);
        }
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


   
        #region 删除最后一个字符之后的字符

    
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
