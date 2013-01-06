using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Web;


namespace ROYcms.FileManager
{
    /// <summary>
    /// FileSystemManager
    /// </summary>
    public class FileSystemManager
    {
        private static string strRootFolder;

        static FileSystemManager()
        {
            strRootFolder = HttpContext.Current.Request.PhysicalApplicationPath;
            strRootFolder = strRootFolder.Substring(0, strRootFolder.LastIndexOf(@"\"));
        }

        /// <summary>
        /// 读根目录
        /// </summary>
        /// <returns></returns>
        public static string GetRootPath()
        {
            return strRootFolder;
        }

        /// <summary>
        /// 写根目录
        /// </summary>
        /// <param name="path"></param>
        public static void SetRootPath(string path)
        {
            strRootFolder = path;
        }

        /// <summary>
        /// 读取列表
        /// </summary>
        /// <returns></returns>
        public static List<FileSystemItem> GetItems()
        {
            return GetItems(strRootFolder);
        }

        /// <summary>
        /// 列出目录内的所有文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
         public static List<FileSystemItem> GetItemsAll(string path)
        {

            //FileInfo[] file = HelloData.Common.SystemCms.GetFilelist(path);
            List<FileSystemItem> list = new List<FileSystemItem>();
            //for (int i = 0; i < file.Length; i++)
            //{
            //    FileSystemItem item = new FileSystemItem();
            //    item.Name = file[i].Name.Replace(path,"");
            //    item.FullName = file[i].FullName;
            //    item.CreationDate = file[i].CreationTime;
            //    item.IsFolder = true;
            //    item.Size = file[i].Length;
            //    list.Add(item);
            //}
           



            DirectoryInfo UnPostil = new DirectoryInfo(path);
            FileInfo[] ArrUnPostil = UnPostil.GetFiles();
            
            foreach (FileInfo FileName in ArrUnPostil)
            {
                if (FileName.Length > 0)
                {
                    FileSystemItem item = new FileSystemItem();
                    item.Name = FileName.Name.Replace(path, "");
                    item.FullName = FileName.FullName;
                    item.CreationDate = FileName.CreationTime;
                    item.IsFolder = true;
                    item.Size = FileName.Length;
                    list.Add(item);
                }
            }



            return list;
         }
        /// <summary>
        /// 读取列表
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static List<FileSystemItem> GetItems(string path)
        {
            string[] folders = Directory.GetDirectories(path);
            string[] files = Directory.GetFiles(path);
            List<FileSystemItem> list = new List<FileSystemItem>();
            foreach (string s in folders)
            {
                FileSystemItem item = new FileSystemItem();
                DirectoryInfo di = new DirectoryInfo(s);
                item.Name = di.Name;
                item.FullName = di.FullName;
                item.CreationDate = di.CreationTime;
                item.IsFolder = true;
                list.Add(item);
            }
            foreach (string s in files)
            {
                FileSystemItem item = new FileSystemItem();
                FileInfo fi = new FileInfo(s);
                item.Name = fi.Name;
                item.FullName = fi.FullName;
                item.CreationDate = fi.CreationTime;
                item.IsFolder = true;
                item.Size = fi.Length;
                list.Add(item);
            }

            if (path.ToLower() != strRootFolder.ToLower())
            {
                FileSystemItem topitem = new FileSystemItem();
                DirectoryInfo topdi = new DirectoryInfo(path).Parent;
                topitem.Name = "[<b><font color=red>Superior</font></b>]";
                topitem.FullName = topdi.FullName;
                list.Insert(0, topitem);

                FileSystemItem rootitem = new FileSystemItem();
                DirectoryInfo rootdi = new DirectoryInfo(strRootFolder);
                rootitem.Name = "[<b><font color=red>Root</font></b>]";
                rootitem.FullName = rootdi.FullName;
                list.Insert(0, rootitem);

            }
            return list;
        }

        /// <summary>
        /// 读取文件夹
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parentName"></param>
        public static void CreateFolder(string name, string parentName)
        {
            DirectoryInfo di = new DirectoryInfo(parentName);
            di.CreateSubdirectory(name);
        }

        /// <summary>
        /// 删除文件夹
        /// </summary>
        /// <param name="path"></param>
        public static void DeleteFolder(string path)
        {
            Directory.Delete(path);
        }

        /// <summary>
        /// 移动文件夹
        /// </summary>
        /// <param name="oldPath"></param>
        /// <param name="newPath"></param>
        public static void MoveFolder(string oldPath, string newPath)
        {
            Directory.Move(oldPath, newPath);
        }

        /// <summary>
        /// 创建文件
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="path"></param>
        public static void CreateFile(string filename, string path)
        {
            FileStream fs = File.Create(path + "\\" + filename);
            fs.Close();
        }

        /// <summary>
        /// 创建文件
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="path"></param>
        /// <param name="contents"></param>
        public static void CreateFile(string filename, string path, byte[] contents)
        {
            FileStream fs = File.Create(path + "\\" + filename);
            fs.Write(contents, 0, contents.Length);
            fs.Close();
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="path"></param>
        public static void DeleteFile(string path)
        {
            File.Delete(path);
        }

        /// <summary>
        /// 移动文件
        /// </summary>
        /// <param name="oldPath"></param>
        /// <param name="newPath"></param>
        public static void MoveFile(string oldPath, string newPath)
        {
            File.Move(oldPath, newPath);
        }

        /// <summary>
        /// 读取文件信息
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static FileSystemItem GetItemInfo(string path)
        {
            FileSystemItem item = new FileSystemItem();
            if (Directory.Exists(path))
            {
                DirectoryInfo di = new DirectoryInfo(path);
                item.Name = di.Name;
                item.FullName = di.FullName;
                item.CreationDate = di.CreationTime;
                item.IsFolder = true;
                item.LastAccessDate = di.LastAccessTime;
                item.LastWriteDate = di.LastWriteTime;
                item.FileCount = di.GetFiles().Length;
                item.SubFolderCount = di.GetDirectories().Length;
            }
            else
            {
                FileInfo fi = new FileInfo(path);
                item.Name = fi.Name;
                item.FullName = fi.FullName;
                item.CreationDate = fi.CreationTime;
                item.LastAccessDate = fi.LastAccessTime;
                item.LastWriteDate = fi.LastWriteTime;
                item.IsFolder = false;
                item.Size = fi.Length;
            }
            return item;
        }

        /// <summary>
        /// 复制文件夹
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        public static void CopyFolder(string source, string destination)
        {
            String[] files;
            if (destination[destination.Length - 1] != Path.DirectorySeparatorChar)
                destination += Path.DirectorySeparatorChar;
            if (!Directory.Exists(destination)) Directory.CreateDirectory(destination);
            files = Directory.GetFileSystemEntries(source);
            foreach (string element in files)
            {
                if (Directory.Exists(element))
                    CopyFolder(element, destination + Path.GetFileName(element));
                else
                    File.Copy(element, destination + Path.GetFileName(element), true);
            }
        }
    }
}