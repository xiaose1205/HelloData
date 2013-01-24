using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HelloData.FrameWork.Data.Enum;

namespace HelloData.FrameWork.Data
{
    public abstract class BaseEntity : IDisposable
    {
        public TableTyleEnum TableType = TableTyleEnum.Table;

        /// <summary>
        /// 主键值
        /// </summary>
        [Column(NoSqlProperty = true)]
        public object KeyIDValue
        {
            get
            {
                var tType = this.GetType();
                var pInfos = tType.GetProperties();
                foreach (var property in pInfos)
                {
                    if (property.Name.ToLower() == KeyId.ToLower())
                    {
                        return property.GetValue(this, null);
                    }
                }
                return null;
            }

        }
        /// <summary>
        /// 当前model的错误信息值（可以用导出或者在线数据纠正）
        /// </summary>
        [Column(NoSqlProperty = true)]
        public string ErrorMsg
        {
            get;
            set;
        }
        [Column(NoSqlProperty = true)]
        public string TableName
        {
            get;
            set;
        }
        public virtual void SetIni(object entity, string tablename, string key)
        {
            TableName = tablename;
            KeyId = key;
        }
        /// <summary>
        /// 默认主键为id
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="tablename"></param>
        public virtual void SetIni(object entity, string tablename)
        {
            TableName = tablename;
            KeyId = "id";
        }
        internal string KeyId { get; set; }
        public void Dispose()
        {

        }
        ~BaseEntity()
        {

        }
        /// <summary>
        /// 转换成json格式
        /// </summary>
        /// <returns></returns>
        public string ToJsonString()
        {
            return JsonHelper.SerializeObject(this);
        }

    }
    public class ColumnAttribute : System.Attribute
    {


        /// <summary>
        /// 是否自增
        /// </summary>
        public bool AutoIncrement { get; set; }

        /// <summary>
        /// 是否虚拟属性
        /// </summary>
        public bool NoSqlProperty { get; set; }

        /// <summary>
        /// 是否为主键
        /// </summary>
        public bool IsKeyProperty { get; set; }

        public ColumnAttribute()
        {
            AutoIncrement = false;
            IsKeyProperty = false;
            NoSqlProperty = false;
        }
    }
}
