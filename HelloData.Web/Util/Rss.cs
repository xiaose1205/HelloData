using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace HelloData.Util
{
   public class Rss
    {
        /// <summary>   
        /// 获取Rss资源   
        /// </summary>   
        /// <param name="RssURL"></param>   
        /// <returns></returns>   
        public static DataTable ReadRss(string RssURL)
        {
            DataTable Dt = new DataTable();
            DataColumn Title = new DataColumn("Title", typeof(string));
            DataColumn Author = new DataColumn("Author", typeof(string));
            DataColumn PubDate = new DataColumn("PubDate", typeof(string));
            DataColumn Link = new DataColumn("Link", typeof(string));
            Dt.Columns.Add(Title);
            Dt.Columns.Add(Author);
            Dt.Columns.Add(PubDate);
            Dt.Columns.Add(Link);

            System.Net.WebRequest myRequest = System.Net.WebRequest.Create(RssURL);
            System.Net.WebResponse myResponse = myRequest.GetResponse();

            System.IO.Stream rssStream = myResponse.GetResponseStream();
            System.Xml.XmlDocument rssDoc = new System.Xml.XmlDocument();
            rssDoc.Load(rssStream);

            System.Xml.XmlNodeList rssItems = rssDoc.SelectNodes("rss/channel/item");
            for (int i = 0; i < rssItems.Count; i++)
            {
                DataRow Row = Dt.NewRow();
                System.Xml.XmlNode rssDetail;
                //标题   
                rssDetail = rssItems.Item(i).SelectSingleNode("title");
                if (rssDetail != null)
                {
                    Row["Title"] = rssDetail.InnerText;
                }
                else
                {
                    Row["Title"] = "";
                }
                //作者   
                rssDetail = rssItems.Item(i).SelectSingleNode("author");
                if (rssDetail != null)
                {
                    Row["Author"] = rssDetail.InnerText;
                }
                else
                {
                    Row["Author"] = "";
                }
                //发布时间   
                rssDetail = rssItems.Item(i).SelectSingleNode("pubDate");
                if (rssDetail != null)
                {
                    Row["PubDate"] = Convert.ToDateTime(rssDetail.InnerText).ToString("yyyy年MM月dd日");
                }
                else
                {
                    Row["PubDate"] = "";
                }
                //链接地址   
                rssDetail = rssItems.Item(i).SelectSingleNode("link");
                if (rssDetail != null)
                {
                    Row["Link"] = rssDetail.InnerText;
                }
                else
                {
                    Row["Link"] = "";
                }
                Dt.Rows.Add(Row);
            }
            return Dt;
        } 
    }
}
