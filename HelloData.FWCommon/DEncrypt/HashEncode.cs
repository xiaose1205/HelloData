#region Version Info
/* ========================================================================
* 【本类功能概述】
* 
* 作者：王军 时间：2013/1/6 16:26:50
* 文件名：HashEncode
* 版本：V1.0.1
* 联系方式：511522329  
*
* 修改者： 时间： 
* 修改说明：
* ========================================================================
*/
#endregion

using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace HelloData.FWCommon.DEncrypt
{
    public class HashEncode
    {
        /// <summary>
        /// 得到随机哈希加密字符串
        /// </summary>
        /// <returns></returns>
        public static string GetSecurity()
        {
            return HashEncoding(GetRandomValue());
        }
        /// <summary>
        /// 得到一个随机数值
        /// </summary>
        /// <returns></returns>
        public static string GetRandomValue()
        {
            Random Seed = new Random();
            return Seed.Next(1, int.MaxValue).ToString();
        }
        /// <summary>
        /// 哈希加密一个字符串
        /// </summary>
        /// <param name="Security"></param>
        /// <returns></returns>
        public static string HashEncoding(string Security)
        {
            UnicodeEncoding code = new UnicodeEncoding();
            byte[] message = code.GetBytes(Security);
            SHA512Managed arithmetic = new SHA512Managed();
            byte[] value = arithmetic.ComputeHash(message);
            return value.Aggregate("", (current, o) => current + ((int)o + "O"));
        }
    }
}
