using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace HelloData.FrameWork.Outport
{
    interface IExport
    {

        /// <summary>
        /// 创建文件
        /// </summary> 
        /// <param name="fileType"> </param>
        void CreateFile(ExportFileType fileType);
        /// <summary>
        /// 创建文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="fileType"> </param>
        void CreateFile(string fileName, ExportFileType fileType);
        /// <summary>
        /// 创建行
        /// </summary>
        /// <param name="cellvalue"></param>
        void CreateRow(List<string> cellvalue);

        void Save(string fileName);

        void Save(Stream stream, Encoding encoding);
        /// <summary>
        /// 保存为流文件
        /// </summary>
        /// <returns></returns>
        MemoryStream SaveToStream();
    }
}
