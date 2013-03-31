namespace HelloData.FrameWork.Data
{

    public class OrderField
    {
        public string FiledName { get; set; }
        public OrderByEnum Order { get; set; }
    }
    /// <summary>
    /// 查询或者更新或者删除的where条件
    /// </summary>
    public class WhereField
    {
        public WhereField()
        {
            Relation = RelationEnum.Equal;
            Condition = ConditionEnum.And;
        }

        public string FiledName { get; set; }
        public object Value { get; set; }
        public object Value2 { get; set; }
        public RelationEnum Relation { get; set; }
        public ConditionEnum Condition { get; set; }
        /// <summary>
        /// 是否是where条件
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
