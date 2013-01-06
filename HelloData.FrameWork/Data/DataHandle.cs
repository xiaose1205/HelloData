using System;
using System.Collections.Generic;
using System.Data;

namespace HelloData.FrameWork.Data
{

    /// <summary>
    /// 专门用来处理其他操作
    /// </summary>
    public class DataHandle : IDisposable
    {
        private DataBase _dbHelper;
        /// <summary>
        /// 获取当前系统运行的唯一的数据处理对象
        /// </summary>
        internal DataBase DbHelper
        {
            get { return _dbHelper ?? (_dbHelper = AppDatabase.Current.GetDbBase()); }
        }
        public IDataReader ExecuteReader(string commandText)
        {
            return DbHelper.ExecuteReader(commandText);
        }

        public object GetSingel(string commandText)
        {
            return DbHelper.GetSingle(commandText);
        }
        /// <summary>
        /// 执行单条语句
        /// </summary>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public int ExecuteSql(string commandText)
        {
            return DbHelper.ExecuteSql(commandText);
        }
        public DataTable ExeDataTable(string commandText)
        {
            return DbHelper.ExeDataTable(commandText);
        }
        public DataTable CreatePage(string tablename, string colums, string where, string order, int pagesize, int pageindex, out int recordcount)
        {
            return DbHelper.CreatePage(tablename, colums, where, order, pagesize, pageindex, out recordcount);
        }
        /// <summary>
        /// 执行多条并加入事务
        /// </summary>
        /// <param name="commandTexts"></param>
        /// <returns></returns>
        public int ExecuteSqlTrans(List<string> commandTexts)
        {
            return DbHelper.ExecuteTransaction(commandTexts);
        }

        public bool IsTrans
        {
            get
            {
                return _dbHelper.IsOpenTrans;
            }
            set
            { _dbHelper.IsOpenTrans = value; }
        }
        public void Dispose()
        {
            if (_dbHelper != null)
                _dbHelper.Dispose();
        }
    }
}
