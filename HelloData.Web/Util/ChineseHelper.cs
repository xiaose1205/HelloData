using System;
using System.Collections.Generic;
using System.Text;

namespace HelloData.Util
{
   public class ChineseHelper
    {
        
        /// <summary>
        /// 汉字
        /// </summary>
        protected static string[] phoneticString;
        /// <summary>
        /// 汉语拼音
        /// </summary>
        protected static string[] phoneticValue;


        /// <summary>
        /// 构造
        /// </summary>
        public ChineseHelper()
        { }

        /// <summary>
        /// 将汉字转换为拼音
        /// </summary>
        /// <param name="character">汉字</param>
        /// <returns>拼音</returns>
        public static string ConvertPhonetic(string character)
        {
            //byte[] bytes = new byte[2];
            //string str = "";
            //int num = 0;
            //int num2 = 0;
            //int num3 = 0;
            //character = character.Trim();
            //string[] strArray = character.Split(new char[] { ' ' });
            //character = "";
            //for (int i = 0; i < strArray.Length; i++)
            //{
            //    character = character + strArray[i].ToString().Trim();
            //}
            //string str2 = "";
            //string str3 = "";
            //foreach (char ch in character)
            //{
            //    if ((ch >= '一') && (ch <= 0x9fa5))
            //    {
            //        str2 = str2 + ch;
            //    }
            //    else
            //    {
            //        str3 = str3 + ch;
            //    }
            //}
            //character = str2;
            //char[] chArray = character.ToCharArray();
            //for (int j = 0; j < chArray.Length; j++)
            //{
            //    bytes = Encoding.Default.GetBytes(chArray[j].ToString());
            //    num2 = bytes[0];
            //    num3 = bytes[1];
            //    num = ((num2 * 0x100) + num3) - 0x10000;
            //    if ((num > 0) && (num < 160))
            //    {
            //        str = str + chArray[j];
            //    }
            //    else
            //    {
            //        for (int k = 0; k < phoneticValue.Length; k++)
            //        {
            //            if (phoneticString[k].ToString().IndexOf(chArray[j].ToString()) >= 0)
            //            {
            //                str = str + phoneticValue[k];
            //                break;
            //            }
            //        }
            //    }
            //}
            //return (str + str3);
            return string.Empty;
        }
    }
}
