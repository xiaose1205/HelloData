using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HelloData.FWCommon.ExportUtils
{
    public abstract class ExportBase
    {
        protected ExportBase()
        {

        }
        protected ExportBase(string fileName)
        {
            FileName = fileName;
            FilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "fileload");
        }
        /// <summary>
        /// 文件的路径
        /// </summary>
        public string FilePath { get; set; }
        /// <summary>
        /// 文本文件的分隔符
        /// </summary>
        public string Separator { get; set; }
        /// <summary>
        /// 文件名（无需加后缀）
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 导出类型
        /// </summary>
        public ExportFileType ExportFileType { get; set; }
        /// <summary>
        /// 文件的后缀名
        /// </summary>
        public string FileExtension { get; set; }
        /// <summary>
        /// 文件的编码
        /// </summary>
        public Encoding Encoding { get; set; }
        /// <summary>
        /// 保存文件
        /// </summary>
        public virtual void Save()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 保存文件流到文件
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="encoding"></param>
        public virtual void Save(Stream stream, Encoding encoding)
        {

        }
        /// <summary>
        /// 存入zip压缩文件
        /// </summary>
        public virtual void SaveToZip()
        {

        }
        /// <summary>
        /// 文件加入内存
        /// </summary>
        /// <returns></returns>
        public virtual MemoryStream SaveToStream()
        {
            using (FileStream stream = File.OpenRead(FilePath))
            {
                using (new StreamReader(stream, Encoding, false))
                {
                    MemoryStream memoryStream = new MemoryStream();
                    const int bufferLength = 1024;
                    int actual;
                    byte[] buffer = new byte[bufferLength];
                    while ((actual = stream.Read(buffer, 0, bufferLength)) > 0)
                        memoryStream.Write(buffer, 0, actual);
                    return memoryStream;
                }
            }
        }
        /// <summary>
        /// 路径的web完整
        /// </summary>
        /// <returns></returns>
        public virtual string GetWebPath(string pre)
        {
            string filefullpath = Path.Combine(pre, FileName + FileExtension);
            return filefullpath;

        }
        /// <summary>
        /// 路径的完整
        /// </summary>
        /// <returns></returns>
        public virtual string GetFullPath()
        {
            string filefullpath = Path.Combine(FilePath, FileName + FileExtension);
            if (!Directory.Exists(FilePath))
            {
                Directory.CreateDirectory(FilePath);
            }
            if (!File.Exists(filefullpath))
            {
                FileStream fs = File.Create(filefullpath);
                fs.Close();
            }
            if (File.Exists(filefullpath))
                return filefullpath;
            return string.Empty;
        }
    }
}
