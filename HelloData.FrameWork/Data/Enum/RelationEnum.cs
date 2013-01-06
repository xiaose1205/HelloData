namespace HelloData.FrameWork.Data
{
    public enum RelationEnum
    {
        /// <summary>
        /// 没有关系，适用于直接写where语句
        /// </summary>
        None,
        /// <summary>
        /// 不等于
        /// </summary>
        NoEqual,
        /// <summary>
        /// 等于
        /// </summary>
        Equal,
        /// <summary>
        /// 大于
        /// </summary>
        Large,
        /// <summary>
        /// 大于等于
        /// </summary>
        LargeThen,
        /// <summary>
        /// 小于
        /// </summary>
        Less,
        /// <summary>
        /// 小于等于
        /// </summary>
        LessThen,
        /// <summary>
        /// Like
        /// </summary>
        Like,
        /// <summary>
        /// Like
        /// </summary>
        LikeLeft,
        /// <summary>
        /// Like
        /// </summary>
        LikeRight,
        /// <summary>
        /// <>
        /// </summary>
        NoLike,
        /// <summary>
        /// 
        /// </summary>
        Between,
        /// <summary>
        /// in
        /// </summary>
        In,
        /// <summary>
        /// not in
        /// </summary>
        NotIn,
        /// <summary>
        /// is null
        /// </summary>
        IsNull,
        /// <summary>
        ///  is not null
        /// </summary>
        IsNotNull

    }

}
