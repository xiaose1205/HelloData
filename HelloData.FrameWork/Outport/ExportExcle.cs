using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Aspose.Cells;

namespace HelloData.FrameWork.Outport
{
    public class ExportExcle : ExportBase
    {
        Workbook workbook = new Workbook();
        public ExportExcle()
            : this(DateTime.Now.ToString("yyyyMMddHHmmss"))
        {
        }
        public ExportExcle(string fileName)
            : base(fileName)
        { workbook.Worksheets.Add();
            ExportFileType = ExportFileType.Xls;
            FileExtension = ".xls";
          
        }
        public void WriteRow(List<string> cellvalue)
        {
            workbook.Open(GetFullPath(), FileFormatType.Excel97To2003);
            var worksheet = workbook.Worksheets[0];
            string[] itemValues = new string[cellvalue.Count];
            for (int i = 0; i < cellvalue.Count; i++)
            {
                itemValues[i] = cellvalue[i];
            }
            if (itemValues != null)
            {
                worksheet.Cells.ImportObjectArray(itemValues, worksheet.Cells.Rows.Count, 0, false);
            }
            workbook.Save(GetFullPath(), FileFormatType.Excel97To2003);
        }
        public void WriteRow(string[] itemValues)
        {
            workbook.Open(GetFullPath(), FileFormatType.Excel97To2003);
            var worksheet = workbook.Worksheets[0];
            if (itemValues != null)
            {
                worksheet.Cells.ImportObjectArray(itemValues, worksheet.Cells.Rows.Count, 0, false);
            }
            workbook.Save(GetFullPath(), FileFormatType.Excel97To2003);
        }
        public void WriteContent(List<string[]> itemValues)
        {
            workbook.Open(GetFullPath(), FileFormatType.Excel97To2003);
            var worksheet = workbook.Worksheets[0];
            if (itemValues != null)
            {
                if (itemValues.Count > 0)
                {
                    var length = itemValues[0].Length;
                    var array = new object[itemValues.Count,length];
                    for (int i = 0; i < itemValues.Count; i++)
                    {
                        for (int j = 0; j < length; j++)
                        {
                            array[i, j] = itemValues[i][j];
                        }
                    }
                    worksheet.Cells.ImportTwoDimensionArray(array, worksheet.Cells.Rows.Count, 0, false);
                }
            }
            workbook.Save(GetFullPath(), FileFormatType.Excel97To2003);
        }
    }
}
