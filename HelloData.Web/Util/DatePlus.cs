using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HelloData.Web.Common
{
    public class DatePlus
    {
        public static string ShowDatetime(DateTime dt)
        {
            if (dt.AddMinutes(+1) > DateTime.Now)
                return "刚刚";
            else if (dt.AddHours(+1) > DateTime.Now)
                return string.Format("{0}分钟前", (DateTime.Now - dt).Minutes);
            else if (dt.Day == DateTime.Now.Day)
            {
                return dt.ToString("今天 HH:mm:ss");
            }
            else if (dt.Day == DateTime.Now.AddDays(-1).Day)
            {
                return dt.ToString("昨天 HH:mm:ss");
            }
            else
            {
                return dt.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }
        public static string ShowDatetimeDate(DateTime dt)
        {
            if (dt.AddMinutes(+1) > DateTime.Now)
                return "刚刚";
            else if (dt.AddHours(+1) > DateTime.Now)
                return string.Format("{0}分钟前", (DateTime.Now - dt).Minutes);
            else if (dt.Day == DateTime.Now.Day)
            {
                return dt.ToString("今天");
            }
            else if (dt.Day == DateTime.Now.AddDays(-1).Day)
            {
                return dt.ToString("昨天");
            }
            else
            {
                return dt.ToString("yyyy-MM-dd");
            }
        }
        public static string ShowDatetime(DateTime? dt)
        {
            if (dt.HasValue)
                return ShowDatetime(dt.Value);
            else
            {
                return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }
        /// <summary>
        /// 显示到年月日时分秒
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string ShowDatetime(object  dt)
        {
            DateTime dtnow = DateTime.Now;
            DateTime.TryParse(dt.ToString(), out dtnow);
            return ShowDatetime(dtnow);
        }
        /// <summary>
        /// 显示年月日
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string ShowDatetimeDate(DateTime? dt)
        {
            if (dt.HasValue)
                return ShowDatetimeDate(dt.Value);
            else
            {
                return DateTime.Now.ToString("yyyy-MM-dd");
            }
        }

        public static string ShowDatetimeDate(object dt)
        {
            DateTime dtnow = DateTime.Now;
            DateTime.TryParse(dt.ToString(), out dtnow);
            return ShowDatetimeDate(dtnow);
        }
    }
}
