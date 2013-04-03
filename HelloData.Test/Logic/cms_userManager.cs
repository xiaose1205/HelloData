using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HelloData.FrameWork.Data;
using HelloData.FrameWork.Data.Enum;
using HelloData.Test.Entity;

namespace HelloData.Test.Logic
{

    public class cms_userManager : BaseManager<cms_userManager, cms_user>
    {
        internal void MulitTest(string ids)
        {
            string[] str = ids.Split(',');
            MultiAction mulut = new MultiAction();
            DataBaseAction action = new DataBaseAction();
            foreach (var item in str)
            {
                DeleteAction delete = new DeleteAction(Entity);
                /// delete.SqlWhere(cms_user.Columns.id, item);
                delete.Cast<cms_user>().Where(user => user.isactive == 1);
                mulut.AddAction(delete);
                action = delete;
            }
            //模拟移除一个操作

            try
            {
                mulut.Commit();
            }
            catch
            {
                mulut.Rollback();
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
                new QueryField { FiledName = "", Value = "" },
                new QueryField { FiledName = "", Value = "" }
            );
                action.SqlWhere(cms_user.Columns.username, "admin");
                action.SqlWhere(cms_user.Columns.password, "123456");
                PageList<cms_user> lists = action.QueryPage<cms_user>(1);
                return null;
            }
        }

        /// <summary>
        /// 删除多个数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        internal int DeleteMuilt(string ids)
        {
            //第一个数据库的操作
            using (DeleteAction delete = new DeleteAction(Entity, 0))
            {
                delete.SqlWhere(cms_user.Columns.id, "1,2,3,4,5", RelationEnum.In);
                delete.Excute();
                return delete.ReturnCode;
            }
            //第二个数据库的操作
            using (DeleteAction delete = new DeleteAction(Entity, 1))
            {
                delete.SqlWhere(cms_user.Columns.id, ids, RelationEnum.In);
                delete.Excute();
                return delete.ReturnCode;
            }
        }
        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        internal int UpdatePwd(string ids)
        {

            using (UpdateAction update = new UpdateAction(Entity))
            {
                update.SqlKeyValue(cms_user.Columns.createtime, null);
                update.SqlKeyValue(cms_user.Columns.password, "123456123");
                update.Cast<cms_user>()
                    .SqlValue(user => new cms_user { password = "123456", createtime = DateTime.Now })
                    .Where(user1 => user1.isadmin)
                    .UnCast().Excute();
                return update.ReturnCode;
            }
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        internal int InsertNew(cms_user model)
        {

            using (InserAction action = new InserAction("cms_user"))
            {
                action.SqlKeyValue(cms_user.Columns.username, model.username);
                action.SqlKeyValue(cms_user.Columns.password, model.password);
                return action.Excute().ReturnCode;
            }
            //或者以下写法

            using (InserAction action = new InserAction(model))
            {
                return action.Cast<cms_user>()
                       .SqlInValues(user =>
                                 new List<cms_user>()
                                    {
                                        new cms_user {password = "12345", phone = "123456"},
                                        new cms_user {phone = "123242343"}
                                    }
                     ).UnCast().Excute().ReturnCode;
            }
        }




        internal cms_user Get(string username, string password)
        {
            using (SelectAction select = new SelectAction(Entity))
            {

                select.SqlWhere(cms_user.Columns.username, "1", "2", ConditionEnum.And, RelationEnum.Between)
                       .SqlWhere(cms_user.Columns.password, password)
                       .SqlWhere(cms_user.Columns.isactive, true)
                       .SqlOrderBy(cms_user.Columns.createtime,OrderByEnum.Desc)
                       .SqlPageParms(1);
                return select.QueryEntity<cms_user>();
            }
        }

        internal cms_user getUser(string id)
        {
            using (SelectAction select = new SelectAction(Entity))
            {
                select.SqlWhere(cms_user.Columns.id, id, RelationEnum.LikeRight);
                select.SqlWhere(cms_user.Columns.isactive, true);
                select.SqlPageParms(1);
                select.Cast<cms_user>().
                     OrderBy(user => user.logintime, OrderByEnum.Asc)
                    .OrderBy(ui => ui.phone, OrderByEnum.Asc)
                    .GroupBy(u => new object[] { u.isadmin, u.logintime })
                    .Where(o => o.password == "12321" && o.logintime == DateTime.Now)
                   ;// .UnCast();

                return select.QueryEntity<cms_user>();
            }
        }

        public int UpdateNewPwd(string p)
        {
            cms_userManager.Instance.FindList(0, 2);

            using (UpdateAction update = new UpdateAction(Entity))
            {
                update.SqlWhere("  and  (id=1  or id=2)");
                update.SqlWhere(cms_user.Columns.id, 1);
                update.SqlKeyValue(cms_user.Columns.password, p);
                update.Excute();
                return update.ReturnCode;
            }

            using (TradAction update = new TradAction(Entity))
            {
                //update.ExcuteStoredProcedure("storename",DataParameter[]);
            }

        }
        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="objects"></param>
        /// <returns></returns>
        public PageList<cms_user> GetList(string username, int pageIndex, int pageSize)
        {
            using (SelectAction select = new SelectAction(Entity))
            {
                if (!string.IsNullOrEmpty(username))
                    select.SqlWhere(cms_user.Columns.username, username, RelationEnum.Like, ConditionEnum.Or);
                select.SqlPageParms(pageSize);
                return select.QueryPage<cms_user>(pageIndex);
            }
        }
    }

}