using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HelloData.FrameWork.Outport
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

            ExportFileType = ExportFileType.Txt;
            FileExtension = ".txt";
            Separator = ",";

        }
        public void WriteRow(List<string> cellvalue)
        {
            using (TextWriter writer = new StreamWriter(GetFullPath(), true, Encoding.Default))
            {
                string[] strs = new string[cellvalue.Count];
                for (int i = 0; i < cellvalue.Count; i++)
                {
                    strs[i] = cellvalue[i];
                }
                writer.WriteLine(string.Join(Separator, strs));
            }
        }
        public void WriteRow(List<string> cellvalue, Encoding encoding)
        {
            Encoding = encoding;
            using (TextWriter writer = new StreamWriter(GetFullPath(), true, encoding))
            {
                string[] strs = new string[cellvalue.Count];
                for (int i = 0; i < cellvalue.Count; i++)
                {
                    strs[i] = cellvalue[i];
                }
                writer.WriteLine(string.Join(Separator, strs));
            }
        }
        public void WriteContent(List<string> rowValue)
        {
            using (TextWriter writer = new StreamWriter(GetFullPath(), true, Encoding.Default))
            {
                foreach (string str in rowValue)
                {
                    writer.WriteLine(str);
                }
            }
        }
        public void WriteContent(List<string> rowValue, Encoding encoding)
        {
            Encoding = encoding;
            using (TextWriter writer = new StreamWriter(GetFullPath(), true, encoding))
            {
                foreach (string str in rowValue)
                {
                    writer.WriteLine(str);
                }
            }
        }
    }
}
