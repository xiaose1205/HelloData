using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HelloData.FWCommon.ExportUtils
{
    public class ExportTxt : ExportBase
    {
        public ExportTxt()
            : this(DateTime.Now.ToString("yyyyMMddHHmmss"))
        {

        }
        public ExportTxt(string fileName)
            : base(fileName)
        {
            FileName = fileName;
            ExportFileType = ExportFileType.Txt;
            FileExtension = ".txt";
            Separator = ",";

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
     
        public void WriteContent(List<object> rowValue)
        {
            using (TextWriter writer = new StreamWriter(GetFullPath(), true, Encoding))
            {
                foreach (string str in rowValue)
                {
                    writer.WriteLine(str);
                }
            }
        } 
    }
}
