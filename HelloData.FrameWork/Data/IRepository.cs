using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HelloData.FrameWork.Data
{
    /// <summary>
    /// manage接口类
    /// </summary>
    public interface IRepository<T> : IDisposable
    {
        /// <summary>
        /// 删除一个实体类
        /// </summary>
        /// <param name="entity"></param>
        int Remove(T entity);
        /// <summary>
        /// 获取一个实体类
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        T FindById(object id);
        /// <summary>
        /// 插入一个实体类
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int Add(T entity);
        /// <summary>
        /// 更新一个实体类
        /// </summary>
        /// <param name="entity"></param>
        int Save(T entity);
        /// <summary>
        /// 获取分页后代码
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="querys"></param>
        /// <returns></returns>
        PageList<T> FindList(int pageIndex, int pageSize, params QueryField[] querys);

    }
}
