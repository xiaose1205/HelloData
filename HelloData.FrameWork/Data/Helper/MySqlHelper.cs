using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;

namespace HelloData.FrameWork.Data
{
    public class MySqlHelper : DataBase
    {
        public MySqlHelper() : base() { }
        public MySqlHelper(string conn)
            : base(conn)
        {
            base.IsOpenTrans = false;
        }

        public override string ProviderName
        {
            get
            {
                return "MySql.Data.MySqlClient";
            }
        }
        public override string CreatePageString(string tablename, string colums, string where, string order, int pageindex, int pagesize, out string selcountstr)
        {
            if (string.IsNullOrEmpty(colums))
                colums = "*";
            if (string.IsNullOrEmpty(where))
                where = string.Empty;
            else
                where = " where " + where.Trim().Substring(3);

            if (pageindex == 0)
            {
                selcountstr = string.Empty;
                string top = string.Empty;
                if (pagesize != -1)
                    top = " limit  " + pagesize + " ";
                //直接查询指定的数目的数据
                if (string.IsNullOrEmpty(order))
                    return (string.Format("SELECT  {0} from {1}   {2}   " + top + " ;",
                        colums, tablename, where));
                return (string.Format("SELECT {0} from {1}   {2} ORDER BY {3} " + top + "  ;",
                                       colums, tablename, @where, order));
            }
            else
            {
                selcountstr = (string.Format("select count(1) from {0}   {1}", tablename, where));
                if (pagesize == -1)
                    pagesize = 20;
                int skip = pageindex;
                if (pageindex > 0)
                {
                    skip = pageindex - 1;
                }
                skip = pagesize * skip;
                if (string.IsNullOrEmpty(order))
                    return (string.Format("select {0} from {1}   {4}   limit {2}, {3}",
                  colums, tablename, skip, pagesize, where));
                else
                    return (string.Format("select {1} from {2}   {5} order by {0}  limit {3} ,{4}",
                    order, colums, tablename, skip, pagesize, where));

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
            get { return " SELECT LAST_INSERT_ID(); "; }
            set { }
        } 
    }
}
