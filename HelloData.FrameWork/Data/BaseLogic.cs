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
        /// <returns></returns>
        public virtual int Remove(T entity)
        {
            BaseEntity baseEntity = entity as BaseEntity;
            if (baseEntity != null)
                using (DeleteAction delete = new DeleteAction(baseEntity))
                {
                    delete.SqlWhere(Entity.KeyId, baseEntity.KeyIDValue);
                    delete.Excute();
                    return delete.ReturnCode;
                }
            return 0;
        }
        /// <summary>
        /// 获取一个model
        /// </summary>
        /// <param name="keyvalue"></param>
        /// <returns></returns>
        public virtual T FindById(object keyvalue)
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
        public virtual int Add(T entity)
        {
            using (InserAction insert = new InserAction(entity as BaseEntity))
            {
                return insert.Excute().ReturnCode;
            }
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        public virtual int Save(T entity)
        {
            BaseEntity baseEntity = entity as BaseEntity;
            if (baseEntity != null)
                using (UpdateAction update = new UpdateAction(baseEntity))
                {
                    update.SqlWhere(Entity.KeyId, baseEntity.KeyIDValue);
                    update.Excute();
                    return update.ReturnCode;
                }
            return 0;
        }
        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="objects"></param>
        /// <returns></returns>
        public virtual PageList<T> FindList(int pageIndex, int pageSize, params QueryField[] querys)
        {
            using (SelectAction select = new SelectAction(Entity))
            {
                if (querys.Length > 0)
                    select.SqlWhere(querys);
                select.SqlPageParms(pageSize);
                return select.QueryPage<T>(pageIndex);
            }
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
