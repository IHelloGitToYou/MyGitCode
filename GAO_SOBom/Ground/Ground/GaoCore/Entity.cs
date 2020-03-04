using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GaoCore
{
    [SqlSugar.SugarTable("Gao_Entity")]
    public class Entity
    {
        public Entity()
        {
            PersistStatus = PersistStatus.NEW;
            StructedViewProperty();
        }
        
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }
        public string CreateId { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateId { get; set; }
        public DateTime UpdateDate { get; set; }
        
        [IngnorePropery("持久化状态")]
        [SugarColumn(IsIgnore=true)]
        public PersistStatus PersistStatus { get; set; }

        [IngnorePropery("排除的列")]
        static List<string> _IgnoreProperys = null;
        public List<string> GetIgnoreProperys()
        {
            if (_IgnoreProperys == null)
            {
                lock (this)
                {
                    _IgnoreProperys = new List<string>();
                    var pList = this.GetType().GetProperties();
                    foreach (var item in pList)
                    {
                        var l = item.GetCustomAttributes(typeof(IngnorePropery), false).FirstOrDefault();
                        if (l != null)
                        {
                            _IgnoreProperys.Add(item.Name);
                        }
                    }
                }
            }
            return _IgnoreProperys;
        }

        private static object LockStructed = new object();
        public static List<string> IsStructedViewPropertyEntity { get; set; } = new List<string>();
        public static Dictionary<string, Dictionary<PropertyInfo, ViewAsPropertyAttribute>> DictViewPropertys { get; set; } = null;


 
        public static Dictionary<PropertyInfo, ViewAsPropertyAttribute> ViewPropertys(Type p_EntityType)
        {
            string typeName = p_EntityType.FullName;
            if (IsStructedViewPropertyEntity.Contains(typeName) == false)
                return null;

            return DictViewPropertys[typeName];
        }
        
        /// <summary>
        /// 生成视图属性
        /// </summary>
        public void StructedViewProperty() {
            if (DictViewPropertys == null)
                DictViewPropertys = new Dictionary<string, Dictionary<PropertyInfo, ViewAsPropertyAttribute>>();

            lock (LockStructed)
            {
                string typeName = this.GetType().FullName;
                if (IsStructedViewPropertyEntity.Contains(typeName) == false)
                {
                    var vps = new Dictionary<PropertyInfo, ViewAsPropertyAttribute>();
                    var list = this.GetType().GetProperties();// System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.GetProperties();
                    foreach (var item in list)
                    {
                        //视图属性
                        if (item.GetCustomAttribute(typeof(ViewAsPropertyAttribute)) != null)
                        {
                            vps.Add(item, (ViewAsPropertyAttribute)item.GetCustomAttribute(typeof(ViewAsPropertyAttribute)));
                        }

                        //下拉框的 _display 字段
                        if (item.GetCustomAttribute(typeof(IsComboboxAttribute)) != null)
                        {
                            var cpro = (IsComboboxAttribute)item.GetCustomAttribute(typeof(IsComboboxAttribute));
                            //[ViewAsProperty("CompId", "Gao_Comp", "b", "CompNo")]
                            string asName = item.Name + "_display";
                            var vpro = new ViewAsPropertyAttribute(item.Name, cpro.TableName, cpro.ShortName, cpro.DisplayField , asName, P_RightEqualFieldName: cpro.RightEqualFieldName);

                            //(ViewAsPropertyAttribute)item.GetCustomAttribute(typeof(ViewAsPropertyAttribute))
                            vps.Add(item, vpro);
                        }
                    }

                    IsStructedViewPropertyEntity.Add(typeName);
                    DictViewPropertys.Add(typeName, vps);
                }
            }
        }

        public static GaoSqlSugarClient GetDb(Type EntityType)
        {
            var dbName = RunningControllers.CacheAssemblyDBs[EntityType.Assembly];
            if (dbName == "X-LOGIN-ID")
            {
                throw new InValidException("Erp帐套数据,不应该调用这个方法[X-LOGIN-ID]");
            }

            return SqlSugarBase.GetDB(dbName);
        }

        public static GaoSqlSugarClient GetDb(string dbName)
        {
            if (dbName.IsNullOrEmpty())
                throw new InvalidCastException("数据库参数不能为空");

            return SqlSugarBase.GetDB(dbName);
        }

    }
}
