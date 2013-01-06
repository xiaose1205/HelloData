using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HelloData.FrameWork.Data
{
    /// <summary>
    /// manage接口类
    /// </summary>
    /// <typeparam name="BaseEntity"></typeparam>
    public interface IRepository<T> : IDisposable
    {
        /// <summary>
        /// 删除一个实体类
        /// </summary>
        /// <param name="entity"></param>
        int Delete(object key);
        /// <summary>
        /// 获取一个实体类
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        T Get(object id);
        /// <summary>
        /// 插入一个实体类
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        object Save(T entity);
        /// <summary>
        /// 更新一个实体类
        /// </summary>
        /// <param name="entity"></param>
        int Update(T entity);
        /// <summary>
        /// 获取分页后代码
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="objects"></param>
        /// <returns></returns>
        PageList<T> GetList(int pageIndex, int pageSize, params object[] objects);
        /// <summary>
        /// 设置其他的参数，sqlkeyvalue(obj,obj)之类
        /// </summary>
        /// <returns></returns>
        DataBaseAction GetOtherParms();
    }
}
