using GaoCore;
using GaoCore.ViewConfigs;
using GAOSelectBom.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace GAOSelectBom.Services.Parts
{
    public class PartAddCommand : ExtjsViewCommand
    {
        public PartAddCommand()
        {
            this.IconCls = "x-fa fa-plus";
            this.Text = "添加";
            this.CommandPath = "ExtGAO.view.part.commands.PartAddCommand";
        }

        public override object Execute(string PostData)
        {
            var newPart = JsonConvert.DeserializeObject<Part>(PostData);
            newPart.PersistStatus = GaoCore.PersistStatus.NEW;

            //计算最大顺序
            var db = SqlSugarBase.GetDB(LoginUser);
            //todo 限制名称重复
            var maxSort = db.Queryable<Part>().Max(o => o.Sort);
            newPart.Sort = maxSort + 1;
            RF<Part>.Save(newPart, LoginUser);
            return newPart;
        }
    }
}
