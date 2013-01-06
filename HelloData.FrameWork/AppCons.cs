using System;
using System.Data.Common;
using HelloData.FrameWork.Data;
using HelloData.FrameWork.Cache;

namespace HelloData.FrameWork
{
    public static class AppCons
    {

        /// <summary>
        /// 设置连接字符串
        /// </summary>
        public static string ConnectionString
        {
            get
            {
                return DataPools.Current.GetDatabase(0).ConnectionString;
            }
            set
            {
                if (DataPools.Current.AppDatabaseList.Count == 1)
                {
                    DataPools.Current.GetDatabase(0).DbBase = new MsSqlHelper(value);
                    DataPools.Current.GetDatabase(0).ConnectionString = value;
                }
                else
                    DataPools.Current.AddAppDatabase(new AppDatabase
                                                         {
                                                             ConnectionString = value,
                                                             DbBase = new MsSqlHelper(value)
                                                         });
            }
        }
        /// <summary>
        /// 是否记录下数据库的执行情况
        /// </summary>
        public static bool LogSqlExcu { get; set; }
        /// <summary>
        /// 获取当前的数据连接
        /// </summary>
        public static DbConnection Connection { get; set; }

        /// <summary>
        /// 设置是否开启全局的缓存
        /// </summary>
        public static bool IsOpenCache
        {
            get { return CacheHelper.IsOpenCache; }
            set { CacheHelper.IsOpenCache = value; }

        }

        /// <summary>
        /// 全局的页面大小
        /// </summary>
        public static int PageCount = 15;

        /// <summary>
        /// 是否启用参数设置(安全不便于调试)
        /// </summary>
        public static bool IsParmes { get; set; }
        /// <summary>
        /// 启动服务
        /// </summary>
        public static DateTime StartTime { get; set; }

        /// <summary>
        /// 设置默认的数据库连接（baselogic使用的是默认连接）
        /// </summary>
        /// <param name="dbBase"></param>
        /// <param name="connectionString"></param>
        public static void SetDefaultConnect(DataBase dbBase, string connectionString)
        {
            if (DataPools.Current.AppDatabaseList.Count == 1)
            {
                DataPools.Current.GetDatabase(0).DbBase = dbBase;
                DataPools.Current.GetDatabase(0).ConnectionString = connectionString;
            }
            else if (DataPools.Current.AppDatabaseList.Count < 1)
            {
                DataPools.Current.AddAppDatabase(new AppDatabase
                                                     {
                                                         ConnectionString = connectionString,
                                                         DbBase = dbBase
                                                     });
            }
        }
        /// <summary>
        /// 设置第二个数据库连接
        /// </summary>
        /// <param name="dbBase"></param>
        /// <param name="connectionString"></param>
        public static void SetSecondConnect(DataBase dbBase, string connectionString)
        {
            if (DataPools.Current.AppDatabaseList.Count == 2)
            {
                DataPools.Current.GetDatabase(1).DbBase = dbBase;
                DataPools.Current.GetDatabase(1).ConnectionString = connectionString;
            }
            else if (DataPools.Current.AppDatabaseList.Count < 2)
            {
                DataPools.Current.AddAppDatabase(new AppDatabase
                                                     {
                                                         ConnectionString = connectionString,
                                                         DbBase = dbBase
                                                     });
            }
        }

        /// <summary>
        /// 设置多个数据库连接，连续使用这个方法即可
        /// </summary>
        /// <param name="dbBase"> </param>
        /// <param name="connectionString"></param>
        public static void SetMoreConnect(DataBase dbBase, string connectionString)
        {
            DataPools.Current.AddAppDatabase(new AppDatabase
                                                 {
                                                     ConnectionString = connectionString,
                                                     DbBase = dbBase
                                                 });
        }
        /// <summary>
        /// 设置自定义的cache类
        /// </summary>
        public static ICache CurrentCache
        {
            get { return CacheHelper.Cache; }
            set { CacheHelper.Cache = value; }
        }
    }
}
