#region Version Info
/* ========================================================================
* 【本类功能概述】
* 
* 作者：王军 时间：2013/1/1 12:34:11
* 文件名：EncodeHelper
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

namespace HelloData.FWCommon.Utils
{
    public class EncodeHelper
    {
        /// <summary>
        ///    汉字转为Unicode编码： 
        /// </summary>
        /// <param name="chinese"></param>
        /// <returns></returns>
        public static string HzToUnicode(string chinese)
        {
            //汉字转为Unicode编码： 
            byte[] b = Encoding.Unicode.GetBytes(chinese);
            string o = string.Empty;
            foreach (var x in b)
            {
                o += string.Format("{0:X2}", x) + " ";
            }
            return o;
        }
        /// <summary>
        /// Unicode编码转为汉字
        /// </summary>
        /// <param name="unicodeStr"></param>
        /// <returns></returns>
        public static string UnicodeToHz(string unicodeStr)
        {
            string cd2 = unicodeStr.Replace(" ", "");
            cd2 = cd2.Replace("\r", "");
            cd2 = cd2.Replace("\n", "");
            cd2 = cd2.Replace("\r\n", "");
            cd2 = cd2.Replace("\t", "");
            if (cd2.Length % 4 != 0)
            {
                throw new Exception("Unicode编码为双字节，请删多或补少！确保是二的倍数。");
            }
            else
            {
                int len = cd2.Length / 2;
                byte[] b = new byte[len];
                for (int i = 0; i < cd2.Length; i += 2)
                {
                    string bi = cd2.Substring(i, 2);
                    b[i / 2] = (byte)Convert.ToInt32(bi, 16);
                }
                return Encoding.Unicode.GetString(b);
            }
        }
        /// <summary>
        /// Unicode十六进制码转成汉字
        /// </summary>
        /// <param name="unicode16"></param>
        /// <returns></returns>
        public static string Unicode16ToHz(string text)
        {
            //string[] b5 = unicode16.Split(' ');
            //byte[] bs = new byte[2];
            //bs[0] = (byte)Convert.ToByte(b5[0], 16);
            //bs[1] = (byte)Convert.ToByte(b5[1], 16);
            //return Encoding.GetEncoding("Unicode").GetString(bs);
            System.Text.RegularExpressions.MatchCollection mc = System.Text.RegularExpressions.Regex.Matches(text, "\\\\u([\\w]{4})");

            string a = text.Replace("\\u", "");

            char[] arr = new char[mc.Count];

            for (int i = 0; i < arr.Length; i++)
            {

                arr[i] = (char)Convert.ToInt32(a.Substring(i * 4, 4), 16);

            }

            string c = new string(arr);

            return c;
        }
        /// <summary>
        /// Unicode十六进制码转成汉字
        /// </summary>
        /// <param name="unicode16"></param>
        /// <returns></returns>
        public static string Utf8_16ToHz(string utf8_16)
        {
            string[] b6 = utf8_16.Split(' ');
            byte[] bs = new byte[3];
            bs[0] = (byte)Convert.ToByte(b6[0], 16);
            bs[1] = (byte)Convert.ToByte(b6[1], 16);
            bs[2] = (byte)Convert.ToByte(b6[2], 16);
            return Encoding.GetEncoding("UTF-8").GetString(bs);
        }
    }
}
