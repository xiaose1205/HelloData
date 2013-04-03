using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Reflection;
using HelloData.FrameWork.Logging;
using HelloData.FrameWork.Data.Enum;

namespace HelloData.FrameWork.Data
{
    /// <summary>
    /// 用来编译动态生成的sql语句
    /// </summary>
    public class SqlCompilation
    {
        public SqlCompilation()
        { }
        public SqlCompilation(DataBase currentData)
        {
            CurrentData = currentData;
        }
        private const string BetweenStr = " {0}       {1}  Between {2} and {3}";
        private const string InOrNotStr = " {0}       {1}   {2} ( {3}) ";
        private const string OtherRelationStr = " {0}      {1} {2} {3}  ";
        private const string IsnullorNotnullstr = "{0}   {1}";
        internal const string InsertStr = "insert into {0} ({1})values({2});";
        internal const string UpdateStr = "update {0} set {1} where 1=1   {2};";
        internal const string DeleteStr = "delete from {0} where 1=1  {1};";
        internal const string SelectStr = "select {0} from {1} where  1=1 {2} {4} {3};";

        public DataBase CurrentData { get; set; }

        public string GetDbTypeString(object oldValue, DbType dbtype)
        {
            if (AppCons.IsParmes)
            {
                if (oldValue == null)
                    return string.Empty;
                return oldValue.ToString();
            }
            if (oldValue == null)
                return string.Format("'{0}'", "");
            switch (dbtype)
            {
                case DbType.Boolean:
                    return oldValue.ToString();
                case DbType.Int16:
                    return oldValue.ToString();

                case DbType.Int32:
                    return oldValue.ToString();
                case DbType.Int64:
                    return oldValue.ToString();
                default:
                    return string.Format("'{0}'", oldValue);
            }
        }
        /// <summary>
        /// 生成where 语句
        /// </summary>
        /// <param name="keyValus"></param>
        /// <returns></returns>
        internal string CreateWhere(QueryField keyValus)
        {
            return CreateWhere(keyValus, true);
        }
        internal string CreateWhere(QueryField keyValus, bool withRelation)
        {
            return CreateWhere(keyValus, withRelation, false);
        }
        /// <summary>
        /// 生成where 语句
        /// </summary>
        /// <param name="keyValus"></param>
        /// <param name="withRelation"> </param>
        /// <returns></returns>
        internal string CreateWhere(QueryField keyValus, bool withRelation, bool isTablecolom)
        {
            if (null == keyValus)
                return string.Empty;
            StringBuilder sb = new StringBuilder();
            string sqlwhere;
            string contionstr = withRelation ? keyValus.Condition.ToString() : "";
            if (keyValus.Relation == RelationEnum.Between)
            {
                sqlwhere = (string.Format(BetweenStr,
                   contionstr,
                    keyValus.FiledName,
                   keyValus.Value,
                  keyValus.Value2));
            }
            else if (keyValus.Relation == RelationEnum.In || keyValus.Relation == RelationEnum.NotIn)
            {
                sqlwhere = (string.Format(InOrNotStr,
                    contionstr,
                  keyValus.FiledName,
                 GetRelationStr(keyValus.Relation),
                 keyValus.Value));

            }
            else if (keyValus.Relation == RelationEnum.LikeLeft)
            {
                sqlwhere = (string.Format(OtherRelationStr,
                   contionstr,
                  keyValus.FiledName,
                 GetRelationStr(keyValus.Relation),
                string.Format("{0}", keyValus.Value)));
            }
            else if (keyValus.Relation == RelationEnum.LikeRight)
            {
                sqlwhere = (string.Format(OtherRelationStr,
                   contionstr,
                  keyValus.FiledName,
                 GetRelationStr(keyValus.Relation),
                string.Format("{0}", keyValus.Value)));
            }
            else if (keyValus.Relation == RelationEnum.Like)
            {
                sqlwhere = (string.Format(OtherRelationStr,
                  contionstr,
                  keyValus.FiledName,
                 GetRelationStr(keyValus.Relation),
                string.Format("{0}", keyValus.Value)));
            }
            else if (keyValus.Relation == RelationEnum.IsNotNull || keyValus.Relation == RelationEnum.IsNull)
            {
                sqlwhere = string.Format(IsnullorNotnullstr,
                        contionstr,
                        keyValus.FiledName);
            }
            else
                if (isTablecolom)
                {
                    sqlwhere = (string.Format(OtherRelationStr,
                                          contionstr,
                                          keyValus.FiledName,
                                          GetRelationStr(keyValus.Relation),
                                         keyValus.Value));
                }
                else
                {
                    sqlwhere = (string.Format(OtherRelationStr,
                                              contionstr,
                                              keyValus.FiledName,
                                              GetRelationStr(keyValus.Relation),
                                              GetDbTypeString(keyValus.Value, ConvertToDbType(keyValus.Value))));
                }

            sb.AppendLine(sqlwhere);
            return sb.ToString();
        }

        /// <summary>
        /// 创建order语句
        /// </summary>
        /// <param name="keyvalues"></param>
        /// <returns></returns>
        internal string CreateOrder(List<OrderField> keyvalues)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in keyvalues)
            {
                sb.AppendFormat(" {0} {1} ,", item.FiledName, item.Order.ToString());
            }
            return sb.ToString().TrimEnd(',');
        }

        internal T GetFromReader<T>(IDataReader sdr, Dictionary<string, PropertyInfo> pInfos) where T : new()
        {
            T data = new T();
            foreach (KeyValuePair<string, PropertyInfo> pInfo in pInfos)
            {
                string columnName = pInfo.Key;
                PropertyInfo propertyInfo = pInfo.Value;
                object valueobj = ConverToValue(sdr[columnName], propertyInfo.PropertyType);
                if (valueobj != null)
                {
                    try
                    {
                        propertyInfo.SetValue(data, valueobj, null);
                    }
                    catch (Exception ex)
                    {
                        Logger.CurrentLog.Error("反射转换数据出错", ex);
                    }
                }
            }
            return data;
        }

        internal T GetFromReader<T>(DataRow sdr, Dictionary<string, PropertyInfo> pInfos) where T : new()
        {

            T data = new T();
            foreach (KeyValuePair<string, PropertyInfo> pInfo in pInfos)
            {
                string columnName = pInfo.Key;
                PropertyInfo propertyInfo = pInfo.Value;
                if (sdr == null || sdr[columnName] == null)
                    continue;
                object valueobj = ConverToValue(sdr[columnName], propertyInfo.PropertyType);
                if (valueobj != null)
                {
                    try
                    {

                        propertyInfo.SetValue(data, valueobj, null);
                    }
                    catch (Exception ex)
                    {
                        Logger.CurrentLog.Error("反射转换数据出错", ex);
                    }
                }
            }
            return data;
        }

        public object ConverToValue(object value, Type type)
        {
            if (value != null && value != DBNull.Value)
            {
                Type valueType = value.GetType();
                if (!valueType.Equals(type))
                {
                    if (valueType.Equals(typeof(Int64)) && type.Equals(typeof(Int32)))
                    {
                        Int64 intvalue = (Int64)value;
                        if (intvalue < int.MaxValue && intvalue > int.MinValue)
                        {
                            return int.Parse(value.ToString());
                        }
                    }
                    else if (valueType.Equals(typeof(string)) && (type.Equals(typeof(Int32)) || type.Equals(typeof(int?))))
                    {
                        int intvalue = 0;
                        if (int.TryParse(value.ToString(), out intvalue))
                            return intvalue;
                        return intvalue;
                    }
                    else if (valueType.Equals(typeof(string)) && (type.Equals(typeof(bool)) || type.Equals(typeof(bool?))))
                    {
                        bool boolvalue;
                        if (bool.TryParse(value.ToString(), out boolvalue))
                            return boolvalue;
                        return boolvalue;
                    }
                    else if (valueType.Equals(typeof(string)) && type.Equals(typeof(Guid)))
                    {
                        string gudistr = value.ToString();
                        value = new Guid(gudistr);
                    }
                    else if (valueType.Equals(typeof(int)) && type.Equals(typeof(bool)))
                    {
                        value = (int)value > 0;
                    }
                    else if (valueType.Equals(typeof(string)) && type.Equals(typeof(string)))
                    {
                        value = value.ToString().Trim().Replace("\0", "");
                    }
                    else if (valueType.Equals(typeof(decimal)) && type.Equals(typeof(decimal)))
                    {
                        value = Convert.ChangeType(value, type);
                    }
                    else if (valueType.FullName.Contains("DateTime") &&
                             (type.Equals(typeof (DateTime)) || type.Equals(typeof (DateTime?))))
                    {
                        DateTime dateTime = DateTime.Now;
                        DateTime.TryParse(value.ToString(),out dateTime);
                        value = dateTime;
                    }
                    else if (type.Equals(typeof (string)))
                    {
                        value = value.ToString().Trim().Replace("\0", "");
                    }
                }
            }
            else
            {
                if (typeof(string) == type)//有字串在服务端不赋值时传到客户端后，客户使用前不做为空判断，会出现异常：所以才改
                {
                    return string.Empty;
                }
                return null;
            }

            return value;
        }

        /// <summary>
        /// 通过反射发出设置属性值
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="propertyName"> </param>
        /// <param name="newValue">新的属性值</param>
        /// <returns></returns>
        public void SetPropertyValue(object obj, string propertyName, object newValue)
        {
            Type type = obj.GetType();
            PropertyInfo property = type.GetProperty(propertyName);
            property.SetValue(obj, newValue, null);
            //  SetPropertyValue(obj, property, newValue);
        }

        /// <summary>
        /// 数据结构在实体中那些属性能赋值
        /// </summary>
        /// <param name="sdr"></param>
        /// <returns></returns>
        public Dictionary<string, PropertyInfo> MappingToProperty<T>(IDataReader sdr)
        {

            Type tType = typeof(T);
            PropertyInfo[] pInfos = tType.GetProperties();
            Dictionary<string, PropertyInfo> dic = new Dictionary<string, PropertyInfo>();
            try
            {
                foreach (PropertyInfo pInfo in pInfos)
                {
                    if (sdr.GetSchemaTable().Columns.Contains(pInfo.Name) && pInfo.CanWrite && false == dic.ContainsKey(pInfo.Name))
                        dic.Add(pInfo.Name, pInfo);
                }
            }
            catch (Exception ex)
            {
                Logger.CurrentLog.Error(ex.Message, ex);
            }
            return dic;
        }

        /// <summary>
        /// 数据结构在实体中那些属性能赋值
        /// </summary>
        /// <param name="sdr"></param>
        /// <returns></returns>
        public Dictionary<string, PropertyInfo> MappingToProperty<T>(DataTable sdr)
        {

            Type tType = typeof(T);
            PropertyInfo[] pInfos = tType.GetProperties();
            Dictionary<string, PropertyInfo> dic = new Dictionary<string, PropertyInfo>();
            foreach (PropertyInfo pInfo in pInfos)
            {
                try
                {

                    if (sdr.Columns.Contains(pInfo.Name) && pInfo.CanWrite && false == dic.ContainsKey(pInfo.Name))
                        dic.Add(pInfo.Name, pInfo);

                }
                catch (Exception ex)
                {
                    Logger.CurrentLog.Error(ex.Message, ex);
                }
            }
            return dic;
        }

        /// <summary>
        /// 获取已经赋值的实体类的属性值
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public List<ValueField> GetKeyValues(BaseEntity entity)
        {
            var tType = entity.GetType();
            var pInfos = tType.GetProperties();
            if (pInfos.Length == 0)
                return new List<ValueField>();
            var valuefields = new List<ValueField>();
            foreach (var property in pInfos)
            {
                var value = property.GetValue(entity, null);
                object[] attributes = property.GetCustomAttributes(typeof(ColumnAttribute), true);
                if (value == null || property.Name == entity.KeyId ||
                    (attributes.Length > 0 && ((ColumnAttribute)attributes[0]).NoSqlProperty))
                    continue;
                var field = new ValueField { FiledName = property.Name, Value = value, DataType = ConvertToDbType(value) };
                valuefields.Add(field);
            }
            return valuefields;
        }


        /// <summary>
        /// 把值转换成对应的类型值
        /// </summary>
        /// <returns></returns>
        public DbType ConvertToDbType(object invalue)
        {
            if (invalue == null)
                return DbType.String;
            DbType objvalue;
            DateTime dateTime;
            Type valueType = invalue.GetType();
            if (valueType.IsEnum)
            {
                objvalue = DbType.Int32;
            }
            else if (valueType.Name == typeof(int).Name ||
                 valueType.Name == typeof(Int16).Name ||
                 valueType.Name == typeof(Int32).Name ||
                 valueType.Name == typeof(Int64).Name ||
                 valueType.Name == typeof(UInt16).Name ||
                 valueType.Name == typeof(UInt32).Name ||
                 valueType.Name == typeof(UInt64).Name ||
                 valueType.Name == typeof(Decimal).Name ||
                 valueType.Name == typeof(Double).Name ||
                 valueType.Name == typeof(Byte).Name)
            {
                objvalue = DbType.Int32;
            }
            else if (valueType == typeof(bool))
            {
                objvalue = DbType.Boolean;
            }
            else if (valueType.Name == "String")
            {
                objvalue = DbType.String;
            }
            else if (DateTime.TryParse(invalue.ToString(), out dateTime))
            {
                objvalue = DbType.DateTime;
            }
            else
            {
                objvalue = DbType.String;
            }
            return objvalue;
        }


        public static string GetJoinEnum(ViewJoinEnum eEnum)
        {
            switch (eEnum)
            {
                case ViewJoinEnum.leftjoin:
                    return " left join ";
                case ViewJoinEnum.crossjoin:
                    return " cross join ";
                case ViewJoinEnum.fulljoin:
                    return " full join ";
                case ViewJoinEnum.rightjoin:
                    return " right join ";
                case ViewJoinEnum.innerjoin:
                    return " inner join ";
            }
            return string.Empty;
        }

        public static string GetRelationStr(RelationEnum relation)
        {
            string relationstr = "=";
            switch (relation)
            {
                case RelationEnum.Equal:
                    relationstr = "=";
                    break;
                case RelationEnum.Large:
                    relationstr = ">";
                    break;
                case RelationEnum.LargeThen:
                    relationstr = ">=";
                    break;
                case RelationEnum.Less:
                    relationstr = "<";
                    break;
                case RelationEnum.LessThen:
                    relationstr = "<=";
                    break;
                case RelationEnum.Like:
                case RelationEnum.LikeLeft:
                case RelationEnum.LikeRight:
                    relationstr = "like";
                    break;
                case RelationEnum.NoLike:
                    relationstr = "not like";
                    break;
                case RelationEnum.NoEqual:
                    relationstr = "<>";
                    break;
                case RelationEnum.In:
                    relationstr = "in";
                    break;
                case RelationEnum.NotIn:
                    relationstr = "not in";
                    break;
                case RelationEnum.IsNull:
                    relationstr = "IS NULL";
                    break;
                case RelationEnum.IsNotNull:
                    relationstr = "IS NOT NULL";
                    break;
            }
            return relationstr;

        }

    }
}
