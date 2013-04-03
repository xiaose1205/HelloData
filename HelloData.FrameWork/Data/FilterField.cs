namespace HelloData.FrameWork.Data
{

    public class OrderField
    {
        /// <summary>
        /// 操作的列名
        /// </summary>
        public string FiledName { get; set; }
        public OrderByEnum Order { get; set; }
    }
    /// <summary>
    /// 查询或者更新或者删除的where条件
    /// </summary>
    public class QueryField
    {
        public QueryField()
        {
            Relation = RelationEnum.Equal;
            Condition = ConditionEnum.And;
        }
        /// <summary>
        /// 操作的列名
        /// </summary>
        public string FiledName { get; set; }
        /// <summary>
        /// 第一个参数
        /// </summary>
        public object Value { get; set; }
        /// <summary>
        /// 第二个参数，用于betweent
        /// </summary>
        public object Value2 { get; set; }
        /// <summary>
        /// 常用的关系，〉〈 in != like  等
        /// </summary>
        public RelationEnum Relation { get; set; }
        /// <summary>
        /// 语句链接关系and or 
        /// </summary>
        public ConditionEnum Condition { get; set; }
        /// <summary>
        /// 是否是where条件,否则value值为普通的sql语句，例如：and  username='wangjun'
        /// </summary>
        public bool IsWhereField = true;
    }
    /// <summary>
    /// 添加或者修改的值
    /// </summary>
    public class ValueField
    {
        public string FiledName { get; set; }
        public object Value { get; set; }
        /// <summary>
        /// 0:表示用filedName+value,1:表示直接用filedname(update参数自定义)
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 数据类型
        /// </summary>
        public System.Data.DbType DataType { get; set; }
    }


}
