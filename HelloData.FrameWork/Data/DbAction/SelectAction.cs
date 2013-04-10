using System.Data;

namespace HelloData.FrameWork.Data
{
    /// <summary>
    /// 查询专用的action
    /// </summary>
    public class SelectAction : DataBaseAction
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="index">默认为0表示使用默认的数据库连接，获取数据库的设置索引，</param>
        public SelectAction(BaseEntity entity, int index = 0)
            : base(entity, index)
        {
            CurrentOperate = OperateEnum.Select;
        }
        public SelectAction(string tbName, int index = 0)
            : base(tbName, index)
        {
            CurrentOperate = OperateEnum.Select;
        }
        private string BuildSql()
        {

            string sqlstr = CreateSql(CurrentOperate);
            DbHelper.Parameters = this.Parameters;
            return sqlstr;
        }
        public new object QuerySingle()
        {

            return DbHelper.GetSingle(BuildSql());
        }

        public new T QueryEntity<T>() where T : new()
        {
            this.SqlPageParms(1);
            return base.QueryEntity<T>();
        }
        public new IDataReader QueryDataReader()
        {
            return DbHelper.ExecuteReader(BuildSql());
        }

        public DataTable QueryDataTable()
        {
            return DbHelper.ExeDataTable(BuildSql());

        }
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public PageList<T> QueryPage<T>(int pageindex) where T : new()
        {
            return base.QueryPage<T>(pageindex);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public PageList<T> QueryPage<T>(int pageindex, int pagesize) where T : new()
        {
            return base.QueryPage<T>(pageindex, pagesize);
        }

    
    }
}