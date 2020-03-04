using GaoCore.ViewConfigs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace GaoCore.Extjs
{
    public class ExtjsBuilder
    {
        public static void Run()
        {
            var jObject = new JsonConfigHelper("GaoSettings.json").jObject;
            var path = jObject.SelectToken("ExtjsGeneratePath")?.ToString();

            ///生成实体Model文件 ErpModels.js
            StringBuilder sb = new StringBuilder();
            foreach (Assembly assembly in RunningControllers.GetAll())
            {
                foreach (var item in assembly.DefinedTypes)
                {
                    var entityClass = item.CustomAttributes.Where(o => o.AttributeType == typeof(RootEntity)).ToList();
                    if (entityClass.Count <= 0)
                        continue;

                    var bb = ((Entity)assembly.CreateInstance(item.FullName));//.StructedViewProperty();
                                                                              // bb.StructedViewProperty();

                    var propertys = GenerateExtJsModel(item);
                    sb.AppendLine("Ext.define('" + item.FullName + "', { extend: 'ExtGAO.model.Base',fields: [" + string.Join(",", propertys) + "]});");
                }
            }

            if (path.IsNotEmpty())
            {
                FileStream s = new FileStream(path + "ErpModels.js", FileMode.OpenOrCreate);
                //清空之前的内容 
                s.SetLength(0);
                byte[] byteData = Encoding.UTF8.GetBytes(sb.ToString());
                s.Write(byteData, 0, byteData.Count());
                s.Flush();
                s.Close();
            }

            List<Type> enumProperys = new List<Type>();
            ///生成枚举Data      ErpDatas.js
            //找出多少个枚举Type
            foreach (Assembly assembly in RunningControllers.GetAll())
            {
                foreach (var item in assembly.DefinedTypes)
                {
                    //继续！
                    var list3 = FindEnumProperyType(item);
                    foreach (var item2 in list3)
                    {
                        if (enumProperys.Contains(item2) == false)
                        {
                            enumProperys.Add(item2);
                        }
                    }
                }
            }

            sb.Clear();
            foreach (var item in enumProperys)
            {
                var listFields = item.GetFields(BindingFlags.Public| BindingFlags.Static);
                List<ExtjsEnumDataModel> enumDatas = new List<ExtjsEnumDataModel>();
                foreach (var item2 in listFields)
                {
                    var enumData = new ExtjsEnumDataModel()
                    {
                        Id = (int)item2.GetRawConstantValue(),
                        NAME = item2.Name
                    };

                    if (item2.GetCustomAttribute(typeof(LabelAttribute)) != null)
                    {
                        enumData.NAME = (item2.GetCustomAttribute(typeof(LabelAttribute)) as LabelAttribute).Label;
                    }

                    enumDatas.Add(enumData);
                }

                sb.AppendLine("Gao.Datas." + item.Name + " = [" + string.Join(",", enumDatas) + "];");
            }
            sb.AppendLine("");
            sb.AppendLine(@"Ext.define('ExtGAO.model.ErpDatas', {singleton: true,Init: function () {}});");
            if (path.IsNotEmpty())
            {
                FileStream s = new FileStream(path + "ErpDatas.js", FileMode.OpenOrCreate);
                //清空之前的内容 
                s.SetLength(0);
                byte[] byteData = Encoding.UTF8.GetBytes(sb.ToString());
                s.Write(byteData, 0, byteData.Count());
                s.Flush();
                s.Close();
            }

        }

        #region 生成实体Model

        public static List<ExtjsDataModelField> GenerateExtJsModel(Type EntityOf)
        {
            List<ExtjsDataModelField> fields = new List<ExtjsDataModelField>
            {
                //必有字段,用于在前台标识记录的CURD状态
                new ExtjsDataModelField("PersistStatus", "int")
            };

            var list = EntityOf.GetProperties();
            var defaultObject = EntityOf.Assembly.CreateInstance(EntityOf.FullName);
            foreach (var item in list)
            {
                //字段被排除了
                if (item.GetCustomAttribute(typeof(IngnorePropery)) != null)
                    continue;
                ///下拉框 的显示字段
                if (item.GetCustomAttribute(typeof(IsComboboxAttribute)) != null)
                {
                    fields.Add(new ExtjsDataModelField(item.Name + "_display", "string"));
                }
                

                var fieldType = item.PropertyType;
                ////视图属性
                //if (item.GetCustomAttribute(typeof(ViewAsPropertyAttribute)) != null)
                //{
                //    (EntityOf as Entity).ViewPropertys = new Dictionary<PropertyInfo, ViewAsPropertyAttribute>();

                //}
                var defaultValue = item.GetValue(defaultObject);

                if (fieldType == typeof(String))
                {
                    fields.Add(new ExtjsDataModelField(item.Name, "string" , defaultValue));
                }
                else if (fieldType == typeof(int) || fieldType == typeof(int?))
                {
                    fields.Add(new ExtjsDataModelField(item.Name, "int", defaultValue));
                }
                else if (fieldType == typeof(double) || fieldType == typeof(decimal)
                        || fieldType == typeof(double?) || fieldType == typeof(decimal?))
                {
                    fields.Add(new ExtjsDataModelField(item.Name, "number", defaultValue));
                }
                else if (fieldType == typeof(DateTime) || fieldType == typeof(DateTime?))
                {
                    fields.Add(new ExtjsDataModelField(item.Name, "date", defaultValue));
                }
                else if (fieldType == typeof(Boolean) || fieldType == typeof(Boolean?))
                {
                    fields.Add(new ExtjsDataModelField(item.Name, "boolean", defaultValue));
                }
                else if (fieldType == typeof(Enum) || fieldType.BaseType == typeof(Enum))
                {
                    fields.Add(new ExtjsDataModelField(item.Name, "int", defaultValue));
                }
                else
                    throw new StructureException("生成实体[{0}] 出错".FormatOrg(EntityOf.Name));
            }

            return fields;
        }

        #endregion

        #region  生成枚举Data
        /// <summary>
        /// 查找实体中使用了多少个枚举
        /// </summary>
        /// <param name="EntityOf"></param>
        /// <returns></returns>
        public static List<Type> FindEnumProperyType(Type EntityOf)
        {
            List<Type> enumProperys = new List<Type>();
            var list = EntityOf.GetProperties();
            foreach (var item in list)
            {
                //字段被排除了
                if (item.GetCustomAttribute(typeof(IngnorePropery)) != null)
                    continue;

                var fieldType = item.PropertyType;
                if (fieldType == typeof(Enum) || fieldType.BaseType == typeof(Enum))
                {
                    enumProperys.Add(fieldType);
                }
            }

            return enumProperys;
        }

        #endregion

        #region 生成实体视图
        private static readonly object Lock1 = new object();
        public static Dictionary<string, EntityViewConfig> CacheViewConfigs = new Dictionary<string, EntityViewConfig>();
        public static EntityViewConfig GenerateExtjsView(string EntityFullName, string ViewGroup)
        {
            EntityViewConfig viewConfig = null;
            //string fullName = entity.GetType().FullName;

            ///Key带出ViewGroup 目的让ViewConfig文件可以定义多个
            string cacheKey = EntityFullName + ViewGroup;
            lock (Lock1)
            {
                if (CacheViewConfigs.ContainsKey(cacheKey))
                {
                    viewConfig = CacheViewConfigs[cacheKey];
                    return viewConfig;
                }
            }

            foreach (Assembly assembly in RunningControllers.GetAll())
            {
                foreach (var item in assembly.DefinedTypes)
                {
                    bool isViewCofing = item.IsSubclassOf(typeof(EntityViewConfig));
                    if (isViewCofing == false)
                        continue;

                    viewConfig = (EntityViewConfig)assembly.CreateInstance(item.FullName);
                    var type = viewConfig.EntityType;

                    if (type.FullName == EntityFullName)
                    {
                        lock (Lock1)
                        {
                            viewConfig.ConfigView(ViewGroup);
                            //测试中
                            //CacheViewConfigs.Add(cacheKey, viewConfig);
                            return viewConfig;
                        }
                    }
                }
            }

            return null;
        }

        #endregion


        private static readonly object Lock2 = new object();
        static Dictionary<string, ExtjsViewCommand> CacheExtjsViewCommands = new Dictionary<string, ExtjsViewCommand>();

        public static ExtjsViewCommand FindViewCommand(string CommandPath)
        {
            if(CommandPath == QueryViewSearchCommand.CommonCommandPath)
            {
                ExtjsViewCommand command = (QueryViewSearchCommand)typeof(QueryViewSearchCommand).Assembly.CreateInstance(typeof(QueryViewSearchCommand).FullName);
                return command;
            }

            string cacheKey = CommandPath;
            lock (Lock2)
            {
                if (CacheExtjsViewCommands.ContainsKey(cacheKey))
                {
                    return CacheExtjsViewCommands[cacheKey];
                }

                ExtjsViewCommand command = null;
                foreach (Assembly assembly in RunningControllers.GetAll())
                {
                    foreach (var item in assembly.DefinedTypes)
                    {
                        bool isExtCommand = item.IsSubclassOf(typeof(ExtjsViewCommand));
                        if (isExtCommand == false)
                            continue;

                        command = (ExtjsViewCommand)assembly.CreateInstance(item.FullName);

                        if(CacheExtjsViewCommands.ContainsKey(command.CommandPath) == false)
                            CacheExtjsViewCommands.Add(command.CommandPath, command);
                        
                        if (command.CommandPath == CommandPath)
                        {
                            return command;
                        }
                    }
                }
            }

            return null;
        }


        private static readonly object Lock3 = new object();
        static Dictionary<string, QueryEntity> CacheQueryEntitys = new Dictionary<string, QueryEntity>();
        
        public static QueryEntity FindQueryEntity(string QueryEntityFullName)
        {
            string cacheKey = QueryEntityFullName;
            lock (Lock3)
            {
                if (CacheQueryEntitys.ContainsKey(cacheKey))
                {
                    return CacheQueryEntitys[cacheKey];
                }

                QueryEntity qe = null;
                foreach (Assembly assembly in RunningControllers.GetAll())
                {
                    foreach (var item in assembly.DefinedTypes)
                    {
                        bool isQECommand = item.IsSubclassOf(typeof(QueryEntity));
                        if (isQECommand == false)
                            continue;

                        qe = (QueryEntity)assembly.CreateInstance(item.FullName);

                        if (CacheQueryEntitys.ContainsKey(item.FullName) == false)
                            CacheQueryEntitys.Add(item.FullName, qe);

                        if (item.FullName == QueryEntityFullName)
                        {
                            return qe;
                        }
                    }
                }
            }

            return null;
        }
    }
}
