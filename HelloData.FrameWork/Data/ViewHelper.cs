using System.Collections.Generic;
using HelloData.FrameWork.Data.Enum;

namespace HelloData.FrameWork.Data
{
    /// <summary>
    /// 视图处理
    /// </summary>
    public class ViewHelper
    {
        public ViewHelper()
        {
            Joinfields = new List<QueryField>();
        }
        public ViewJoinEnum Join { get; set; }
        public string TableName1 { get; set; }
        public string TableName2 { get; set; }
        public List<QueryField> Joinfields { get; set; }
    }
}
