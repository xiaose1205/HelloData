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
            Joinfields = new List<WhereField>();
        }
        public ViewJoinEnum Join { get; set; }
        public string TableName1 { get; set; }
        public string TableName2 { get; set; }
        public List<WhereField> Joinfields { get; set; }
    }
}
