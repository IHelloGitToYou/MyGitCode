using GaoCore.ViewConfigs;
using GaoCore.ViewConfigs.JSON;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace GaoCore
{
    public class EntityViewConfig {
        public static string ViewGroupDetail = "DETAIL";
        public static string ViewGroupList = "LIST";
        public string Container { get; set; } = "LIST";
        /// <summary>
        /// Extjs控制器简称
        /// </summary>
        public string Controller { get; set; } = "";//base_controller

        /// <summary>
        /// 区域标题栏
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Tab菜单名
        /// </summary>
        public string TabTitle { get; set; }
        

        [JsonConverter(typeof(EntityTypeFullNameJsonConverter))]
        public Type EntityType { get; set; }

        public string ViewGroup { get; set; }

        public List<ExtjsViewCommand> Commands;

        public virtual void ConfigView(string P_ViewGroup)
        {
            ViewGroup = P_ViewGroup;
            if (ViewGroup == ViewGroupDetail)
            {
                OnDetailView();
                Container = "DETAIL";
            }
            else if (ViewGroup == ViewGroupList)
            {
                OnListView();
                Container = "LIST";
            }
            else
            {
                OnOtherView(P_ViewGroup);
            }
        }

        public Dictionary<string, object> DetailDefaultConfig { get; set; }
  
        /// <summary>
        /// Form中Defaults:{}
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="Value"></param>
        public void SetDetailDefaultConfig(string Key, object Value)
        {
            if (DetailDefaultConfig == null)
                DetailDefaultConfig = new Dictionary<string, object>();

            if (DetailDefaultConfig.ContainsKey(Key) == false)
                DetailDefaultConfig.Add(Key, Value);

            DetailDefaultConfig[Key] = Value;
        }

        public virtual void OnDetailView() { }


        public int? PageSize { get; set; } = 25;
        public virtual void OnListView() { }

        public virtual void OnOtherView(string P_ViewGroup)
        {

        }

        public virtual void SetPageSize(int PageSizeNum)
        {
            PageSize = PageSizeNum;
        }

        public virtual void SetContainer( bool isFrom = false)
        {
            this.Container = isFrom ? "DETAIL" : "LIST";
        }
        public LayoutConfig ContainerLayoutConfig { get; set; }


        public virtual void SetController(string ShortControllerName)
        {
            this.Controller = ShortControllerName;
        }


    }


    public class EntityViewConfig<T>: EntityViewConfig where T : Entity 
    {
        //复合UI布局
        public List<EntityViewConfig> Items;
        //普通布局下的 子元素 ,在Items有值情况下忽略此
        public List<ViewPropery> ShowFields { get; set; }
        /// <summary>
        /// 查询面板视图 
        /// </summary>
        public EntityViewConfig QueryView { get; set; }

        /// <summary>
        /// Extjs布局方式 
        /// </summary>
        public Layout Layout { get; set; }
        
        //name, length, label, 
        public EntityViewConfig()
        {
            EntityType =typeof(T);
            ShowFields = new List<ViewPropery>();
            //Items = new List<IContainer>();
        }

        public void SetTabTitle(string TabTitle)
        {
            this.TabTitle = TabTitle;
        }

        public void SetTitle(string Title)
        {
            this.Title = Title;
        }

        //public ViewPropery Propery(Expression<Func<T, EunmTax>> expression)
        //{
        //    MemberExpression memberExpression = expression.Body as MemberExpression;
        //    if (memberExpression == null)
        //        throw new StructureException("生成视图失败: 表达式异常");

        //    ViewPropery p1 = _Propery(memberExpression, expression.ReturnType);
        //    return p1;
        //}

        public ViewPropery Propery(Expression<Func<T, Enum>> expression)
        {
            if(expression.Body is System.Linq.Expressions.UnaryExpression)
            {
                var operand = (expression.Body as System.Linq.Expressions.UnaryExpression).Operand;
                MemberExpression memberExpression2 = operand as MemberExpression;
                if (memberExpression2 == null)
                    throw new StructureException("生成视图失败: 表达式异常");
                ViewPropery p2 = _Propery(memberExpression2, operand.Type);
                return p2;
            }

            MemberExpression memberExpression = expression.Body as MemberExpression;
            if (memberExpression == null)
                throw new StructureException("生成视图失败: 表达式异常");

            ViewPropery p1 = _Propery(memberExpression, expression.ReturnType);
            return p1;
        }

        public ViewPropery Propery(Expression<Func<T, decimal>> expression)
        {
            MemberExpression memberExpression = expression.Body as MemberExpression;
            if (memberExpression == null)
                throw new StructureException("生成视图失败: 表达式异常");

            ViewPropery p1 = _Propery(memberExpression, expression.ReturnType);
            return p1;
        }

        public ViewPropery Propery(Expression<Func<T, double>> expression)
        {
            MemberExpression memberExpression = expression.Body as MemberExpression;
            if (memberExpression == null)
                throw new StructureException("生成视图失败: 表达式异常");

            ViewPropery p1 = _Propery(memberExpression, expression.ReturnType);
            return p1;
        }

        public ViewPropery Propery(Expression<Func<T, int>> expression)
        {
            MemberExpression memberExpression = expression.Body as MemberExpression;
            if (memberExpression == null)
                throw new StructureException("生成视图失败: 表达式异常");

            ViewPropery p1 = _Propery(memberExpression, expression.ReturnType);
            return p1;
        }

        public ViewPropery Propery(Expression<Func<T, bool>> expression)
        {
            MemberExpression memberExpression = expression.Body as MemberExpression;
            if (memberExpression == null)
                throw new StructureException("生成视图失败: 表达式异常");

            ViewPropery p1 = _Propery(memberExpression, expression.ReturnType);
            return p1;
        }

        public ViewPropery Propery(Expression<Func<T, DateTime>> expression)
        {
            MemberExpression memberExpression = expression.Body as MemberExpression;
            if (memberExpression == null)
                throw new StructureException("生成视图失败: 表达式异常");

            ViewPropery p1 = _Propery(memberExpression, expression.ReturnType);
            return p1;
        }

        public ViewPropery Propery(Expression<Func<T, DateTime?>> expression)
        {
            MemberExpression memberExpression = expression.Body as MemberExpression;
            if (memberExpression == null)
                throw new StructureException("生成视图失败: 表达式异常");

            ViewPropery p1 = _Propery(memberExpression, expression.ReturnType);
            return p1;
        }


        public ViewPropery Propery(Expression<Func<T, string>> expression)
        {
            MemberExpression memberExpression = expression.Body as MemberExpression;
            if (memberExpression == null)
                throw new StructureException("生成视图失败: 表达式异常");

            ViewPropery p1 = _Propery(memberExpression, expression.ReturnType);
            return p1;
        }


        private ViewPropery _Propery(MemberExpression memberExpression, Type ReturnType)
        {
            string propertyName = memberExpression.Member.Name;
            ViewPropery p1 = ShowFields.FirstOrDefault(o => o.Name == propertyName);
            if (p1 == null)
            {
                p1 = new ViewPropery();
            }

            p1.Name = propertyName;
            p1.Label = propertyName;

            var mpAttributes = (memberExpression.Member.GetCustomAttributes(typeof(LabelAttribute), false));
            if (mpAttributes.Count() >= 1)
            {
                p1.Label = (mpAttributes[0] as LabelAttribute).Label;
            }

            if (ReturnType == typeof(Enum)||( ReturnType != null && ReturnType.BaseType == typeof(Enum))) 
                p1.DataType = typeof(int);
            else
                p1.DataType = ReturnType;

            //设置默认编辑器
            SetDefaultEditor(p1, ReturnType);

            ///设置 前台校验条码, 
            var reqAttributes = (memberExpression.Member.GetCustomAttributes(typeof(RequireAttribute), false));
            if (reqAttributes.Count() >= 1)
                p1.SetEditorConfig("allowBlank",false);

            var minValAttribute = (memberExpression.Member.GetCustomAttributes(typeof(MinValueAttribute), false)).FirstOrDefault();
            if (minValAttribute != null)
            {
                object minValue = (minValAttribute as MinValueAttribute).MinValue;
                if (minValue != null)
                {
                    if (CommonExtend.IsNumberType(p1.DataType))
                        p1.SetEditorConfig("minValue", minValue.ToString());
                    else if (CommonExtend.IsDateType(p1.DataType))
                        p1.SetEditorConfig("minValue", minValue.GetDateOrNull()?.ToString("yyyy/MM/dd"));
                }
            }

            var maxValAttribute = (memberExpression.Member.GetCustomAttributes(typeof(MaxValueAttribute), false)).FirstOrDefault();
            if (maxValAttribute != null)
            {
                object maxValue = (maxValAttribute as MaxValueAttribute).MaxValue;
                if (maxValue != null)
                {
                    if (CommonExtend.IsNumberType(p1.DataType))
                        p1.SetEditorConfig("maxValue", maxValue.ToString());
                    else if (CommonExtend.IsDateType(p1.DataType))
                        p1.SetEditorConfig("maxValue", maxValue.GetDateOrNull()?.ToString("yyyy/MM/dd"));
                }
            }

            //视图属性 readOnly 只读
            var viewPreportyAttribute = (memberExpression.Member.GetCustomAttributes(typeof(ViewAsPropertyAttribute), false)).FirstOrDefault();
            if (viewPreportyAttribute != null)
            {
                p1.SetEditable(false);
                //p1.SetEditorConfig("editable", false);
            }

            //如果是下拉框则 ,设置显示字段
            var isComboboxAttribute = (memberExpression.Member.GetCustomAttributes(typeof(IsComboboxAttribute), false)).FirstOrDefault();
            if(isComboboxAttribute != null)
            {
                var combobox = (IsComboboxAttribute)isComboboxAttribute;
                p1.SetEditorConfig("displayField", combobox.DisplayField);
                p1.SetEditorConfig("RendererDisplayField", combobox.DisplayField);

            }

            ShowFields.Add(p1);
            return p1;
        }

        public EntityViewConfig AddItem(EntityViewConfig container)
        {
            if(Items == null)
                Items = new List<EntityViewConfig>();

            Items.Add(container);
            return container;
        }

        public EntityViewConfig<T> UseLayout(Layout Layout)
        {
            this.Layout = Layout;
            return this;
        }


        public EntityViewConfig UseCommands(params Type[] P_Commands)
        {
            if (Commands == null)
                Commands = new List<ExtjsViewCommand>();

            foreach (var item in P_Commands)
            {
                if (item.IsSubclassOf(typeof(ExtjsViewCommand)) == false)
                    continue;

                var itemObj = item.Assembly.CreateInstance(item.FullName) as ExtjsViewCommand;
                if (Commands.Where(o=>o.CommandPath ==itemObj.CommandPath).FirstOrDefault() == null)
                    Commands.Add(itemObj);
            }

            return this;
        }

        //public string GetJson(string TargerType)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    foreach (var item in ShowFields)
        //    {
        //        sb.Append(
        //            item.ToString()
        //        );
        //    }

        //    return sb.ToString();
        //}


        public void SetDefaultEditor(ViewPropery p1, Type DataType)
        {
            p1.Editor = "textfield";
            ///默认的Editor
            //var dataType = memberExpression.Member.DeclaringType;
            if (DataType == typeof(String))
            {
                p1.Editor = "textfield";
            }
            else if (DataType == typeof(int) || DataType == typeof(int?))
            {
                p1.Editor = "numberfield";
            }
            else if (DataType == typeof(double) || DataType == typeof(decimal)
                    || DataType == typeof(double?) || DataType == typeof(decimal?))
            {
                p1.Editor = "numberfield";
                if (p1.EditorConfig == null)
                    p1.EditorConfig = new Dictionary<string, object>();
                p1.EditorConfig.Add("allowDecimals", true);
            }
            else if (DataType == typeof(DateTime) || DataType == typeof(DateTime?))
            {
                p1.Editor = "gaodateeditor";
                if (p1.EditorConfig == null)
                    p1.EditorConfig = new Dictionary<string, object>();

                //p1.EditorConfig.Add("dateFormat", "Y-m-d");
                //p1.EditorConfig.Add("format", "Y-m-d");
                //p1.Format = "Y-m-d";
            }
            else if (DataType == typeof(Boolean) || DataType == typeof(Boolean?))
            {
                p1.Editor = "checkboxfield";
            }
            else if (DataType == typeof(Enum) || (DataType != null && DataType.BaseType == typeof(Enum)))
            {
                p1.Editor = "SelectEunmEditor";
                if (p1.EditorConfig == null)
                    p1.EditorConfig = new Dictionary<string, object>();
                
                p1.EditorConfig.Add("EditorEunmData", "{0}".FormatOrg(DataType.Name));
            }
        }
    }

}
