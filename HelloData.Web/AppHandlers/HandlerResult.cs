using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HelloData.FrameWork;

namespace HelloData.AppHandlers
{
    public class HandlerResult
    {
        public HandlerResult()
        {
            PostTime = DateTime.Now;
        }
        public int Result { get; set; }
        public object Message { get; set; }
        public new string ToString()
        {
            return JsonHelper.SerializeObject(this);
        }
        /// <summary>
        /// 执行时间
        /// </summary>
        public DateTime PostTime
        {
            get;
            set;
        }
    }
}
