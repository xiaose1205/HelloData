using System;

namespace HelloData.FrameWork.Data
{
    /// <summary>
    /// 全局的数据处理层
    /// </summary>
    public class AppDatabase
    {

        public string ConnectionString { get; set; }


        private DataBase _database;
        /// <summary>
        /// 数据库实际操作的类
        /// </summary>
        internal DataBase DbBase
        {
            get
            {
                if (_database != null)
                    return _database;
                return null;
            }
            set
            {
                _database = value;
                if (!string.IsNullOrEmpty(ConnectionString))
                {
                    _database.CurConStr = ConnectionString;
                    _database.Inistall();
                }
            }
        }


    }
}
