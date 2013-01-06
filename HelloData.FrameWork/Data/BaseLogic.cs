using System;

namespace HelloData.FrameWork.Data
{
    public abstract class BaseLogic<T> : IRepository<T>, IDisposable where T : new()
    {
        protected BaseLogic()
        {
            t = new T();
            Entity = t as BaseEntity;
        }
        public BaseEntity Entity;

        /// <summary>
        /// 执行删除操作
        /// </summary>
        /// <param name="keyvalue"> </param>
        /// <returns></returns>
        public virtual int Delete(object keyvalue)
        {
            using (DeleteAction delete = new DeleteAction(Entity))
            {
                if (keyvalue != null)
                    delete.SqlWhere(Entity.KeyId, keyvalue);
                delete.Excute();
                return delete.ReturnCode;
            }
        }
        /// <summary>
        /// 获取一个model
        /// </summary>
        /// <param name="keyvalue"></param>
        /// <returns></returns>
        public virtual T Get(object keyvalue)
        {
            using (SelectAction select = new SelectAction(Entity))
            {
                select.SqlWhere(Entity.KeyId, keyvalue);
                return select.QueryEntity<T>();
            }
        }
        /// <summary>
        /// 保存（粗略的保存，保存实体类有值的数据）
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual object Save(T entity)
        {
            using (InserAction insert = new InserAction(entity as BaseEntity))
            {
                insert.Excute();
                return 1;
            }
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        public virtual int Update(T entity)
        {
            using (UpdateAction update = new UpdateAction(entity as BaseEntity))
            {
                BaseEntity baseEntity = entity as BaseEntity;
                if (baseEntity != null) update.SqlWhere(Entity.KeyId, baseEntity.KeyIDValue);
                update.Excute();
                return update.ReturnCode;
            }
        }
        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="objects"></param>
        /// <returns></returns>
        public virtual PageList<T> GetList(int pageIndex, int pageSize, params object[] objects)
        {
            using (SelectAction select = new SelectAction(Entity))
            {
                select.SqlPageParms(pageSize);
                return select.QueryPage<T>(pageIndex);
            }
        }
        /// <summary>
        /// 设置其他的参数，sqlkeyvalue(obj,obj)之类
        /// </summary>
        /// <returns></returns>
        public virtual DataBaseAction GetOtherParms()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 操作的基础model
        /// </summary>
        public T t { get; set; }


        public void Dispose()
        {
            Entity = null;
        }
    }
}
