using System.Collections.Generic;

namespace HelloData.FrameWork.Data
{
    /// <summary>
    /// 全局的数据操控池
    /// </summary>
    public class DataPools
    {
        class set
        {
            static set()
            {
            }
            internal static readonly DataPools Instance = new DataPools();
        }

        public static DataPools Current
        {
            get { return set.Instance; }
        }
        public List<AppDatabase> AppDatabaseList;
        public DataPools()
        {
            AppDatabaseList = new List<AppDatabase>();
        }
        /// <summary>
        /// 新增一个数据池
        /// </summary>
        /// <param name="model"></param>
        public void AddAppDatabase(AppDatabase model)
        {
            AppDatabaseList.Add(model);
        }
        /// <summary>
        /// 获取指定一个数据池
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public AppDatabase GetDatabase(int index)
        {
            return AppDatabaseList.Count - 1 >= index ? AppDatabaseList[index] : null;
        }
    }
}
