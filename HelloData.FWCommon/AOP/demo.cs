#region Version Info
/* ========================================================================
* 【本类功能概述】
* 
* 作者：王军 时间：2013/4/13 13:08:54
* 文件名：demo
* 版本：V1.0.1
* 联系方式：511522329  
*
* 修改者： 时间： 
* 修改说明：
* ========================================================================
*/
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HelloData.FWCommon.AOP
{
    class Program
    {
        static void Main(string[] args)
        {
            ICalc calc2 = ProxyFactory.CreateProxy<ICalc>(typeof(Calculater));
            calc2.Add(2, 3);
            calc2.Divide(6, 3);
            User model = new User();
            model.S = "asdasd";
            model.I = 12;
            List<User> models = new List<User>();
            models.Add(model);
            calc2.TestModel(model, models);
            Console.ReadKey(true);
        }
    }
    public class LogStartAttribute : PreAspectAttribute
    {
        public override void Action(InvokeContext context)
        {
            Console.WriteLine("log start!");
        }
    }

    public sealed class Arround1Attribute : ArroundAttribute
    {

    }

    public class LogEndAttribute : PostAspectAttribute
    {
        public override void Action(InvokeContext context)
        {
            Console.WriteLine("log end!");
        }
    }

    public class LogExAttribute : ExceptionAspectAttribute
    {
        public override void Action(InvokeContext context)
        {
            Console.WriteLine("log exception!");
        }
    }

    public interface ICalc
    {
        void Add(int a, int b);
        void Divide(int a, int b);
        void TestModel(User model, List<User> models);
    }
    [LogStart]
    [LogEnd]
    public class Calculater : ICalc
    {

        public void Add(int a, int b)
        {
            Console.WriteLine("a+b=" + (a + b).ToString());
        }

        //  [LogEx]
        [LogStart]
        [LogEnd]
        public void Divide(int a, int b)
        {
            Console.WriteLine("a/b=" + (a / b).ToString());
            throw new Exception("");
        }
        [LogStart]
        [LogEnd]

        public void TestModel(User model, List<User> models)
        {
            Console.WriteLine(model.S);
            Console.WriteLine(models[0].I);
        }
    }

    public class User
    {
        public string S { get; set; }
        public int I { get; set; }
    }
}
