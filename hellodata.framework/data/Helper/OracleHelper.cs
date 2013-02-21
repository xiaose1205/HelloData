#region Version Info
/* ========================================================================
* 【本类功能概述】
* 
* 作者：王军 时间：2013/1/16 23:26:55
* 文件名：OracleHelper
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
using System.Data;
using System.Linq;
using System.Text;

namespace HelloData.FrameWork.Data
{
    public class OracleHelper : DataBase
    {
        public OracleHelper() : base() { }
        public OracleHelper(string conn)
            : base(conn)
        {
            base.IsOpenTrans = false;
        }
        public override string ProviderName
        {
            get
            {
               // return "MySql.Data.MySqlClient";
                return "System.Data.OracleClient";
            }
        }

        //select * from in_c_op202currenttotal where rowid in(select rid from 
        //(select rownum rn,rid from
        //(select rowid rid,reportdate from 
        //in_c_op202currenttotal  order by reportdate desc) 
        //where rownum<=1000) where rn>=990)
        //order by reportdate desc;

        //select * from {0} where rowid in(select rid from 
        //(select rownum rn,rid from
        //(select rowid rid,{1} from 
        // {0} {5}  {2}) 
        //where rownum<={3}) where rn>={4})
        //{2};
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
        public override string CreatePageString(string tablename, string colums, string where, string groupby, string order, int pageindex, int pagesize, out string selcountstr)
        {
            if (string.IsNullOrEmpty(tablename))
                throw new Exception("必须输入表名");
            if (string.IsNullOrEmpty(colums))
                colums = "*";
            if (string.IsNullOrEmpty(where))
                where = string.Empty;
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
                    top = " rownum < " + pagesize + " ";
                //直接查询指定的数目的数据
                if (string.IsNullOrEmpty(order))
                    return (string.Format("SELECT   {0} from {1}   {2}   " + groupbystr + "  ;",
                        colums, tablename, string.IsNullOrEmpty(where) ? "where " + top : where + top));
                return (string.Format("SELECT   {0} from {1}   {2}  " + groupbystr + " ORDER BY {3}  ;",
                                      colums, tablename, string.IsNullOrEmpty(@where) ? "where " + top : @where + top, order));
            }
            selcountstr = string.Format("select count(1) from {0}   {1}  " + groupbystr + "", tablename, @where);

            if (pagesize == -1)
                pagesize = 20;
            int skip = pageindex;
            if (pageindex > 0)
                skip = pageindex - 1;

            skip = pagesize * skip;
            return string.Format("  select {6} from {0} where rowid in" +
                   "(select rid from  (select rownum rn,rid from " +
                   "(select rowid rid {1} from  {0} {5}  {2}) " +
                   " where rownum<={3}) where rn>={4}) " +
                   "{2}", tablename,
                   string.IsNullOrEmpty(order) ? string.Empty : "," + order.ToLower().Replace(" desc", "").Replace(" asc", "")
                   , string.IsNullOrEmpty(order) ? string.Empty : "order by " + order,
                   pageindex * pagesize, skip, where, colums);
        }

        public override string ReturnDBValue(DbType dataType, object value)
        {
            string revalue;
            if (dataType == DbType.Boolean)
                revalue = (bool)value ? "1" : "0";
            else if (dataType == DbType.Int16 || dataType == DbType.Int32 || dataType == DbType.Int64)
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
                return "NULL";
            return value;
        }

        public override string SELECTIDENTITY
        {
            get { return " select @@IDENTITY"; }
            set { }
        }

    }
}
