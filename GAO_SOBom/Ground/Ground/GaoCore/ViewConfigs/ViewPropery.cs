using GaoCore.ViewConfigs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace GaoCore
{
    public class ViewPropery
    {
        [JsonConverter(typeof(DataTypeJsonConverter))]
        public Type DataType { get; set; }
        public string Label { get; set; }
        public string Name { get; set; }
        public int? TotalWidth { get; set; }
        public int? LabelWidth { get; set; }
        public bool? Hidden { get; set; }

        public bool Editable { get; private set; } = true;
        public string Editor { get; set; }
        public bool IsCombobox { get; private set; }
        public string Format { get; set; }

        public Dictionary<string, object> EditorConfig { get; set; }
        //                                   setOtherField = { "CompNo": "CompNo", "Name": "CompName" };

        public LayoutConfig LayoutConfig { get; set; }
        public override string ToString()
        {
            ///JsonSerializerSettings jsetting = new JsonSerializerSettings();
            ///jsetting.ContractResolver = new LimitPropsContractResolver(new string[] { "Age", "IsMarry" });
            var s = JsonConvert.SerializeObject(this, Formatting.Indented);

            return s;
            //return JsonConvert.SerializeObject(this);
        }

        public ViewPropery UserGrid(int? TotalWidth = null, string Label = "", bool? Hidden = null)
        {
            this.TotalWidth = TotalWidth != null ? TotalWidth.Value : 100;
            if (string.IsNullOrEmpty(Label) == false)
                this.Label = Label;
            if (Hidden != null)
                this.Hidden = Hidden.Value;
            return this;
        }

        public ViewPropery SetEditable(bool Editable)
        {
            this.Editable = Editable;
            SetEditorConfig("editable", Editable);
            return this;
        }

        public ViewPropery SetLabel(string P_Label)
        {
            this.Label = P_Label;
            return this;
        }

        public ViewPropery SetLayoutConfig(LayoutConfig LConfig)
        {
            this.LayoutConfig = LConfig;
            return this;
        }

        public ViewPropery SetEditorConfig(string Key, object Value)
        {
            if (EditorConfig == null)
                EditorConfig = new Dictionary<string, object>();

            if (EditorConfig.ContainsKey(Key) == false)
                EditorConfig.Add(Key, Value);

            EditorConfig[Key] = Value;
            return this;
        }

        public ViewPropery SetComboboxEditor(string EditorXType)
        {
            this.IsCombobox = true;
            this.Editor = EditorXType;
            
            SetEditorConfig("iscombobox", true); 
            return this;
        }


        /// <summary>
        /// 下拉框在Grid中显示内容,不需要Renderer Display
        /// </summary>
        /// <returns></returns>
        public ViewPropery SetComboboxDisplayItSelf(bool IsYes)
        {
            SetEditorConfig("DisplayItSelf", IsYes);
            return this;
        }

        /// <summary>
        /// 下拉框同步字段
        /// </summary>
        /// <param name="CombobxField">控件记录的字段</param>
        /// <param name="TargetGridField">本视图要设置的字段</param>
        /// <returns></returns>
        public ViewPropery SetComboboxEditorSetter(string CombobxField, string TargetGridField)
        {
            if (EditorConfig.ContainsKey("SetterOtherField") == false)
                EditorConfig.Add("SetterOtherField", new Dictionary<string, string>());
            var setter = (Dictionary<string, string>)EditorConfig["SetterOtherField"];

            if (setter.ContainsKey(CombobxField) == false)
                setter.Add(CombobxField, TargetGridField);
            else
                setter[CombobxField] = TargetGridField;

            return this;
        }
    }
}
