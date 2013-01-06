using System;
using System.Web;
using System.Text;
using System.Text.RegularExpressions;

namespace HelloData.Web.Util
{
    /// <summary>
    ///input 的摘要说明
    /// </summary>
    public class InputStr
    {



        public static string url(string name)
        {
            return HttpContext.Current.Server.UrlEncode(name);
        }


        /// <summary>
        /// 字符串分会数组
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string[] SubGroup(string content)
        {
            content = content.Replace('，', ',');
            content = CutComma(content);
            return content.Split(',');
        }
        /// <summary>
        /// 字符串截取
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string CutString(int sl, string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                if (name.Length > sl)
                {
                    name = name.Substring(0, sl);
                }
                return name;
            }
            return string.Empty;
        }

        /// <summary>
        /// 字符串截取
        /// </summary>
        /// <param name="sl"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string CutString(int sl, string name, string afterstr)
        {
            if (!string.IsNullOrEmpty(name))
            {
                if (name.Length > sl)
                {
                    name = name.Substring(0, sl) + afterstr;
                }
                return name;
            }
            return string.Empty;
        }

        /// <summary>
        /// 判断是否是字符型
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsSstring(object str)
        {
            string s = "";
            try
            {
                s = str.ToString();
                return s.Length > 0;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 判断是否是中文 Determines whether the specified C string is china.
        /// </summary>
        /// <param name="cString">The C string.</param>
        /// <returns>
        /// 	<c>true</c> if the specified C string is china; otherwise, <c>false</c>.
        /// </returns>
        public bool IsChina(string cString)
        {
            bool boolValue = false;
            for (int i = 0; i < cString.Length; i++)
            {
                return Convert.ToInt32(Convert.ToChar(cString.Substring(i, 1))) >= Convert.ToInt32(Convert.ToChar(128));
            }
            return false;
        }

        /// <summary>
        /// 检测是否整数型数据
        /// </summary>
        /// <param name="Num">待检查数据</param>
        /// <param name="input"> </param>
        /// <returns></returns>
        public static bool IsInteger(string input)
        {
            if (input == null)
            {
                return false;
            }
            return IsInteger(input, true);
        }

        /// <summary>
        /// 是否全是正整数
        /// </summary>
        /// <param name="input"></param>
        /// <param name="Plus"> </param>
        /// <returns></returns>
        public static bool IsInteger(string input, bool Plus)
        {
            if (input == null)
                return false;
            string pattern = "^-?[0-9]+$";
            if (Plus)
                pattern = "^[0-9]+$";
            if (Regex.Match(input, pattern, RegexOptions.Compiled).Success)
                return true;
            return false;
        }

        /// <summary>
        /// 判断输入是否为日期类型
        /// </summary>
        /// <param name="s">待检查数据</param>
        /// <returns></returns>
        public static bool IsDate(string s)
        {
            try
            {
                DateTime d = DateTime.Parse(s);
                return true;
            }
            catch
            {
                return false;
            }
        }


        /// <summary>
        /// 过滤字符串中的html代码
        /// </summary>
        /// <param name="str"></param>
        /// <returns>返回过滤之后的字符串</returns>
        public static string LostHTML(string str)
        {
            string reStr = "";
            if (str != null)
            {
                if (str != string.Empty)
                {
                    string Pattern = "<\\/*[^<>]*>";
                    reStr = Regex.Replace(str, Pattern, "");

                }
            }
            return (reStr.Replace("\\r\\n", "")).Replace("\\r", "").Replace("&nbsp;", "");
        }




        /// <summary>
        /// 截取字符串函数
        /// </summary>
        /// <param name="Str">所要截取的字符串</param>
        /// <param name="Num">截取字符串的长度</param>
        /// <returns></returns>
        public static string GetSubString(string Str, int Num)
        {
            if (Str == null || Str == "")
                return "";
            string outstr = "";
            int n = 0;
            foreach (char ch in Str)
            {
                n += System.Text.Encoding.Default.GetByteCount(ch.ToString());
                if (n > Num)
                    break;
                else
                    outstr += ch;
            }
            return outstr;
        }
        /// <summary>
        /// 截取字符串函数
        /// </summary>
        /// <param name="Str">所要截取的字符串</param>
        /// <param name="Num">截取字符串的长度</param>
        /// <param name="Num">截取字符串后省略部分的字符串</param>
        /// <returns></returns>
        public static string GetSubString(string Str, int Num, string LastStr)
        {
            return (Str.Length > Num) ? Str.Substring(0, Num) + LastStr : Str;
        }

        /// <summary>
        /// 验证字符串是否是图片路径
        /// </summary>
        /// <param name="Input">待检测的字符串</param>
        /// <returns>返回true 或 false</returns>
        public static bool IsImgString(string Input)
        {
            return IsImgString(Input, "/{@dirfile}/");
        }

        public static bool IsImgString(string Input, string checkStr)
        {
            bool re_Val = false;
            if (Input != string.Empty)
            {
                string s_input = Input.ToLower();
                if (s_input.IndexOf(checkStr.ToLower()) != -1 && s_input.IndexOf(".") != -1)
                {
                    string Ex_Name = s_input.Substring(s_input.LastIndexOf(".") + 1).ToString().ToLower();
                    if (Ex_Name == "jpg" || Ex_Name == "gif" || Ex_Name == "bmp" || Ex_Name == "png")
                    {
                        re_Val = true;
                    }
                }
            }
            return re_Val;
        }


        /// <summary>
        ///  将字符转化为HTML编码
        /// </summary>
        /// <param name="str">待处理的字符串</param>
        /// <returns></returns>
        public static string HtmlEncode(string Input)
        {
            return HttpContext.Current.Server.HtmlEncode(Input);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Input"></param>
        /// <returns></returns>
        public static string HtmlDecode(string Input)
        {
            return HttpContext.Current.Server.HtmlDecode(Input);
        }

        /// <summary>
        /// URL地址编码
        /// </summary>
        /// <param name="Input"></param>
        /// <returns></returns>
        public static string URLEncode(string Input)
        {
            return HttpContext.Current.Server.UrlEncode(Input);
        }

        /// <summary>
        /// URL地址解码
        /// </summary>
        /// <param name="Input"></param>
        /// <returns></returns>
        public static string URLDecode(string Input)
        {
            return HttpContext.Current.Server.UrlDecode(Input);
        }

        /// <summary>
        /// 过滤字符
        /// </summary>
        /// <param name="Input"></param>
        /// <returns></returns>
        public static string Filter(string sInput)
        {
            if (sInput == null || sInput == "")
                return null;
            string sInput1 = sInput.ToLower();
            string output = sInput;
            string pattern = @"*|and|exec|insert|select|delete|update|count|master|truncate|declare|char(|mid(|chr(|'";
            if (Regex.Match(sInput1, Regex.Escape(pattern), RegexOptions.Compiled | RegexOptions.IgnoreCase).Success)
            {
                throw new Exception("字符串中含有非法字符!");
            }
            else
            {
                output = output.Replace("'", "''");
            }
            return output;
        }

        /// <summary>
        /// 过滤特殊字符/前台会员
        /// </summary>
        /// <param name="Input"></param>
        /// <returns></returns>
        public static string Htmls(string Input)
        {
            if (Input != string.Empty && Input != null)
            {
                string ihtml = Input.ToLower();
                ihtml = ihtml.Replace("<script", "&lt;script");
                ihtml = ihtml.Replace("script>", "script&gt;");
                ihtml = ihtml.Replace("<%", "&lt;%");
                ihtml = ihtml.Replace("%>", "%&gt;");
                ihtml = ihtml.Replace("<$", "&lt;$");
                ihtml = ihtml.Replace("$>", "$&gt;");
                return ihtml;
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 字符串字符处理
        /// </summary>
        /// <param name="chr">等待处理的字符串</param>
        /// <returns>处理后的字符串</returns>
        /// //把HTML代码转换成TXT格式
        public static String ToTxt(String Input)
        {
            StringBuilder sb = new StringBuilder(Input);
            sb.Replace("&nbsp;", " ");
            sb.Replace("<br>", "\r\n");
            sb.Replace("<br>", "\n");
            sb.Replace("<br />", "\n");
            sb.Replace("<br />", "\r\n");
            sb.Replace("&lt;", "<");
            sb.Replace("&gt;", ">");
            sb.Replace("&amp;", "&");
            return sb.ToString();
        }

        /// <summary>
        /// 字符串字符处理
        /// </summary>
        /// <param name="chr">等待处理的字符串</param>
        /// <returns>处理后的字符串</returns>
        /// //把HTML代码转换成TXT格式
        public static String ToshowTxt(String Input)
        {
            StringBuilder sb = new StringBuilder(Input);
            sb.Replace("&lt;", "<");
            sb.Replace("&gt;", ">");
            return sb.ToString();
        }

        /// <summary>
        /// 把字符转化为文本格式
        /// </summary>
        /// <param name="Input"></param>
        /// <returns></returns>
        public static string ForTXT(string Input)
        {
            StringBuilder sb = new StringBuilder(Input);
            sb.Replace("<font", " ");
            sb.Replace("<span", " ");
            sb.Replace("<style", " ");
            sb.Replace("<div", " ");
            sb.Replace("<p", "");
            sb.Replace("</p>", "");
            sb.Replace("<label", " ");
            sb.Replace("&nbsp;", " ");
            sb.Replace("<br>", "");
            sb.Replace("<br />", "");
            sb.Replace("<br />", "");
            sb.Replace("&lt;", "");
            sb.Replace("&gt;", "");
            sb.Replace("&amp;", "");
            sb.Replace("<", "");
            sb.Replace(">", "");
            return sb.ToString();
        }
        /// <summary>
        /// 字符串字符处理
        /// </summary>
        /// <param name="chr">等待处理的字符串</param>
        /// <returns>处理后的字符串</returns>
        /// //把TXT代码转换成HTML格式

        public static String ToHtml(string Input)
        {
            StringBuilder sb = new StringBuilder(Input);
            sb.Replace("&", "&amp;");
            sb.Replace("<", "&lt;");
            sb.Replace(">", "&gt;");
            sb.Replace("\r\n", "<br />");
            sb.Replace("\n", "<br />");
            sb.Replace("\t", " ");
            //sb.Replace(" ", "&nbsp;");
            return sb.ToString();
        }

        /// <summary>
        /// MD5加密字符串处理
        /// </summary>
        /// <param name="Half">加密是16位还是32位；如果为true为16位</param>
        /// <param name="Input">待加密码字符串</param>
        /// <returns></returns>
        public static string MD5(string Input, bool Half)
        {
            string output = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(Input, "MD5").ToLower();
            if (Half)//16位MD5加密（取32位加密的9~25字符）
                output = output.Substring(8, 16);
            return output;
        }

        public static string MD5(string Input)
        {
            return MD5(Input, false);
        }

        /// <summary>
        /// 字符串加密  进行位移操作
        /// </summary>
        /// <param name="str">待加密数据</param>
        /// <returns>加密后的数据</returns>
        public static string EncryptString(string Input)
        {
            string _temp = "";
            int _inttemp;
            char[] _chartemp = Input.ToCharArray();
            for (int i = 0; i < _chartemp.Length; i++)
            {
                _inttemp = _chartemp[i] + 1;
                _chartemp[i] = (char)_inttemp;
                _temp += _chartemp[i];
            }
            return _temp;
        }

        /// <summary>
        /// 字符串解密
        /// </summary>
        /// <param name="str">待解密数据</param>
        /// <returns>解密成功后的数据</returns>
        public static string NcyString(string Input)
        {
            string _temp = "";
            int _inttemp;
            char[] _chartemp = Input.ToCharArray();
            for (int i = 0; i < _chartemp.Length; i++)
            {
                _inttemp = _chartemp[i] - 1;
                _chartemp[i] = (char)_inttemp;
                _temp += _chartemp[i];
            }
            return _temp;
        }

        /// <summary>
        /// 检测含中文字符串实际长度
        /// </summary>
        /// <param name="str">待检测的字符串</param>
        /// <returns>返回正整数</returns>
        public static int NumChar(string Input)
        {
            ASCIIEncoding n = new ASCIIEncoding();
            byte[] b = n.GetBytes(Input);
            int l = 0;
            for (int i = 0; i <= b.Length - 1; i++)
            {
                if (b[i] == 63)//判断是否为汉字或全脚符号
                {
                    l++;
                }
                l++;
            }
            return l;
        }

        /// <summary>
        /// 检测是否合法日期
        /// </summary>
        /// <param name="str">待检测的字符串</param>
        /// <returns></returns>
        public static bool ChkDate(string Input)
        {
            try
            {
                DateTime t1 = DateTime.Parse(Input);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 转换日期时间函数
        /// </summary>
        /// <returns></returns>        
        public static string ReDateTime()
        {
            return System.DateTime.Now.ToString("yyyyMMdd");
        }



        /// <summary>
        /// 去除字符串最后一个','号
        /// </summary>
        /// <param name="chr">:要做处理的字符串</param>
        /// <returns>返回已处理的字符串</returns>
        /// /// CreateTime:2007-03-26 Code By DengXi
        public static string CutComma(string Input)
        {
            return CutComma(Input, ",");
        }

        public static string CutComma(string Input, string indexStr)
        {
            if (Input.IndexOf(indexStr) >= 0)
                return Input.Remove(Input.LastIndexOf(indexStr));
            else
                return Input;
        }

        /// <summary>
        /// 去掉首尾P
        /// </summary>
        /// <param name="Input"></param>
        /// <returns></returns>
        public static string RemovePor(string Input)
        {
            if (Input != string.Empty && Input != null)
            {
                string TMPStr = Input;
                if (Input.ToLower().Substring(0, 3) == "<p>")
                {
                    TMPStr = TMPStr.Substring(3);
                }
                if (TMPStr.Substring(TMPStr.Length - 4) == "</p>")
                {
                    TMPStr = TMPStr.Remove(TMPStr.ToLower().LastIndexOf("</p>"));
                }
                return TMPStr;
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 判断参数是否合法
        /// </summary>
        /// <param name="ID">要判断的参数</param>
        /// <returns>返回已处理的字符串</returns>

        public static string checkID(string ID)
        {
            if (ID == null && ID == string.Empty)
                throw new Exception("参数传递错误!<li>参数不能为空</li>");
            return ID;
        }

        /// <summary>
        /// 去除编号字符串中的'-1'
        /// </summary>
        /// <param name="id"></param>
        /// <returns>如果为空则返回'IsNull'</returns>

        public static string Losestr(string id)
        {
            if (string.IsNullOrEmpty(id) || id == string.Empty)
                return "IsNull";

            id = id.Replace("'-1',", "");

            if (id == "" || id == string.Empty)
                return "IsNull";
            else
                return id;
        }

        public static string FilterHTML(string html)
        {
            if (html == null)
                return "";
            Regex regex1 = new Regex(@"<script[\s\S]+</script *>", RegexOptions.IgnoreCase);
            Regex regex2 = new Regex(@" href *= *[\s\S]*script *:", RegexOptions.IgnoreCase);
            Regex regex3 = new Regex(@" on[\s\S]*=", RegexOptions.IgnoreCase);
            Regex regex4 = new Regex(@"<iframe[\s\S]+</iframe *>", RegexOptions.IgnoreCase);
            Regex regex5 = new Regex(@"<frameset[\s\S]+</frameset *>", RegexOptions.IgnoreCase);
            Regex regex6 = new Regex(@"\<img[^\>]+\>", RegexOptions.IgnoreCase);
            Regex regex7 = new Regex(@"</p>", RegexOptions.IgnoreCase);
            Regex regex8 = new Regex(@"<p>", RegexOptions.IgnoreCase);
            Regex regex9 = new Regex(@"<[^>]*>", RegexOptions.IgnoreCase);
            html = regex1.Replace(html, ""); //过滤<script></script>标记
            html = regex2.Replace(html, ""); //过滤href=javascript: (<A>) 属性
            html = regex3.Replace(html, " _disibledevent="); //过滤其它控件的on...事件
            html = regex4.Replace(html, ""); //过滤iframe
            html = regex5.Replace(html, ""); //过滤frameset
            html = regex6.Replace(html, ""); //过滤frameset
            html = regex7.Replace(html, ""); //过滤frameset
            html = regex8.Replace(html, ""); //过滤frameset
            html = regex9.Replace(html, "");
            html = html.Replace(" ", "");
            html = html.Replace("</strong>", "");
            html = html.Replace("<strong>", "");
            return html;
        }


        /// <summary>
        /// 判断字符串是否图片地址
        /// </summary>
        /// <param name="inputStr"></param>
        /// <returns></returns>
        public static bool img(string URL)
        {
            bool x = imgto(URL, ".gif") || imgto(URL, ".jpg") || imgto(URL, ".jpeg") || imgto(URL, ".png");
            return x;
        }
        public static bool imgto(string URL, string type)
        {
            bool x = false;
            Regex r = new Regex(type); // 定义一个Regex对象实例
            Match m = r.Match(URL); // 在字符串中匹配
            if (m.Success)
            {
                x = true;
            }
            return x;
        }
    }

}