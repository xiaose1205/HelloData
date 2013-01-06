using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HelloData.FrameWork.Outport
{
   public  class ExportCsv : ExportBase
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
