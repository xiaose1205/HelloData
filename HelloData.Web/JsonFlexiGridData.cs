using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HelloData.Web
{
    /// <summary>
    /// flexigrid  json格式
    /// </summary>
    public class JsonFlexiGridData
    {
        public JsonFlexiGridData()
        {
        }
        public JsonFlexiGridData(
            int pageIndex, int totalCount, IList<FlexiGridRow> data)
        {
            page = pageIndex;
            total = totalCount;
            rows = data;
        }
        public int page { get; set; }
        public int total { get; set; }
        public IList<FlexiGridRow> rows { get; set; }
        public FlexiGridError error { get; set; }
    }
    public class FlexiGridError
    {
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
    }
    public class FlexiGridRow
    {
        public string id { get; set; }
        public Dictionary<string, object> cell { get; set; }
    }
}
