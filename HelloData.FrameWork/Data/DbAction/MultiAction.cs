using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Reflection;
using System.Data.Common;

namespace HelloData.FrameWork.Data
{
    /// <summary>
    /// 多数据操作，加入事务操作
    /// </summary>
    public class MultiAction : IDisposable
    {
        private DataBase _dbHelper;

        private readonly int _appindex = 0;
        public MultiAction(int index = 0)
        {
            _appindex = index;
        }
        /// <summary>
        /// 获取当前系统运行的唯一的数据处理对象
        /// </summary>
        private DataBase DbHelper
        {
            get { return _dbHelper ?? (_dbHelper = DataPools.Current.GetDatabase(_appindex).DbBase); }
        }
        private readonly List<DataBaseAction> _multiActions = new List<DataBaseAction>();
        private readonly List<string> _multiSqls = new List<string>();
        public void AddActionSql(string sql)
        {
            _multiSqls.Add(sql);
        }
        public void AddActionSql(List<string> sqls)
        {
            _multiSqls.AddRange(sqls);
        }
        /// <summary>
        /// 添加操作对象
        /// </summary>
        /// <param name="action"></param>
        public void AddAction(DataBaseAction action)
        {
            _multiActions.Add(action);
        }
        /// <summary>
        /// 删除操作对象
        /// </summary>
        /// <param name="action"></param>
        public void RemoveAction(DataBaseAction action)
        {
            foreach (var item in _multiActions)
            {
                if (item.ActionKey == action.ActionKey)
                    _multiActions.Remove(item);
            }
        }
        /// <summary>
        /// 清空操作对象
        /// </summary>
        public void ClearAction()
        {
            _multiActions.Clear();
            _multiSqls.Clear();
        }
        private DbTransaction _currentTransaction;
        /// <summary>
        /// 执行并加入事务
        /// </summary>
        ///      
        public void Commit()
        {
            if (_multiSqls.Count > 0)
            {
                using (DbConnection currentConnet = DbHelper.CreateConnection())
                {
                    DbHelper.OpenCon(currentConnet);
                    DbCommand command = DbHelper.CreateCommand(currentConnet);
                    _currentTransaction = command.Connection.BeginTransaction(IsolationLevel.Serializable);
                    command.Transaction = _currentTransaction;
                    foreach (string item in _multiSqls)
                    {
                        command.Parameters.Clear(); 
                        command.CommandText = item ;
                        command.CommandType = CommandType.Text;
                        command.ExecuteNonQuery();
                    }
                    if (_currentTransaction != null)
                    {
                        _currentTransaction.Commit();
                        _currentTransaction.Dispose();
                        _currentTransaction = null;
                    }
                    DbHelper.CloseCon(currentConnet);
                }
            }
            else
            { 
                using (DbConnection currentConnet = DbHelper.CreateConnection())
                {
                    DbHelper.OpenCon(currentConnet);
                    DbCommand command = DbHelper.CreateCommand(currentConnet);
                    _currentTransaction = command.Connection.BeginTransaction();
                    command.Transaction = _currentTransaction;
                    foreach (DataBaseAction item in _multiActions)
                    {
                        command.Parameters.Clear();
                        item.DbHelper.MyDbCommand = command;
                        command.CommandText = item.CreateSql(item.CurrentOperate);
                        foreach (DataParameter itemparmeters in item.Parameters)
                        {
                            DbParameter newParameter = command.CreateParameter();
                            newParameter.ParameterName = itemparmeters.ParameterName;
                            newParameter.Value = itemparmeters.Value;
                            newParameter.DbType = itemparmeters.DbType;
                            if (itemparmeters.Size > -1)
                            {
                                newParameter.Size = itemparmeters.Size;
                            }
                            newParameter.Direction = itemparmeters.Direction;
                            command.Parameters.Add(newParameter);

                        }
                        command.ExecuteNonQuery();
                    }
                    if (_currentTransaction != null)
                    {
                        _currentTransaction.Commit();
                        _currentTransaction.Dispose();
                        _currentTransaction = null;
                    }
                    DbHelper.CloseCon(currentConnet);
                }
            }
        }
        /// <summary>
        /// 回退
        /// </summary>
        public void Rollback()
        {
            if (_currentTransaction == null) return;
            _currentTransaction.Rollback();
            _currentTransaction.Dispose();
            _currentTransaction = null;
        
        }
        public void Dispose()
        {
            Dispose(true);
        }

        protected void Dispose(bool isDisposing)
        {
            if (!isDisposing) return;
            if (_currentTransaction != null)
            {
                Commit();
          
            }
            ClearAction();
            GC.SuppressFinalize(this);
        }
        ~MultiAction()
        {
            Dispose();
        }

    }
}
