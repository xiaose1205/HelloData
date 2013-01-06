#region Version Info
/* ========================================================================
* 【本类功能概述】
* 
* 作者：王军 时间：2013/1/6 16:35:54
* 文件名：MD5Encrypt
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
using System.Security.Cryptography;
using System.Text;

namespace HelloData.FWCommon.DEncrypt
{
    public class MD5Encrypt
    {
         private MD5 md5;

        /// <summary>
        /// 构造方法.
        /// </summary>
         public MD5Encrypt()
        {
            md5 = MD5.Create();
        }

        #region [加密接口成员]

        /// <summary>
        /// 加密指定字节数组.
        /// </summary>
        /// <param name="plainBytes"></param>
        /// <returns></returns>
        public byte[] Encrypt(byte[] plainBytes)
        {
            return md5.ComputeHash(plainBytes);
        }

        /// <summary>
        /// 加密指定字符串.
        /// </summary>
        /// <param name="plainText"></param>
        /// <returns></returns>
        public string Encrypt(string plainText)
        {
            byte[] data = md5.ComputeHash(Encoding.Default.GetBytes(plainText));

            StringBuilder resultBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
                resultBuilder.Append(data[i].ToString("x2"));
            return resultBuilder.ToString();
        }


        #endregion [加密接口成员]
    }
}
