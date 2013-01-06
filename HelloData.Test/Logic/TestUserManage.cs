using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HelloData.FrameWork.Data;
using HelloData.Test.Entity;
using System.Data;
using System.Threading;


namespace HelloData.Test.Logic
{
    public class TestUserManage : BaseManager<TestUserManage, TestUser>
    {
        IDbConnection dbcon;
        public void CreateTable()
        {
            string tablsestr = @"CREATE TABLE TestUser (
                id int,
                firstName varchar(32))";
            TradAction action = new TradAction();
            action.Excute(tablsestr);
            ////Data Source=file::memory:,version=3
            //string connectionString = "Data Source=:memory:;Version=3";

            //dbcon = (IDbConnection)new System.Data.SQLite.SQLiteConnection(connectionString);
            //dbcon.Open();
            //IDbCommand dbcmd = dbcon.CreateCommand();
            //dbcmd.CommandText = tablsestr;
            //dbcmd.ExecuteScalar();

            //// requires a table to be created named employee
            //// with columns firstname and lastname
            //// such as,
            ////        CREATE TABLE employee (
            ////           firstname varchar(32),
            ////           lastname varchar(32));
            //string sql = "insert into TestUser(id,firstName) values(1,'123asdasd');";
            //   //"SELECT * " +
            //   //"FROM TestUser";
            //dbcmd.CommandText = sql;
            //dbcmd.ExecuteScalar();
            //dbcmd.Dispose();
            //dbcmd = null;
        }
        public TestUser InsertTable()
        {
            //string connectionString = "Data Source=:memory:;Version=3";
            //IDbCommand dbcmd = dbcon.CreateCommand();

            //string sql =
            //"SELECT * " +
            //"FROM TestUser";
            //dbcmd.CommandText = sql;
            //IDataReader reader = dbcmd.ExecuteReader();
            //while (reader.Read())
            //{
            //    string FirstName = reader.GetString(1);
            //    Console.WriteLine("Name: " +FirstName  );
            //}
            // clean up
            //reader.Close();
            //reader = null;
            //dbcmd.Dispose();
            //dbcmd = null;
            //dbcon.Close();
            //dbcon = null;
       

            for (int i = 0; i < 1; i++)
            {
                Thread t = new Thread(new ThreadStart(getList));
                 t.Start();
            } 
         
            return new TestUser();
        }

        private object obj = 0;
        public void getList()
        {
           // lock (obj)
            {
                TestUser user = new TestUser();

          
                for (int i = 0; i < 1; i++)
                {
                    user.firstName = "1223334s" + i.ToString();
                    this.Save(user);
                }
                PageList<TestUser> users = this.GetList(1, 10000);
                foreach (TestUser testUser in users)
                {
                    Console.WriteLine(testUser.firstName);
                    Console.WriteLine(users.TotalCount);
                    break;
                } 
          
            }

        }
    }
}
