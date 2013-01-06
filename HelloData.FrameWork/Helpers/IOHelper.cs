namespace HelloData.FrameWork.Helpers
{
    using System;
    using System.IO;
    using System.Text;

    public class IOHelper
    {
        public static void FileCopy(string sourceFileName, string destFileName)
        {
            // This item is obfuscated and can not be translated.
        }

        public static bool FileExists(string filename)
        {
            return File.Exists(filename);
        }

        public static void FileMove(string sourceFileName, string destFileName)
        {
            //IdentifyEncoding sinodetector = new IdentifyEncoding();
            //Encoding encoding = sinodetector.GetFileEncoding(PathFile);
        }

        public static Encoding GetEncoding(FileStream stream, Encoding defaultEncoding)
        {
            throw new Exception();
            //IdentifyEncoding sinodetector = new IdentifyEncoding();
            //Encoding encoding = sinodetector.GetFileEncoding(PathFile);
        }

        public static string GetExtension(string filename)
        {
            return Path.GetExtension(filename);
        }

        public static string MD5HashFile(string FileName, int blocksize)
        {
            throw new Exception();
            // This item is obfuscated and can not be translated.
        }
    }
}

