using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Gao.Models;
using GaoCore;
using SqlSugar;
using GaoCore.ViewConfigs;

namespace Gao.Services.Users.Commands
{
    public class ListAddCommand: ExtjsViewCommand
    {
        public ListAddCommand()
        {
            this.Text = "添加";
            this.CommandPath = "ExtGAO.view.user.commands.ListAddCommand";
        }

        public override object Execute(string PostData)
        {
            var newUser = JsonConvert.DeserializeObject<User>(PostData);
            newUser.PersistStatus = GaoCore.PersistStatus.NEW;

            RF<User>.SaveFixDllDB(newUser, LoginUser);
            return newUser;

            ////string DbName = RunningControllers.CacheAssemblyDBs[typeof(User).Assembly];
            ////var db = SqlSugarBase.GetDB(DbName);

            ////var list1 = db.Queryable<User>()//.IgnoreColumns(u=>u.PersistStatus)
            ////                .Select<ViewModelUser>()
            ////                .ToList();

            ////////var list2 = db
            ////////            .Queryable<User, City>((u, c) => new JoinQueryInfos(JoinType.Left, u.CityId == c.Id))
            ////////            .Where((u, c) => c.Id == 1)
            ////////            .Select((a, b) => new { User = a, City = b })
            ////////            .ToList();

            ////var q = db.Queryable<User>("o");
            ////foreach (var item2 in User.ViewPropertys)
            ////{
            ////    var item = item2.Value;
            ////    q.AddJoinInfo(item.TableName, item.ShortName, "o.{0} = c.Id".FormatOrg(item.LeftJoinFieldName, item.TakeFieldName), JoinType.Left);
            ////}
            ////string selects = "o.*";
            ////foreach (var item in User.ViewPropertys)
            ////{
            ////    selects += ",{0}.{1} {2}".FormatOrg(item.Value.ShortName, item.Value.TakeFieldName, item.Key.Name);
            ////}
            ////q.Select(selects);
            ////q = q.MergeTable();

            ////q.Where(o => o.CityId == 1);

            
            ////q.Where("CityName ='广东'");
            ////// .Select("o.*, c.Name as CustomName ") 
            ////var list6 = q.ToDataTable();


            ////return PostData + " Api";
        }
    }
}
