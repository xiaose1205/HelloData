using System;
using System.Text;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.IO;
using System.Xml;

namespace HelloData.FWCommon.Utils
{
    /// <summary>
    /// 邮件发送助手类
    /// </summary>
    public class MailHelper
    {
        #region 属性

        public string AdminEmail { get; set; }

        public string SmtpServer { get; set; }

        /// <summary>
        /// 收邮件服务
        /// </summary>
        public string PopServer { get; set; }

        public string Password { get; set; }

        public string UserName { get; set; }

        #endregion

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="to"></param>
        /// <param name="from"></param>
        /// <param name="subject"></param>
        /// <param name="message"></param>
        /// <param name="priority"> </param>
        /// <returns></returns>
        public bool Send(string to, string from, string subject, string message, string priority)
        {

            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(from);//发送人地址
            mailMessage.To.Add(to);//接受人地址
            mailMessage.Subject = subject.Trim().Replace("\r\n", " ").Replace("<br/>", " ");

            mailMessage.SubjectEncoding = Encoding.UTF8;
            mailMessage.Body = message.Replace("\r\n", "<br/>");
            mailMessage.BodyEncoding = Encoding.UTF8;
            mailMessage.IsBodyHtml = true;
            switch (priority)       //邮件优先级
            {
                case "High":
                    mailMessage.Priority = MailPriority.High;
                    break;
                case "Low":
                    mailMessage.Priority = MailPriority.Low;
                    break;
                case "Normal":
                    mailMessage.Priority = MailPriority.Normal;
                    break;
                default:
                    mailMessage.Priority = MailPriority.Normal;
                    break;
            }
            SmtpClient smtp = new SmtpClient
                                  {
                                      Credentials = new NetworkCredential(UserName, Password),
                                      Port = 25,
                                      Host = SmtpServer,
                                      EnableSsl = false
                                  }; // 提供身份验证的用户名和密码 // 网易邮件用户可能为：username password // Gmail 用户可能为：username@gmail.com password 

            //smtp.SendCompleted += new SendCompletedEventHandler(SendMailCompleted);
            try
            {
                smtp.Send(mailMessage);
            }
            catch (SmtpException ex)
            {
                SendMailMessageToXml(mailMessage);
                throw new Exception("邮件发送失败，请登录管理后台检查邮件配置是否正确。原因：" + ex.Message);
            }
            return true;
        }
        /// <summary>
        /// 未能正确发送的邮件将以XML形式转存至/_Data/SendEmail/目录下
        /// </summary>
        /// <param name="mailMessage"></param>
        public void SendMailMessageToXml(MailMessage mailMessage)
        {
            try
            {
                string subject = mailMessage.Subject.ToString();//邮件标题
                string body = (string)mailMessage.Body;//邮件正文
                string replyTime = DateTime.Now.ToString();//邮件
                string user = mailMessage.To[0].Address;//收件人地址
                string formUser = mailMessage.From.Address;//发件人地址

                if (subject != "")
                {
                    string filePath = HttpContext.Current.Server.MapPath("/_Data/SendEmail/");
                    DateTime time = Convert.ToDateTime(replyTime);
                    string fileName = subject + DateTime.Now.ToString(".yyyy_MM_dd_HH_mm_ss") + ".xml";
                    string path = Path.Combine(filePath, fileName);

                    //检查是否XML文件存放临时路径存在，如果不存在则进行处理
                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }
                    //检查XMLSchema文件是否存在，如果不存在则进行处理
                    if (!File.Exists(subject))
                    {
                        XmlDocument doc = new XmlDocument();
                        //转换字符
                        subject = StringPlus.Base64Encode(subject);
                        user = StringPlus.Base64Encode(user);
                        body = StringPlus.Base64Encode(body);
                        formUser = StringPlus.Base64Encode(formUser);

                        string xml = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>\r\n"
                            + "<root><infoSubject>" + subject + "</infoSubject><infoUser>" + user + "</infoUser><infoFormUser>" +
                            formUser + "</infoFormUser><infoBody>" + body + "</infoBody><infoTime>"
                            + replyTime + "</infoTime></root>";
                        doc.LoadXml(xml);
                        doc.Save(path);
                    }
                }
            }
            catch
            {
            }
        }

    }

    public class MailResult
    {
        public int Count { get; set; }

        public int Success { get; set; }

        public string StateText { get; set; }
    }

}
