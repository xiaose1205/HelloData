using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace HelloData.FWCommon
{
    public class HandlerResponse
    {
        public HandlerResponse()
        {
            PostTime = DateTime.Now;
        }
        /// <summary>
        ///   结果 1表示成功，0表示失败，其余参数可以自定义
        /// </summary>
        public int Result { get; set; }
        /// <summary>
        /// 结果信息（可存放实体类等）
        /// </summary>
        public object Message { get; set; }
        public new string ToString()
        {

            return FWCommon.JsonHelper.SerializeObject(this);
        }
        /// <summary>
        /// 执行时间
        /// </summary>
        public DateTime PostTime
        {
            get;
            set;
        }
        /// <summary>
        /// 获取默认的请求
        /// </summary>
        /// <returns></returns>
        public HandlerResponse GetDefaultResponse()
        {
            return new HandlerResponse() { Result = 0, Message = "当前请求处理失败" };
        }
    }
}
