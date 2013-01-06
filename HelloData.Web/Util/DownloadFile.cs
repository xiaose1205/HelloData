using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace HelloData.Util
{
    /// <summary>
    /// //测试用线程1断点续传下载网络上的文件到本地电脑
    /// </summary>
    public class DownloadFile
    {
        public string StrUrl;//文件下载网址
        public string StrFileName;//下载文件保存地址 
        public string strError;//返回结果
        public long lStartPos = 0; //返回上次下载字节
        public long lCurrentPos = 0;//返回当前下载字节
        public long lDownloadFile;//返回当前下载文件长度

        public bool Download()
        {
            bool Error = false;
            System.IO.FileStream fs;
            if (System.IO.File.Exists(StrFileName))
            {
                fs = System.IO.File.OpenWrite(StrFileName);
                lStartPos = fs.Length;
                fs.Seek(lStartPos, System.IO.SeekOrigin.Current);
                //移动文件流中的当前指针 
            }
            else
            {
                fs = new System.IO.FileStream(StrFileName, System.IO.FileMode.Create);
                lStartPos = 0;
            }

            //打开网络连接 
            try
            {
                System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(StrUrl);
                long length = request.GetResponse().ContentLength;
                lDownloadFile = length;
                if (lStartPos > 0)
                    request.AddRange((int)lStartPos); //设置Range值

                //向服务器请求，获得服务器回应数据流 
                System.IO.Stream ns = request.GetResponse().GetResponseStream();

                byte[] nbytes = new byte[512];
                int nReadSize = 0;
                nReadSize = ns.Read(nbytes, 0, 512);
                while (nReadSize > 0)
                {
                    fs.Write(nbytes, 0, nReadSize);
                    nReadSize = ns.Read(nbytes, 0, 512);
                    lCurrentPos = fs.Length;
                }

                fs.Close();
                ns.Close();
                strError = "下载完成";
                Error = true;
               

            }
            catch (Exception ex)
            {
                fs.Close();
                strError = "下载过程中出现错误:" + ex.ToString();
                Error = false;
            }
            return Error;
        }
    }　

}