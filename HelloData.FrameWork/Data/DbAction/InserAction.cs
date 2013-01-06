using System.Text;

namespace HelloData.FrameWork.Data
{
    /// <summary>
    /// 插入
    /// </summary>
    public class InserAction : DataBaseAction
    {
        public InserAction(BaseEntity entity, int index = 0)
            : base(entity, index)
        {
            CurrentOperate = OperateEnum.Insert;
        }
        public InserAction(string tbName, int index = 0)
            : base(tbName, index)
        {
            CurrentOperate = OperateEnum.Insert;
        }
        private string BuildSql()
        {
            return CreateSql(CurrentOperate);
        }

        public override void Excute()
        {

            Cache.CacheHelper.RemoveByPreFix(string.Format("entity_{0}", this.TbName));
            DbHelper.Parameters = this.Parameters;
            ReturnCode = DbHelper.ExecuteSql(BuildSql());

        }
        /// <summary>
        /// sql特有的 select @@IDENTITY
        /// </summary>
        public void ExcuteIdentity()
        {

            Cache.CacheHelper.RemoveByPreFix(string.Format("entity_{0}", this.TbName));
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(BuildSql());
            sb.AppendLine(DbHelper.SELECTIDENTITY);
            DbHelper.Parameters = this.Parameters;
            ReturnCode = int.Parse(DbHelper.GetSingle(sb.ToString()).ToString());

        }

    }
}