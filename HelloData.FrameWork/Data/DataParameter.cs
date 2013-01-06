using System.Data;

namespace HelloData.FrameWork.Data
{
    /// <summary>
    /// 数据库参数
    /// </summary>
    public class DataParameter : IDbDataParameter
    {
        public DataParameter()
        {
            Direction = ParameterDirection.Input;
            SourceVersion = DataRowVersion.Default;
            DbType = DbType.String;
            Size = 50;
            Scale = 0;
            Precision = 0;
        }

        public DataParameter(string paraName, object value)
            : this()
        {
            ParameterName = paraName;
            Value = value;
        }
        /// <summary>
        /// 精度
        /// </summary>
        public byte Precision
        { get; set; }
        /// <summary>
        /// 数值范围
        /// </summary>
        public byte Scale
        { get; set; }
        /// <summary>
        /// 大小
        /// </summary>
        public int Size
        { get; set; }

        /// <summary>
        /// 输入输出方向
        /// </summary>
        public ParameterDirection Direction
        { get; set; }

        /// <summary>
        /// 数据类型
        /// </summary>
        public DbType DbType
        { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public object Value
        { get; set; }

        /// <summary>
        /// 可以为空
        /// </summary>
        public bool IsNullable
        { get; set; }
        public DataRowVersion SourceVersion
        { get; set; }

        /// <summary>
        /// 参数名称
        /// </summary>
        public string ParameterName
        { get; set; }
        /// <summary>
        /// 来源列
        /// </summary>
        public string SourceColumn { get; set; }
    }
}
