using System.Collections.Generic;
using HelloData.FWCommon;

namespace HelloData.FrameWork.Data
{
    /// <summary>
    /// 分页查询生成的对象集合
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PageList<T> : List<T>
    {
        /// <summary>
        /// 总数 select count()
        /// </summary>
        public int TotalCount { get; set; }
        /// <summary>
        /// 分页的页码数
        /// </summary>
        public int TotalPage { get; set; }
        /// <summary>
        /// 转换成json格式
        /// </summary>
        /// <returns></returns>
        public string ToJsonString()
        {
            return JsonHelper.SerializeObject(this);
        }
         
    }
}
