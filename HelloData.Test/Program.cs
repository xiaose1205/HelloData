using System;
using System.Collections.Generic;
using System.Linq;
using HelloData.FWCommon.Cache;
using HelloData.FWCommon.Logging;
using HelloData.FrameWork;
using HelloData.FrameWork.Logging;
using HelloData.FrameWork.Data;
using System.Configuration;
using HelloData.Test.Logic;
using HelloData.Test.Entity; 
using HelloData.FrameWork.Data.Helper;

namespace HelloData.Test
{
    class Program
    {
        static void Main(string[] args)
        {

         

            //启动日志模块
            Logger.Current.SetLogger = new ConsoleLog();
            Logger.Current.IsOpenLog = true;
            Logger.CurrentLog.Info("INSTALLING");

            //设置数据库连接执行状况
            AppCons.LogSqlExcu = true;
            //设置第一个数据库
            AppCons.SetDefaultConnect(new SQLliteHelper(), ConfigurationManager.AppSettings["ConnectionString1"]);
            ////设置第二个数据库
            //AppCons.SetSecondConnect(new MySqlHelper(), ConfigurationManager.AppSettings["ConnectionString1"]);
            ////设置更多个数据库
            //AppCons.SetMoreConnect(new SQLliteHelper(), ConfigurationManager.AppSettings["ConnectionString2"]);
            //是否需要数据库全局参数化
            AppCons.IsParmes = false;
            //是否数据库操作的缓存
            AppCons.IsOpenCache = false;
            //使用第三方的分布式缓存
            //AppCons.CurrentCache =new  RedisCache();
            //使用内置的webcache缓存
            AppCons.CurrentCache = new WebCache();

            TestUserManage.Instance.CreateTable();
            TestUser user = TestUserManage.Instance.InsertTable();
            Console.WriteLine(user.firstName);
            //内存数据库的操作



            cms_userManager.Instance.SelectDemo();
            //// 新增demo
            cms_userManager.Instance.Save(new cms_user()
            {
               
                username = "test" + DateTime.Now.Millisecond,
                password = "123456",
                phone = "",
                isadmin = true,
                //主键一定要加入
                id=12
            }); 

            //if (cms_userManager.Instance.UpdatePwd("1,2,3,4,5") > 0)
            //    Console.WriteLine("success");

            //cms_userManager.Instance.Update(new cms_user()
            //                                    {
            //                                        username = "test" + DateTime.Now.Millisecond,
            //                                        password = "123456",
            //                                        id = 12,
            //                                        createtime = null
            //                                    });
            //自定义视图的操作
            //cms_user viewmodel = cms_userManager.Instance.viewtestModel();
            //if (viewmodel != null)
            //    Console.WriteLine("-->VIEWusername:" + viewmodel.username + "    VIEWid:" + viewmodel.id);
            ////修改demo
            //cms_userManager.Instance.InsertNew(new Entity.cms_user()
            //{
            //    id = 6,//主键查询where条件
            //    username = "test" + "update" + DateTime.Now.Millisecond,
            //    password = "123456"
            //});
            ////删除demo 
            //Console.WriteLine(cms_userManager.Instance.Delete(1));
            ////获取一个model
            //cms_user model = cms_userManager.Instance.getUser("11");
            //Console.WriteLine(model == null ? "null" : model.username);
            ////分页获取数据
            //PageList<cms_user> list = cms_userManager.Instance.GetList(1, 20);
            //foreach (var item in list)
            //{
            //    Console.WriteLine("-->username:" + item.username + "    id:" + item.id);
            //}
            //Console.WriteLine(list.TotalPage);
            //Console.WriteLine(list.TotalCount);

            Logger.CurrentLog.Info("the End!");
            //发送Email
            try
            {
                //MailHelper mailHelper = new MailHelper
                //                            {
                //                                SmtpServer = "smtp.qq.com",
                //                                AdminEmail = "511522329@qq.com",
                //                                UserName = "511522329@qq.com",
                //                                Password = "wan8756345675",
                //                                PopServer = "pop.qq.com"
                //                            };


                //string subject = "注册系统邮件验证";
                //string message = String.Format("亲爱的test，您好！<p/>感谢您在我们网站注册成为会员，故系统自动为你发送了这封邮件。请点击下面链接进行验证：<a href='http://icoolly.taobao.com/'>点击验证</a> ");
                ////mailHelper.Send("123456789@qq.com", mailHelper.AdminEmail, subject, message, "Low");
                //Logger.CurrentLog.Info("Email is ok!");
            }
            catch (Exception ex)
            {
                Logger.CurrentLog.Info("Email is wrong:" + ex.Message + "");
            }

            ////写入txt
            //ExportTxt export = new ExportTxt();
            //var liststr = new List<string> { "12312", "123asass12" };
            //export.WriteContent(liststr);
            //Console.WriteLine(export.GetFullPath());
            ////写入csv
            //ExportCsv exportsc = new ExportCsv();
            //exportsc.WriteContent(liststr);
            //Console.WriteLine(exportsc.GetFullPath());
            ////写入excel
            //ExportExcle exportex = new ExportExcle();
            //exportex.WriteRow(liststr);
            //Console.WriteLine(exportex.GetFullPath());

            Console.ReadLine();
        }
    }

}
