using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HelloData.FWCommon.ExportUtils
{
    public class ExportCsv : ExportBase
    {
        public ExportCsv()
            : this(DateTime.Now.ToString("yyyyMMddHHmmss"))
        {
        }
        public ExportCsv(string fileName)
            : base(fileName)
        {
            FileName = fileName;
            ExportFileType = ExportFileType.Txt;
            FileExtension = ".csv";
            Separator = ",";
            FilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "fileload");
        } 
        public void WriteRow(List<object> cellvalue)
        {
            using (TextWriter writer = new StreamWriter(GetFullPath(), true, Encoding))
            {
                string[] strs = new string[cellvalue.Count];
                for (int i = 0; i < cellvalue.Count; i++)
                {
                    strs[i] = cellvalue[i].ToString();
                }
                writer.WriteLine(string.Join(Separator, strs));
            }
        }
        /// <summary>
        /// 写入行数据
        /// </summary>
        /// <param name="rowValue"></param>
        public void WriteContent(List<object> rowValue)
        {
            using (TextWriter writer = new StreamWriter(GetFullPath(), true, Encoding))
            {
                foreach (object str in rowValue)
                {
                    writer.WriteLine(str);
                }
            }
        } 
    }
}
