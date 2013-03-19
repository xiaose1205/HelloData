using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace HelloData.FrameWork
{
    public class HandlerResult
    {
        public HandlerResult()
        {
            PostTime = DateTime.Now;
        }
        /// <summary>
        ///   结果
        /// </summary>
        public int Result { get; set; }
        /// <summary>
        /// 结果信息（可存放实体类等）
        /// </summary>
        public object Message { get; set; }
        public new string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        /// <summary>
        /// 执行时间
        /// </summary>
        public DateTime PostTime
        {
            get;
            set;
        }

        public HandlerResult DefaultResult()
        {
            return new HandlerResult() { Result = 0, Message = "当前请求处理失败" };
        }
    }
}
