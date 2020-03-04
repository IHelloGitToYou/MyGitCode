using GaoCore;
using GaoCore.ViewConfigs;
using GAOSelectBom.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GAOSelectBom.Services.Parts
{
    public class MoveUpCommand : ExtjsViewCommand
    {
        public MoveUpCommand()
        {
            IconCls = "x-fa fa-arrow-up";
            this.Text = "上移";
            this.CommandPath = "ExtGAO.view.part.commands.MoveUpCommand";
        }

        public override object Execute(string PostData)
        {
            var moveInfo = JsonConvert.DeserializeObject<MoveSortApiInfo>(PostData);
            var db = SqlSugarBase.GetDB(LoginUser);
            var allList = db.Queryable<Part>().OrderBy(o => o.Sort).ToList();

            var now = allList.Where(o => o.Id == moveInfo.CurrentEntityId).First();
            var nowIndex = allList.IndexOf(now);
            allList.Remove(now);
            allList.Insert(nowIndex - 1, now);

            int sortIndex = 0;
            foreach (var item in allList)
            {
                item.Sort = ++sortIndex;
                item.PersistStatus = PersistStatus.MODIFY;
            }

            RF<Part>.Save(allList, LoginUser);

            //newPart.PersistStatus = GaoCore.PersistStatus.NEW;

            ////计算最大顺序
            //var db = SqlSugarBase.GetDB(CurrentXLoginDB);
            //var maxSort = db.Queryable<Part>().Max(o => o.Sort);
            //newPart.Sort = maxSort + 1;
            //RF<Part>.Save(newPart, CurrentXLoginDB);
            //return newPart;

            return true;
        }
    }
}
