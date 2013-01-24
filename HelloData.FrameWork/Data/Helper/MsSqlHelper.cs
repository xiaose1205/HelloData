using System;
using System.Data;

namespace HelloData.FrameWork.Data
{
    public class MsSqlHelper : DataBase
    {
        public MsSqlHelper() : base() { }
        public MsSqlHelper(string conn)
            : base(conn)
        {
            base.IsOpenTrans = false;
        }
        public override string ProviderName
        {
            get
            {
                return "System.Data.SqlClient";
            }
        }

        /// <summary>
        /// 创建分页
        /// </summary>
        /// <param name="tablename"></param>
        /// <param name="colums"></param>
        /// <param name="where"></param>
        /// <param name="order"></param>
        /// <param name="pagesize">-1表示系统默认值</param>
        /// <param name="pageindex"></param>
        /// <returns></returns>
        public override string CreatePageString(string tablename, string colums, string where,string groupby, string order, int pageindex, int pagesize, out string selcountstr)
        {
            if (string.IsNullOrEmpty(tablename))
                throw new Exception("必须输入表名");
            if (string.IsNullOrEmpty(colums))
                colums = "*";
            if (string.IsNullOrEmpty(where))
                where = "  ";
            else
                where = " where " + (where.Length > 7 ? where.Remove(0, 7) : where);
            string groupbystr = string.Empty;
            if (!string.IsNullOrEmpty(groupby))
                groupbystr = "group by " + groupby;
            if (pageindex == 0)
            {
                selcountstr = string.Empty;
                string top = string.Empty;
                if (pagesize != -1)
                    top = " top  " + pagesize + " ";
                //直接查询指定的数目的数据
                if (string.IsNullOrEmpty(order))
                    return (string.Format("SELECT " + top + " {0} from {1}   {2}   " + groupbystr + "  ;",
                        colums, tablename, where));
                else
                    return (string.Format("SELECT " + top + " {0} from {1}   {2}  " + groupbystr + " ORDER BY {3}  ;",
                            colums, tablename, where, order));
            }
            else
            {
                selcountstr = (string.Format("select count(1) from {0}   {1}  " + groupbystr + "", tablename, where));

                if (pagesize == -1)
                    pagesize = 20;
                int skip = pageindex;
                if (pageindex > 0)
                {
                    skip = pageindex - 1;
                }

                skip = pagesize * skip;
                if (string.IsNullOrEmpty(order))
                    return (string.Format("with temptbl as (SELECT ROW_NUMBER() OVER (ORDER BY {0})AS Row, {1} from {2}  O   {5}  " + groupbystr + ") SELECT * FROM temptbl where Row between {3} and {4}",
                  "  newid() ", colums, tablename, skip, skip + pagesize, where));
                else
                    return (string.Format("with temptbl as (SELECT ROW_NUMBER() OVER (ORDER BY {0})AS Row, {1} from {2}  O    {5}  " + groupbystr + ") SELECT * FROM temptbl where Row between {3} and {4}",
                    order, colums, tablename, skip, skip + pagesize, where));

            }
        }

        public override string ReturnDBValue(DbType DataType, object value)
        {
            string revalue = string.Empty;
            if (DataType == DbType.Boolean)
                revalue = (bool)value ? "1" : "0";
            else if (DataType == DbType.Int16 || DataType == DbType.Int32 || DataType == DbType.Int64)
                revalue = value.ToString();
            else if (value == null)
                revalue = "NULL";
            else
                revalue = string.Format("'{0}'", value);

            return revalue;

        }
        /// <summary>
        /// 参数的差异处理
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override object ReturnDbParmValue(object value)
        {
            if (value == null)
                return value = "NULL";
            else
                return value;
        }

        public override string SELECTIDENTITY
        {
            get { return " select @@IDENTITY"; }
            set { }
        }


    }
}
