using System.Collections.Generic;
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

        private List<BaseEntity> _entities;

        /// <summary>
        /// 操作多个insert
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public DataBaseAction InsertList(List<BaseEntity> entities)
        {
            _entities = entities;
            return this;
        }

        public override DataBaseAction Excute()
        {
            if (_entities != null && _entities.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var baseEntity in _entities)
                {
                    this.ResetAction(baseEntity);
                    sb.AppendLine(BuildSql());
                }
                ReturnCode = DbHelper.ExecuteSql(sb.ToString());
                return this;
            }

            Cache.CacheHelper.RemoveByPreFix(string.Format("entity_{0}", this.TbName));
            DbHelper.Parameters = this.Parameters;
            ReturnCode = DbHelper.ExecuteSql(BuildSql());
            return this;

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