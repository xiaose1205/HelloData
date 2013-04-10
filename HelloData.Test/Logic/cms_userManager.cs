using System;
using System.Collections.Generic;
using HelloData.FrameWork.Data;
using HelloData.FrameWork.Data.Enum;
using HelloData.Test.Entity;

namespace HelloData.Test.Logic
{

    public class cms_userManager : BaseManager<cms_userManager, cms_user>
    {
        #region  新增的demo
        /// <summary>
        /// 实体对象直接插入
        /// </summary>
        public void AddDemo()
        {
            cms_user user = new cms_user();
            user.username = "wangjun";
            user.password = "123456";
            user.phone = "13800138000";
            using (InserAction action = new InserAction(user))
                action.Excute();
        }
        /// <summary>
        /// 按照指定的条件插入
        /// </summary>
        public void AddDemo1()
        {

            using (InserAction action = new InserAction(Entity))
            {
                action.SqlKeyValue(cms_user.Columns.username, "wangjun");
                action.SqlKeyValue(cms_user.Columns.password, "123456");
                action.Excute();
            }
        }
        /// <summary>
        /// linq方式指定条件插入
        /// </summary>
        public void AddDemo2()
        {
            using (InserAction action = new InserAction(Entity))
            {
                action.Cast<cms_user>()
                    .SqlValue(u => u.username == "wangjun" && u.password == "123456")
                      .UnCast().Excute();
            }
        }
        /// <summary>
        /// 批量插入
        /// </summary>
        public void AddDemo3()
        {
            using (InserAction action = new InserAction(new NullEntity()))
            {
                action.Cast().InsertList(() => new List<BaseEntity>()
                    {
                    new cms_user(){username="wangjun",password="123456"},
                    new TestUser(){}
                    });
            }
        }
        /// <summary>
        /// 查询一个实体
        /// </summary>
        /// <returns></returns>
        public cms_user SelectDemo()
        {
            using (SelectAction action = new SelectAction(Entity))
            {
                action.SqlWhere(cms_user.Columns.username, "wang", RelationEnum.LikeLeft);
                return action.QueryEntity<cms_user>();
            }
        }
        public cms_user SelectDemo1()
        {
            using (SelectAction action = new SelectAction(Entity))
            {
                action.Cast<cms_user>()
                      .Where(user1 => user1.username == "wangjun");
                return action.QueryEntity<cms_user>();
            }
        }
        /// <summary>
        /// 获取第3组30条数据
        /// </summary>
        /// <returns></returns>
        public PageList<cms_user> SelectDemo2()
        {
            using (SelectAction action = new SelectAction(Entity))
            {
                action.SqlPageParms(30).Cast<cms_user>()
                      .Where(user1 => user1.username == "wangjun");
                return action.QueryPage<cms_user>(3);
            }
        }
        /// <summary>
        /// 事务操作
        /// </summary>
        public void TranstionDemo()
        {
            MultiAction actions = new MultiAction();
            for (int i = 0; i < 10; i++)
            {
                if (i % 4 == 0)
                {
                    DeleteAction delete = new DeleteAction(Entity);
                    delete.Cast<cms_user>().Where(u => u.username == "wangjun");
                    actions.AddAction(delete);
                }
                if (i % 4 == 1)
                {
                    UpdateAction update = new UpdateAction(Entity);
                    update.Cast<cms_user>()
                        .Where(u => u.username == "wangjun")
                        .UnCast()
                        .SqlKeyValue(cms_user.Columns.password, "1234567");
                    actions.AddAction(update);
                }
            }
            try
            {
                actions.Commit();
            }
            catch (Exception)
            {
                actions.Rollback();
            }
        }
        /// <summary>
        /// 自定义视图
        /// </summary>
        public cms_user viewtestModel()
        {
            using (SelectAction action = new SelectAction(""))
            {
                action.SqlClomns = "_cms_user.*,_cms_manager.name as  managername";
                {
                    //添加视图的关联关系
                    List<QueryField> field = new List<QueryField>();
                    field.Add(new QueryField() { FiledName = "mangerid", Condition = ConditionEnum.And, Value = "id" });
                    action.AddJoin(ViewJoinEnum.leftjoin, "cms_user", "cms_manager", field);
                }


                action.SqlWhere(
                    new QueryField { FiledName = cms_user.Columns.username, Value = "wangjun" });
                action.SqlWhere(cms_user.Columns.password, "123456");
                PageList<cms_user> lists = action.QueryPage<cms_user>(1);
                return null;
            }
        }
        /// <summary>
        /// 创建当前的sql
        /// </summary>
        /// <returns></returns>
        public string CreateSql()
        {
            UpdateAction update = new UpdateAction(Entity);
            update.Cast<cms_user>()
                .Where(u => u.username == "wangjun")
                .UnCast()
                .SqlKeyValue(cms_user.Columns.password, "1234567");
            return update.CreateSql(OperateEnum.Update);
        }

        #endregion
    }

}