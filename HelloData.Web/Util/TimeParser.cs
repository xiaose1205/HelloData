using System;
using System.Collections.Generic;
using System.Text;

namespace HelloData.Util
{
    public class TimeParser
    {
        /// <summary>
        /// 把秒转换成分钟
        /// </summary>
        /// <returns></returns>
        public static int SecondToMinute(int Second)
        {
            decimal mm = (decimal)((decimal)Second / (decimal)60);
            return Convert.ToInt32(Math.Ceiling(mm));           
        }
        /// <summary>
        /// 距离当前几天几小时
        /// </summary>
        /// <param name="date_time">The date_time.</param>
        /// <returns></returns>
        public static string TimeNonce(string date_time)
        {
            try
            {
                DateTime dt1 = DateTime.Now;
                DateTime dt2 = Convert.ToDateTime(date_time);
                TimeSpan ts = dt1 - dt2;
                if (ts.TotalHours > 24)
                {
                    return Convert.ToInt32(ts.TotalDays).ToString() + "天" + Convert.ToInt32(ts.TotalHours - 24).ToString() + "小时";
                }
                else { return Convert.ToInt32(ts.TotalHours).ToString() + "小时"; }
            }
            catch { return "未知"; }
        }
    }
}
