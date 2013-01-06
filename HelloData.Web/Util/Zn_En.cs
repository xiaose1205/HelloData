using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace HelloData.Util
{
    /// <summary>
    /// 自定义部分数据
    /// </summary>
    public class Zn_En
    {
        /// <summary>
        /// Fies the specified en.
        /// </summary>
        /// <param name="En">The en.</param>
        /// <returns></returns>
        public static string Fy(string En)
        { 
            En = En.Replace("类别", "DdlMenu");
            En = En.Replace("标题", "txttitle");
            En = En.Replace("推荐", "tuijian");
            En = En.Replace("开关", "switchs");
            En = En.Replace("出处", "txtinfor");
            En = En.Replace("作者", "txtauthor");
            En = En.Replace("浏览量", "txthits");
            En = En.Replace("顶次数", "txtdig");
            En = En.Replace("关键字", "keyword");
            En = En.Replace("描述", "txtzhaiyao");
            En = En.Replace("形象图", "txtpic");
            En = En.Replace("地址", "txturl");
            En = En.Replace("标签", "txttag");
            En = En.Replace("内容", "FCKeditor1");
            return En;
        }
    }
}
