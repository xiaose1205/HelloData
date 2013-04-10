using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using HelloData.FWCommon.Logging;
using HelloData.FrameWork.AOP;
namespace HelloData.FrameWork.Data
{
    public class DataBase : IDisposable
    {
        public DataBase()
        {

        }
        public void Inistall()
        {
            MyDbFactory = GetFactory();
            MyConnection = MyDbFactory.CreateConnection();
            var myConnection = MyConnection;
            if (myConnection == null) return;
            myConnection.ConnectionString = CurConStr;
            AppCons.Connection = MyConnection;
        }

        /// <summary>
        /// 当前链接字符串
        /// </summary>
        public string CurConStr { get; set; }

        public DataBase(string conn)
        {
            CurConStr = conn;
            Inistall();
        }
        /*   public const string SqlServer = "System.Data.SqlClient";
           public const string Sybase = "Sybase.Data.AseClient";
           public const string Access = "System.Data.OleDb";
           public const string MySql = "MySql.Data.MySqlClient";
           public const string Oracle = "System.Data.OracleClient";
           public const string PostgreSql = "policy.2.0.Npgsql";
           public const string SqLite = "System.Data.SQLite";*/
        /// <summary>
        /// 当前数据库操作provider
        /// </summary>
        public virtual string ProviderName
        {
            get { return string.Empty; }
        }

        readonly AopTimer _watch = new AopTimer();
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// 数据操作工厂（作为统一用）
        /// </summary>
        public DbProviderFactory MyDbFactory;
        /// <summary>
        /// 数据库链接（作为统一链接用）
        /// </summary>
        public DbConnection MyConnection;
        private DbTransaction _myDbTransaction;

        internal delegate void OnException(string msg);
        /// <summary>
        /// 获取运行中的错误及异常
        /// </summary>
        internal event OnException OnExceptionEvent;

        /// <summary>
        /// 是否开启事务
        /// </summary>
        public bool IsOpenTrans = true;
        internal IsolationLevel TranLevel = IsolationLevel.Unspecified;
        /// <summary>
        /// 手动设置command 
        /// </summary>
        public DbCommand MyDbCommand { get; set; }


        /// <summary>
        /// 当前使用的command
        /// </summary>
        internal DbCommand CurCommand;
        /// <summary>
        /// 主动创建command
        /// </summary>
        /// <returns></returns>
        public DbCommand CreateCommand(DbConnection currentConnet)
        {
            if (currentConnet != null)
            {
                DbCommand command = currentConnet.CreateCommand();
                command.Connection = currentConnet;
                return command;
            }
            return null;
        }

        /// <summary>
        ///  主动创建command并带入参数
        /// </summary>
        /// <returns></returns>
        public virtual DbCommand CreateCommandWithPar(DbConnection connention)
        {
            if (connention != null)
            {
                DbCommand command = connention.CreateCommand();
                command.Connection = connention;
                command.Parameters.Clear();
                if (Parameters != null && Parameters.Count > 0)
                {
                    foreach (DataParameter item in Parameters)
                    {
                        if (AppCons.LogSqlExcu)
                           Logger.CurrentLog.Info(string.Format("parms:{0};value:{1}", item.ParameterName, item.Value));
                        DbParameter newParameter = GetNewParameter();
                        newParameter.ParameterName = item.ParameterName;
                        newParameter.Value = item.Value;
                        newParameter.DbType = item.DbType;
                        if (item.Size > -1)
                        {
                            newParameter.Size = item.Size;
                        }
                        newParameter.Direction = item.Direction;
                        command.Parameters.Add(newParameter);
                    }
                }

                return command;
            }
            return null;
        }
        internal DbParameter GetNewParameter()
        {
            if (MyDbFactory == null)
                return null;
            DbParameter parmeter = MyDbFactory.CreateParameter();
            return parmeter;
        }

        /// <summary>
        /// 获取数据源类
        /// </summary>
        /// <returns></returns>
        private DbProviderFactory GetFactory()
        {
            return DbProviderFactories.GetFactory(ProviderName);
        }



        public void RollBack()
        {
            if (_myDbTransaction != null)
                _myDbTransaction.Rollback();
        }
        private void CommitError(string errMsg, Exception ex)
        {
            if (OnExceptionEvent != null)
                OnExceptionEvent(errMsg);
            if (_myDbTransaction != null)
            {
                _myDbTransaction.Rollback();
                _myDbTransaction.Dispose();
                _myDbTransaction = null;
            }



            if (Logger.Current.IsOpenLog)
            {
                Logger.CurrentLog.Error(errMsg, ex);
                if (Parameters != null)
                    foreach (var item in Parameters)
                    {
                        Logger.CurrentLog.Info(string.Format("parms:{0};value:{1}", item.ParameterName, item.Value));
                    }
            }
            _watch.LogMessage(string.Format("执行异常，计时器停止。"));
        }
        public string Pre = "@";
        /// <summary>
        /// 获取合法的参数
        /// </summary>
        /// <param name="parameterName"></param>
        /// <returns></returns>
        public virtual string GetParameterName(string parameterName)
        {
            return (parameterName.Substring(0, 1) == Pre) ? parameterName : (Pre + parameterName);
        }
        /// <summary>
        /// 当前处理的参数，只对当前操作有效
        /// </summary>
        public List<DataParameter> Parameters = new List<DataParameter>();

        /// <summary>
        /// 打开连接
        /// </summary>
        public DbConnection OpenCon(DbConnection connection)
        {
            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
            }
            catch (Exception ex)
            {
                CommitError(ex.Message, ex);
            }
            return connection;
        }
        /// <summary>
        /// 关闭连接 
        /// </summary>
        public virtual void CloseCon(DbConnection myConnection)
        {
            try
            {
                if (myConnection.State != ConnectionState.Open)
                    return;
                if (_myDbTransaction != null)
                {
                    IsOpenTrans = false;
                    _myDbTransaction.Commit();
                    _myDbTransaction = null;
                }
                if (CurCommand != null)
                {
                    CurCommand.Dispose();
                    CurCommand.Parameters.Clear();
                    CurCommand = null;
                }
                Parameters.Clear();
                myConnection.Close();
            }
            catch (Exception ex)
            {
                CommitError(ex.Message, ex);
            }
        }





        #region 常用的数据操作
        /// <summary>
        /// 执行sql
        /// </summary>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public int ExecuteSql(string commandText)
        {
            int num = 0;

            DbConnection currentConnet = CreateConnection();
            try
            {
                {
                    using (DbCommand command = SetCommandText(commandText, currentConnet))
                    {
                        _watch.BeginWithMessage(string.Format("sql语句：{0}", commandText));
                        OpenCon(currentConnet);
                        num = command.ExecuteNonQuery();
                        _watch.End();
                        Logger.CurrentLog.Info("ExecuteSql Is Success!Result(int):" + num + "");
                    }
                }
            }
            catch (Exception ex)
            {
                CommitError(ex.Message, ex);
            }
            finally { CloseCon(currentConnet); }
            return num;
        }
        /// <summary>
        /// 执行 存储过程
        /// </summary>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public int ExecuteStoredProcedure(string commandText)
        {
            int num = 0;
            DbConnection currentConnet = CreateConnection();
            try
            {

                {
                    using (DbCommand command = SetCommandText(commandText, currentConnet))
                    {
                        _watch.BeginWithMessage(string.Format("sql语句：{0}", commandText));
                        OpenCon(currentConnet);
                        command.CommandType = CommandType.StoredProcedure;
                        num = command.ExecuteNonQuery();
                        _watch.End();
                        Logger.CurrentLog.Info("ExecuteStoredProcedure Is Success!Result(int):" + num + "");
                    }
                }
            }
            catch (Exception ex)
            {
                CommitError(ex.Message, ex);
            }
            finally { CloseCon(currentConnet); }
            return num;
        }

        /// <summary>
        /// 执行 存储过程
        /// </summary>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public List<DataParameter> ExecuteStoredOutProcedure(string commandText)
        {
            int num = 0;
            List<DataParameter> outparameters = new List<DataParameter>();
            DbConnection currentConnet = CreateConnection();
            try
            {

                {
                    using (DbCommand command = SetCommandText(commandText, currentConnet))
                    {
                        _watch.BeginWithMessage(string.Format("sql语句：{0}", commandText));
                        OpenCon(currentConnet);
                        command.CommandType = CommandType.StoredProcedure;
                        num = command.ExecuteNonQuery();
                        outparameters.AddRange(from DbParameter par in command.Parameters
                                               where par.Direction == ParameterDirection.Output
                                               select new DataParameter()
                                                   {
                                                       ParameterName = par.ParameterName,
                                                       Value = par.Value
                                                   });
                        _watch.End();
                        Logger.CurrentLog.Info("ExecuteStoredProcedure Is Success!Result(int):" + num + "");
                    }
                }
            }
            catch (Exception ex)
            {
                CommitError(ex.Message, ex);
            }
            finally { CloseCon(currentConnet); }
            return outparameters;
        }
        /// <summary>
        /// 获取第一行第一列
        /// </summary>
        /// <returns></returns>
        public object GetSingle(string commandText)
        {
            object obj = null;
            DbConnection currentConnet = CreateConnection();
            try
            {

                {
                    using (DbCommand command = SetCommandText(commandText, currentConnet))
                    {
                        _watch.BeginWithMessage(string.Format("sql语句：{0}", commandText));
                        OpenCon(currentConnet);
                        obj = command.ExecuteScalar();
                        _watch.End();
                        Logger.CurrentLog.Info("getSingle Is Success!Result(object):" + obj + "");
                    }
                }
            }
            catch (Exception ex)
            {
                CommitError(ex.Message, ex);
            }
            finally { CloseCon(currentConnet); }

            return obj;
        }
        /// <summary>
        /// 返回datareader
        /// </summary>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public IDataReader ExecuteReader(string commandText)
        {
            // SetCommandText(commandText);
            DbDataReader reader = null;
            //try
            //{
            //    OpenCon();
            //    reader = _curCommand.ExecuteReader(CommandBehavior.CloseConnection);
            //}
            //catch (Exception ex)
            //{
            //    CommitError(ex.Message, ex);
            //}
            //finally { CloseCon(); }
            return null;
        }

        /// <summary>
        /// 获取datatable
        /// </summary>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public DataTable ExeDataTable(string commandText)
        {
            DataTable dataTable = new DataTable();
            DbConnection currentConnet = CreateConnection();

            try
            {
                {
                    using (DbCommand command = SetCommandText(commandText, currentConnet))
                    {
                        DbDataAdapter adapter = MyDbFactory.CreateDataAdapter();
                        if (adapter != null)
                        {
                            adapter.SelectCommand = command;
                            adapter.SelectCommand.CommandText = commandText;
                        }
                        else
                        {
                            return new DataTable();
                        }

                        OpenCon(currentConnet);
                        _watch.BeginWithMessage(string.Format("sql语句：{0}", commandText));

                        adapter.Fill(dataTable);
                        _watch.End();
                        Logger.CurrentLog.Info("ExeDataTable Is Success!Result(datatable):" + dataTable.Rows.Count + "");
                    }
                }
            }
            catch (Exception ex)
            {
                CommitError(ex.Message, ex);
            }
            finally { CloseCon(currentConnet); }

            return dataTable;
        }



        /// <summary>
        /// 执行多条sql并加入事务
        /// </summary>
        /// <param name="commondtexts"></param>
        /// <returns></returns>
        public int ExecuteTransaction(List<string> commondtexts)
        {
            int num = 0;
            DbConnection currentConnet = CreateConnection();
            try
            {
                {
                    OpenCon(currentConnet);
                    _watch.Begin();
                    foreach (var commandText in commondtexts)
                    {
                        using (DbCommand command = SetCommandText(commandText, currentConnet))
                        {
                            _watch.BeginWithMessage(string.Format("sql语句：{0}", commandText));

                            if (command != null)
                            { 
                                int res=command.ExecuteNonQuery();
                                num += res;
                                Logger.CurrentLog.Info("ExeDataTable Is Success!Result(int));" + res + "");
                            }
                          
                        }
                    }

                    _watch.End();
                    Logger.CurrentLog.Info("ExecuteTransaction Is Success!Result(int):" + num + "");

                }
            }
            catch (Exception ex)
            {
                CommitError(ex.Message, ex);
            }
            finally { CloseCon(currentConnet); }

            return num;
        }
        /// <summary>
        /// 创建一个新的链接
        /// </summary>
        /// <returns></returns>
        public virtual DbConnection CreateConnection()
        {
            //if (_myConnection != null)
            //{
            //    DbConnection connect = _myConnection;
            //    return connect;
            //}
            //else
            {
                DbConnection connect = MyDbFactory.CreateConnection();
                if (connect == null)
                    return null;
                connect.ConnectionString = CurConStr;
                return connect;
            }
        }
        /// <summary>
        /// 创建分页
        /// </summary>  
        /// <returns></returns>
        public DataTable CreatePage(string sql, string sqlcount, out int recordcount)
        {
            recordcount = 0;
            if (!string.IsNullOrEmpty(sqlcount))
            {
                object totalcount = GetSingle(sqlcount);
                if (totalcount != null)
                    recordcount = Convert.ToInt32(totalcount);
                if (recordcount == 0)
                    return null;
                return ExeDataTable(sql);
            }

            return ExeDataTable(sql);
        }


        #endregion
        /// <summary>
        /// 设置commandText
        /// </summary>
        /// <param name="commandText"></param>
        private DbCommand SetCommandText(string commandText, DbConnection connention)
        {
            DbCommand command = CreateCommandWithPar(connention);
            command.CommandText = commandText;
            command.CommandTimeout = 30;
            return command;
        }

        /// <summary>
        /// 字符转换
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public string FilterContent(string content)
        {
            return string.IsNullOrEmpty(content) ? string.Empty : content.Replace("'", "''");
        }

        public virtual DbType ConvertToDbType(object invalue)
        {
            if (invalue == null)
                return DbType.String;
            DbType objvalue = DbType.String;
            DateTime dateTime;
            Type valueType = invalue.GetType();
            if (valueType.IsEnum)
            {
                objvalue = DbType.Int32;
            }
            else if (valueType.Name == typeof(int).Name ||
                 valueType.Name == typeof(Int16).Name ||
                 valueType.Name == typeof(Int32).Name ||
                 valueType.Name == typeof(Int64).Name ||
                 valueType.Name == typeof(UInt16).Name ||
                 valueType.Name == typeof(UInt32).Name ||
                 valueType.Name == typeof(UInt64).Name ||
                 valueType.Name == typeof(Decimal).Name ||
                 valueType.Name == typeof(Double).Name ||
                 valueType.Name == typeof(Byte).Name)
            {
                objvalue = DbType.Int32;
            }
            else if (valueType.Equals(typeof(bool)))
            {
                objvalue = DbType.Boolean;
            }
            else if (valueType.Name == "String")
            {
                objvalue = DbType.String;
            }
            else if (DateTime.TryParse(invalue.ToString(), out dateTime))
            {
                objvalue = DbType.DateTime;
            }
            else
            {
                objvalue = DbType.String;
            }
            return objvalue;
        }


        public virtual string ReturnDBValue(DbType dataType, object value)
        {
            string revalue;
            if (dataType == DbType.Boolean)
                revalue = (bool)value ? "1" : "0";
            else if (dataType == DbType.Int16 || dataType == DbType.Int32 || dataType == DbType.Int64)
                revalue = value.ToString();
            else
                revalue = string.Format("'{0}'", value);
            return revalue;

        }
        /// <summary>
        /// 参数的差异处理
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual object ReturnDbParmValue(object value)
        {
            return value;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tablename"></param>
        /// <param name="coloums"></param>
        /// <param name="where"></param>
        /// <param name="orderby"></param>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <param name="selcountstr"></param>
        /// <returns></returns>
        public virtual string CreatePageString(string tablename, string coloums, string where, string groupby, string orderby, int pageindex, int pagesize, out string selcountstr)
        {
            selcountstr = string.Empty;
            return string.Empty;
        }
        /// <summary>
        /// 查询插入后查询出来的主键
        /// </summary>
        public virtual string SELECTIDENTITY { get; set; }
    }
}
