using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Data;
using System.Reflection;
using HelloData.FrameWork.Data.Enum;
namespace HelloData.FrameWork.Data
{

    public class LinqQueryAction<T>
    {

        internal DataBaseAction CAction { get; set; }
 
    }
    /// <summary>
    /// 具体的命令操作 ，反射实体类的具体表名及字段名
    /// </summary> 
    public class DataBaseAction : IDisposable
    {
        public LinqQueryAction<T> Cast<T>()
        {
            return new LinqQueryAction<T>() { CAction = this };
        }
        private int _appindex;
        public DataBaseAction()
        {
            _appindex = 0;
            Sqlcom = new SqlCompilation(DbHelper);
        }

        public DataBaseAction(int index)
        {
            _appindex = index;
            Sqlcom = new SqlCompilation(DbHelper);
        }

        /// <summary>
        /// 设置表名
        /// </summary>
        /// <param name="entity"></param> 
        public DataBaseAction(BaseEntity entity)
            : this(entity, 0)
        {
        }

        /// <summary>
        /// 设置表名
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="index"> </param>
        public DataBaseAction(BaseEntity entity, int index = 0)
        {
            _appindex = index;
            Sqlcom = new SqlCompilation(DbHelper);
            _entity = entity;
            GetTableNameFromEntity(entity);
        }

        /// <summary>
        /// 设置表名,性能优于 DataAction(BaseEntity entity)
        /// </summary>
        /// <param name="tbName"></param>
        /// <param name="index"> </param>
        public DataBaseAction(string tbName, int index = 0)
        {
            TbName = tbName;
            _appindex = index;
            Sqlcom = new SqlCompilation(DbHelper);
        }

        /// <summary>
        /// 重新设置当前操作
        /// </summary>
        /// <param name="entity"></param> 
        public DataBaseAction ResetAction(BaseEntity entity)
        {
            return ResetAction(entity, 0);
        }

        /// <summary>
        /// 重新设置当前操作
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="index"> </param>
        public DataBaseAction ResetAction(BaseEntity entity, int index = 0)
        {
            _appindex = index;
            Sqlcom = new SqlCompilation(DbHelper);
            ClearPams();
            _entity = entity;
            GetTableNameFromEntity(entity);

            return this;
        }
        /// <summary>
        /// 重新设置当前操作,所有的设置必须重新设置
        /// </summary>
        /// <param name="tbName"></param>
        public void ResetAction(string tbName)
        {
            ClearPams();

            TbName = tbName;
        }
        ~DataBaseAction()
        {
            if (_dbHelper != null)
                _dbHelper.Dispose();
            GC.SuppressFinalize(this);
        }
        private Guid _actionkey = Guid.Empty;
        /// <summary>
        /// 每个操作action的唯一标示
        /// </summary>
        public Guid ActionKey
        {
            get
            {
                if (_actionkey == Guid.Empty)
                    _actionkey = Guid.NewGuid();
                return _actionkey;
            }
        }


        private BaseEntity _entity = null;
        protected string TbName = string.Empty;
        /// <summary>
        /// insert or update的key——value值
        /// </summary>
        private List<ValueField> _keyValues = new List<ValueField>();
        private List<OrderField> _orderValues = new List<OrderField>();
        private string _groupByValue = string.Empty;
        private DataBase _dbHelper;

        public SqlCompilation Sqlcom = new SqlCompilation();
        private StringBuilder _whereStr = new StringBuilder();
        /// <summary>
        /// 设置当前action的操作参数集
        /// </summary>
        public List<DataParameter> Parameters = new List<DataParameter>();

        /// <summary>
        /// 获取当前系统运行的唯一的数据处理对象
        /// </summary>
        public DataBase DbHelper
        {
            get { return _dbHelper ?? (_dbHelper = DataPools.Current.GetDatabase(_appindex).DbBase); }
        }

        private void GetTableNameFromEntity(object entity)
        {
            if (entity != null)
                TbName = ((BaseEntity)entity).TableName;
        }

        #region 设置系统的参数
        /// <summary>
        /// 用户 update的设置值(例如：name='wangjun')
        /// </summary>
        /// <returns></returns>
        public DataBaseAction SqlKeyValue(string filedNameWithValue)
        {
            if (filedNameWithValue == null) return this;
            _keyValues.Add(new ValueField()
            {
                FiledName = filedNameWithValue,
                Value = string.Empty,
                Type = 1
            });

            return this;
        }
        /// <summary>
        /// 主键是否自动生成，fasle表示需要动态创建，默认为自增或者系统自给
        /// </summary>
        public bool IsKeyAuto = true;

        public DataBaseAction SqlKeyValue(params ValueField[] valueFields)
        {
            foreach (ValueField valueField in valueFields)
            {
                SqlKeyValue(valueField.FiledName, valueField.Value);
            }
            return this;
        }

        /// <summary>
        /// 用户insert ,update的设置值(例如："name"    "wangjun")
        /// </summary>
        /// <param name="filedName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public DataBaseAction SqlKeyValue(string filedName, object value)
        {
            if ((IsKeyAuto && (_entity != null && filedName == _entity.KeyId)))
                return this;
            if (!AppCons.IsParmes)
            {
                _keyValues.Add(new ValueField()
                {
                    FiledName = filedName,
                    Value = value,
                    DataType = _dbHelper.ConvertToDbType(value),
                    Type = 0
                });
            }
            else
            {
                string parmeterName = DbHelper.GetParameterName(filedName);
                DbType dbtype = _dbHelper.ConvertToDbType(value);
                _keyValues.Add(new ValueField()
                {
                    FiledName = filedName,
                    Value = parmeterName,
                    DataType = dbtype,
                    Type = 0
                });
                AddParmarms(parmeterName, dbtype, value);
            }
            return this;
        }


        /// <summary>
        /// 增加条件,适用于等于
        /// </summary>
        /// <param name="filedName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public DataBaseAction SqlWhere(string filedName, object value)
        {
            return SqlWhere(filedName, value, RelationEnum.Equal, ConditionEnum.And);
        }
        /// <summary>
        /// 增加条件,特殊的类似 in，不等于，大于
        /// </summary>
        /// <param name="filedName"></param>
        /// <param name="value"></param>
        /// <param name="relation"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public DataBaseAction SqlWhere(string filedName, object value, RelationEnum relation = RelationEnum.Equal)
        {
            return SqlWhere(filedName, value, null, ConditionEnum.And, relation);
        }
        /// <summary>
        /// 增加条件,特殊的类似 in，不等于，大于
        /// </summary>
        /// <param name="filedName"></param>
        /// <param name="value"></param>
        /// <param name="relation"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public DataBaseAction SqlWhere(string filedName, object value, ConditionEnum condition = ConditionEnum.And, RelationEnum relation = RelationEnum.Equal)
        {
            return SqlWhere(filedName, value, null, condition, relation);
        }
        /// <summary>
        /// 增加条件，适用于between
        /// </summary>
        /// <param name="filedName"></param>
        /// <param name="value"></param>
        /// <param name="value1"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public DataBaseAction SqlWhere(string filedName, object value, object value1, ConditionEnum condition)
        {
            return SqlWhere(filedName, value, value1, condition, RelationEnum.Equal);
        }
        /// <summary>
        /// 缓存的主键的值
        /// </summary>
        internal string Cachekeyvalue = string.Empty;

        public DataBaseAction SqlWhere(params WhereField[] whereFields)
        {
            foreach (WhereField whereField in whereFields)
            {
                SqlWhere(whereField.FiledName, whereField.Value, whereField.Value2, whereField.Condition, whereField.Relation);
            }
            return this;
        }

        /// <summary>
        /// 增加条件 
        /// </summary>
        /// <param name="filedName"></param>
        /// <param name="value"></param>
        /// <param name="value1"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public DataBaseAction SqlWhere(string filedName, object value, object value1, ConditionEnum condition, RelationEnum relation)
        {
            if (!AppCons.IsParmes || relation == RelationEnum.In)
            {
                WhereField wherestr = new WhereField()
                {
                    FiledName = filedName,
                    Value = value,
                    Value2 = value1,
                    Condition = condition,
                    Relation = relation
                };
                wherestr.Value = GetValueRelation(value, relation);
                _whereStr.Append(Sqlcom.CreateWhere(wherestr));
            }
            else
            {
                string parmeterName = DbHelper.GetParameterName(filedName);
                string parmeterName1 = string.Empty;
                if (value1 != null)
                    parmeterName1 = DbHelper.GetParameterName(filedName + "_1");
                WhereField wherestr = new WhereField()
                {
                    FiledName = filedName,
                    Value = parmeterName,
                    Value2 = parmeterName1,
                    Condition = condition,
                    Relation = relation
                };
                AddParmarms(parmeterName, Sqlcom.ConvertToDbType(value), GetValueRelation(value, relation));
                if (value1 != null)
                {
                    AddParmarms(parmeterName1, _dbHelper.ConvertToDbType(value1), value1);
                }
                _whereStr.Append(Sqlcom.CreateWhere(wherestr));
            }

            return this;
        }



        public void AddParmarms(string parmeterName, DbType dbtype, object value)
        {
            foreach (DataParameter dp in Parameters)
            {
                if (dp.ParameterName == parmeterName)
                    return;
            }
            DataParameter parameter = new DataParameter()
            {
                ParameterName = parmeterName,
                DbType = dbtype,
                Value = DbHelper.ReturnDbParmValue(value)
            };
            if (DbType.Boolean == parameter.DbType)
            {
                parameter.Value = (bool)value ? 1 : 0;
                parameter.DbType = DbType.Int32;
            }
            Parameters.Add(parameter);
        }
        private object GetValueRelation(object value, RelationEnum relation)
        {
            if (AppCons.IsParmes)
            {
                switch (relation)
                {
                    case RelationEnum.Like:
                        return string.Format("%{0}%", value);

                    case RelationEnum.LikeLeft:
                        return string.Format("%{0}", value);

                    case RelationEnum.LikeRight:
                        return string.Format("{0}%", value);

                    case RelationEnum.NoLike:
                        return string.Format("{0}", value);
                }
                return value;
            }
            else
            {
                switch (relation)
                {
                    case RelationEnum.Like:
                        return string.Format("'%{0}%'", value);

                    case RelationEnum.LikeLeft:
                        return string.Format("'%{0}'", value);

                    case RelationEnum.LikeRight:
                        return string.Format("'{0}%'", value);

                    case RelationEnum.NoLike:
                        return string.Format("'{0}'", value);
                }
                return value;
            }
        }

        /// <summary>
        /// 直接写sql语句，必须加入链接词，例如and , or
        /// </summary>
        /// <param name="wherestr"></param>
        /// <returns></returns>
        public DataBaseAction SqlWhere(string wherestr, params object[] objects)
        {
            if (objects.Length == 0)
                _whereStr.AppendLine(wherestr);
            else
            {
                object[] values = new object[objects.Length];
                for (int i = 0; i < objects.Length; i++)
                {

                    values[i] = DbHelper.ReturnDBValue(_dbHelper.ConvertToDbType(objects[i]), objects[i]);
                }
                _whereStr.AppendFormat(wherestr, values);
            }
            return this;
        }

        /// <summary>
        /// 设置排序，用于select ,pagelist
        /// </summary>
        /// <param name="filedName"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public DataBaseAction SqlOrderBy(string filedName, OrderByEnum order)
        {
            _orderValues.Add(new OrderField()
            {
                FiledName = filedName,
                Order = order
            });
            return this;
        }

        public DataBaseAction SqlGroupBy(string filedName)
        {
            _groupByValue += filedName + ",";
            return this;
        }
        /// <summary>
        /// 设置分页的pagecount,默认为20,-1表示所有
        /// </summary>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        public DataBaseAction SqlPageParms(int pagesize)
        {
            PageSize = pagesize;
            return this;
        }

        private string _clomns = "*";
        /// <summary>
        /// 查询的列名，默认为*,视图操作时候，将表名加_表示自定义表名
        /// </summary>
        public string SqlClomns
        {
            get
            {
                return _clomns;
            }
            set { _clomns = value; }
        }
        /// <summary>
        /// 生成where 语句
        /// </summary>
        /// <returns></returns>
        protected String CreateWhereStr()
        {
            string wherestr = _whereStr.ToString();
            if (Cache.CacheHelper.IsOpenCache)
                Cachekeyvalue = wherestr.ToLower().Replace(" ", "").Replace("and", "").Replace("or", "").Trim();
            return wherestr;
        }

        /// <summary>
        /// 生成orderby语句
        /// </summary>
        /// <returns></returns>
        protected string CreateOrdeyStr()
        {
            return Sqlcom.CreateOrder(_orderValues);
        }

        /// <summary>
        /// 当keyvalues为空的时候自动生成当前需要的keyvalues
        /// </summary>
        /// <returns></returns>
        public void GetKetValuesWithEntity()
        {
            _keyValues = Sqlcom.GetKeyValues(_entity);
        }

        public DataBaseAction AddJoin(ViewJoinEnum join, BaseEntity table1, BaseEntity table2, List<WhereField> joinfields)
        {
            return AddJoin(join, table1.TableName, table2.TableName, joinfields);
        }
        /// <summary>
        /// 加入join连接
        /// </summary>
        /// <param name="join"></param>
        /// <param name="table1"></param>
        /// <param name="table2"></param>
        /// <param name="joinfields"></param>
        public DataBaseAction AddJoin(ViewJoinEnum join, string table1, string table2, List<WhereField> joinfields)
        {
            if (ViewList == null)
                ViewList = new List<ViewHelper>();
            foreach (var item in ViewList)
            {
                if (item.TableName1 != table1 || item.TableName2 != table2) continue;
                item.Joinfields.AddRange(joinfields);
                return this;
            }
            ViewList.Add(new ViewHelper()
            {
                Join = join,
                TableName1 = table1,
                TableName2 = table2,
                Joinfields = joinfields
            });
            return this;
        }
        /// <summary>
        /// 视图的对象类
        /// </summary>
        private List<ViewHelper> ViewList { get; set; }
        #endregion


        public OperateEnum CurrentOperate = OperateEnum.None;
        public string CreateSql(OperateEnum operate, int pagesize, int pageindex)
        {
            CurrentOperate = operate;
            string where = CreateWhereStr();
            switch (operate)
            {
                case OperateEnum.None:
                    return _tradstr;
                case OperateEnum.Insert:
                    string files = string.Empty;
                    string values = string.Empty;
                    if (_keyValues.Count == 0)
                        GetKetValuesWithEntity();
                    foreach (var item in _keyValues)
                    {
                        if (AppCons.IsParmes)
                        {
                            files += item.FiledName + ",";
                            string parmarmName = DbHelper.GetParameterName(item.FiledName);
                            values += parmarmName + ",";
                            AddParmarms(parmarmName, item.DataType, item.Value);
                        }
                        else
                        {
                            string dbvalue = DbHelper.ReturnDBValue(item.DataType, item.Value);
                            if (!string.IsNullOrEmpty(dbvalue))
                            {
                                files += item.FiledName + ",";
                                values += dbvalue + ",";
                            }
                        }
                    }
                    return string.Format(SqlCompilation.InsertStr, TbName, files.TrimEnd(','), values.TrimEnd(','));

                case OperateEnum.Update:
                    string setvalues = string.Empty;
                    if (_keyValues.Count == 0)
                        GetKetValuesWithEntity();
                    foreach (var item in _keyValues)
                    {
                        switch (item.Type)
                        {
                            case 0:
                                if (AppCons.IsParmes)
                                {
                                    string parmarmName = DbHelper.GetParameterName(item.FiledName);
                                    AddParmarms(parmarmName, item.DataType, item.Value);
                                    setvalues += string.Format("{0}={1},",
                                                               item.FiledName,
                                                               parmarmName);
                                }
                                else
                                {
                                    string dbvalue = DbHelper.ReturnDBValue(item.DataType, item.Value);
                                    if (!string.IsNullOrEmpty(dbvalue))
                                    {
                                        setvalues += string.Format("{0}={1},",
                                                                   item.FiledName,
                                                                   dbvalue);
                                    }
                                }
                                break;
                            default:
                                setvalues += item.FiledName + ",";
                                break;
                        }
                    }
                    return string.Format(SqlCompilation.UpdateStr, TbName, setvalues.TrimEnd(','), where);
                case OperateEnum.Delete:
                    return string.Format(SqlCompilation.DeleteStr, TbName, where);

                case OperateEnum.Select:
                    {
                        string order = CreateOrdeyStr();
                        if (ViewList == null || ViewList.Count == 0)
                            return string.Format(SqlCompilation.SelectStr, SqlClomns,
                                                 TbName,
                                                 where,
                                                 string.IsNullOrEmpty(order)
                                                     ? string.Empty
                                                     : string.Format("order by {0}", order),
                                                 string.IsNullOrEmpty(_groupByValue)
                                                     ? string.Empty
                                                     : string.Format(" group by {0} ", _groupByValue.Trim(',')));
                        //视图的处理
                        StringBuilder jointable = new StringBuilder();
                        List<string> tables = new List<string>();
                        foreach (var item in ViewList)
                        {
                            if (!tables.Contains(item.TableName1))
                            {
                                tables.Add(item.TableName1);
                                jointable.AppendLine(string.Format("{0}  as _{0} ", item.TableName1));
                            }
                            jointable.AppendLine(SqlCompilation.GetJoinEnum(item.Join));

                            jointable.AppendLine(string.Format("{0}  as _{0} ", item.TableName2));
                            for (int i = 0; i < item.Joinfields.Count; i++)
                            {
                                item.Joinfields[i].FiledName = string.Format("_{0}.{1}", item.TableName1,
                                                                             item.Joinfields[i].FiledName);
                                item.Joinfields[i].Value = string.Format("_{0}.{1}", item.TableName2,
                                                                         item.Joinfields[i].Value);
                                if (i == 0)
                                {
                                    jointable.AppendLine("on");
                                    jointable.AppendLine(Sqlcom.CreateWhere(item.Joinfields[i], false));
                                    continue;
                                }
                                jointable.AppendLine(Sqlcom.CreateWhere(item.Joinfields[i]));
                            }
                        }
                        return string.Format(SqlCompilation.SelectStr, SqlClomns,
                                             jointable,
                                             where,
                                             string.IsNullOrEmpty(order)
                                                 ? string.Empty
                                                 : string.Format("order by {0}", order),
                                             string.IsNullOrEmpty(_groupByValue)
                                                 ? string.Empty
                                                 : string.Format(" group by {0} ", _groupByValue.Trim(',')));
                    }
                case OperateEnum.SelectPage:
                    {
                        string order = CreateOrdeyStr();
                        if (ViewList == null || ViewList.Count == 0)
                        {
                            _selcountstr = string.Format("select count(1) from {0}  where  1=1  {1} ", TbName, where);
                            //return string.Format(SqlCompilation.SelectStr, SqlClomns,
                            //                     TbName,
                            //                     where,
                            //                     string.IsNullOrEmpty(order)
                            //                         ? string.Empty
                            //                         : string.Format("order by {0}", order),
                            //                     string.IsNullOrEmpty(_groupByValue)
                            //                         ? string.Empty
                            //                         : string.Format(" group by {0} ", _groupByValue.Trim(',')));
                            return DbHelper.CreatePageString(TbName, SqlClomns,
                                             where, _groupByValue.Trim(','),
                                             string.IsNullOrEmpty(order) ? string.Empty : order
                                        , pageindex, pagesize, out _selcountstr);
                        }
                        //视图的处理
                        StringBuilder jointable = new StringBuilder();
                        List<string> tables = new List<string>();
                        foreach (var item in ViewList)
                        {
                            string table1 = string.Empty;
                            string table2 = string.Empty;
                            table1 = string.Format("_{0} ", item.TableName1);
                            if (!tables.Contains(item.TableName1))
                            {
                                tables.Add(item.TableName1);
                                jointable.AppendLine(string.Format("{0}  as {1} ", item.TableName1, table1));
                            }
                            jointable.AppendLine(SqlCompilation.GetJoinEnum(item.Join));
                            if (!tables.Contains(item.TableName2))
                                table2 = string.Format("_{0} ", item.TableName2);
                            else
                            {
                                table2 = string.Format("_{0}{1} ", item.TableName2, tables.FindAll(p => p.Equals(item.TableName2)).Count);
                            }
                            jointable.AppendLine(string.Format("{0}  as {1} ", item.TableName2, table2));
                            tables.Add(item.TableName2);
                            for (int i = 0; i < item.Joinfields.Count; i++)
                            {
                                WhereField field = new WhereField();
                                field.FiledName = string.Format("{0}.{1}", table1,
                                                                             item.Joinfields[i].FiledName);
                                field.Value = string.Format("{0}.{1}", table2, item.Joinfields[i].Value);

                                if (i == 0)
                                {
                                    jointable.AppendLine("on");
                                    jointable.AppendLine(Sqlcom.CreateWhere(field, false, true));
                                    continue;
                                }
                                jointable.AppendLine(Sqlcom.CreateWhere(field));
                            }
                        }
                        return DbHelper.CreatePageString(jointable.ToString(), SqlClomns,
                                             where, _groupByValue.Trim(','),
                                             string.IsNullOrEmpty(order) ? string.Empty : order
                                        , pageindex, pagesize, out _selcountstr);

                    }
            }

            return string.Empty;
        }
        private string _selcountstr = string.Empty;
        public string CreateSql(OperateEnum operate)
        {
            return CreateSql(operate, 0, 0);
        }

        public int PageSize = AppCons.PageCount;

        /// <summary>
        /// 此方法不支持dataaction单独执行
        /// </summary>
        public virtual DataBaseAction Excute()
        {
            return this;
        }

        public void Dispose()
        {
            ClearPams();
            if (_dbHelper != null)
                _dbHelper.Dispose();
        }
        /// <summary>
        /// 清理所有的设置
        /// </summary>
        internal void ClearPams()
        {
            _whereStr = new StringBuilder();
            _keyValues = new List<ValueField>();
            _orderValues = new List<OrderField>();
            _groupByValue = string.Empty;

            Parameters.Clear();
        }
        #region 建议不要直接用以下方法,这些方法都抽离到各个action
        /// <summary>
        /// 执行结果
        /// </summary>
        public int ReturnCode = 0;

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <returns></returns>

        public virtual object QuerySingle()
        {
            return DbHelper.GetSingle(CreateSql(OperateEnum.Select));
        }
        /// <summary>
        /// 是否需要当前的查询进入缓存
        /// </summary>
        public bool IsNeedCache = true;
        private string _tradstr = string.Empty;
        /// <summary>
        /// 查询一个实体类
        /// </summary>
        /// <returns></returns> 
        public virtual T QueryEntity<T>(string sqlStr) where T : new()
        {
            if (CurrentOperate == OperateEnum.None)
                _tradstr = sqlStr;
            SqlPageParms(1);
            DbHelper.Parameters = Parameters;
            DataTable dt = DbHelper.ExeDataTable(sqlStr);
            if (dt != null)
            {
                Dictionary<string, PropertyInfo> pInfos = Sqlcom.MappingToProperty<T>(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr == null)
                        return default(T);
                    if (Cache.CacheHelper.IsOpenCache)
                    {
                        string cachekey = string.Format("entity_{2}_{0}_key_{1}",
                            TbName,
                            Cachekeyvalue, _appindex);
                        if (!IsNeedCache)
                        {
                            Cache.CacheHelper.Remove(cachekey);
                            T entity = Sqlcom.GetFromReader<T>(dr, pInfos);
                            return entity;
                        }
                        else
                        {
                            T entity = Cache.CacheHelper.Get<T>(cachekey);
                            if (entity == null)
                            {
                                entity = Sqlcom.GetFromReader<T>(dr, pInfos);
                                Cache.CacheHelper.Insert(cachekey, entity);
                            }
                            return entity;
                        }

                    }
                    return Sqlcom.GetFromReader<T>(dr, pInfos);
                }
            }
            return default(T);
        }
        /// <summary>
        /// 查询一个实体类
        /// </summary>
        /// <returns></returns> 
        public virtual T QueryEntity<T>() where T : new()
        {
            return QueryEntity<T>(CreateSql(OperateEnum.Select));
        }


        public virtual IDataReader QueryDataReader()
        {
            return DbHelper.ExecuteReader(CreateSql(OperateEnum.Select));
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>

        public virtual PageList<T> QueryPage<T>(int pageindex) where T : new()
        {
            return QueryPage<T>(pageindex, PageSize);
        }
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>

        public virtual PageList<T> QueryPage<T>(int pageindex, int pagesize) where T : new()
        {
            PageList<T> lists = new PageList<T>();
            int totalcount = 0;
            DbHelper.Parameters = Parameters;
            if (Cache.CacheHelper.IsOpenCache)
            {
                string cachekey = string.Format("entity_{4}_{0}_key_{1}_{2}_{3}",
                    TbName,
                    Cachekeyvalue,
                    pageindex,
                    pagesize, _appindex);
                PageList<T> entitys = Cache.CacheHelper.Get<PageList<T>>(cachekey);
                if (entitys == null || entitys.Count == 0)
                {
                    DataTable dt = DbHelper.CreatePage(CreateSql(OperateEnum.SelectPage, pagesize, pageindex), this._selcountstr, out totalcount);
                    if (dt != null)
                    {
                        Dictionary<string, PropertyInfo> pInfos = Sqlcom.MappingToProperty<T>(dt);
                        lists.AddRange(from DataRow dr in dt.Rows select Sqlcom.GetFromReader<T>(dr, pInfos));
                        lists.TotalCount = totalcount;
                        lists.TotalPage = totalcount / pagesize + (totalcount % pagesize > 0 ? 1 : 0);
                        Cache.CacheHelper.Insert(cachekey, lists);
                    }
                }
                else
                    return entitys;
            }
            else
            {
                DataTable dt = DbHelper.CreatePage(CreateSql(OperateEnum.SelectPage, pagesize, pageindex), this._selcountstr, out totalcount);
                if (dt != null)
                {
                    Dictionary<string, PropertyInfo> pInfos = Sqlcom.MappingToProperty<T>(dt);
                    lists.AddRange(from DataRow dr in dt.Rows select Sqlcom.GetFromReader<T>(dr, pInfos));
                    lists.TotalCount = totalcount;
                    lists.TotalPage = totalcount / pagesize + (totalcount % pagesize > 0 ? 1 : 0);
                }
            }
            return lists;
        }


        public virtual List<T> QueryList<T>(string sqlStr) where T : new()
        {
            _tradstr = sqlStr;
            List<T> lists = new List<T>();
            DbHelper.Parameters = Parameters;
            CreateWhereStr(sqlStr);
            if (Cache.CacheHelper.IsOpenCache)
            {
                string cachekey = string.Format("entity_{0}_key_{1}", TbName, Cachekeyvalue);
                List<T> entitys = Cache.CacheHelper.Get<List<T>>(cachekey);
                if (entitys == null || entitys.Count == 0)
                {
                    DataTable dt = DbHelper.ExeDataTable(sqlStr);
                    if (dt != null)
                    {
                        Dictionary<string, PropertyInfo> pInfos = Sqlcom.MappingToProperty<T>(dt);
                        lists.AddRange(from DataRow dr in dt.Rows select Sqlcom.GetFromReader<T>(dr, pInfos));
                        Cache.CacheHelper.Insert(cachekey, lists);
                    }
                }
                else
                    return entitys;
            }
            else
            {
                DataTable dt = DbHelper.ExeDataTable(sqlStr); if (dt != null)
                {
                    Dictionary<string, PropertyInfo> pInfos = Sqlcom.MappingToProperty<T>(dt);
                    lists.AddRange(from DataRow dr in dt.Rows select Sqlcom.GetFromReader<T>(dr, pInfos));
                }
            }
            return lists;
        }

        private string CreateWhereStr(string sqlStr)
        {
            if (sqlStr != null)
            {
                if (Cache.CacheHelper.IsOpenCache)
                    Cachekeyvalue = sqlStr.ToLower().Replace(" ", "").Replace("and", "").Replace("or", "").Trim();
                return sqlStr;
            }
            return string.Empty;
        }
        #endregion
    }
}

