using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace GaoCore
{
    public class RF
    {
        public static List<Dictionary<string, string>> ToDictionary(DataTable dt)
        {
            List<Dictionary<string, string>> result = new List<Dictionary<string, string>>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Dictionary<string, string> row = new Dictionary<string, string>();
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    row.Add(dt.Columns[j].ColumnName, dt.Rows[i][j].ToString());
                }
                result.Add(row);
            }
            return result;
        }

        public static List<Dictionary<string, string>> ToDictionary(List<DataRow> rows, DataTable dtStruct)
        {
            List<Dictionary<string, string>> result = new List<Dictionary<string, string>>();

            for (int i = 0; i < rows.Count(); i++)
            {
                Dictionary<string, string> row = new Dictionary<string, string>();
                for (int j = 0; j < dtStruct.Columns.Count; j++)
                {
                    row.Add(dtStruct.Columns[j].ColumnName, rows[i][j].ToString());
                }
                result.Add(row);
            }
            return result;
        }
        
        public static string ToJson(DataTable dt)
        {
            System.Text.StringBuilder jsonBuilder = new System.Text.StringBuilder();
            jsonBuilder.Append("[");
            if (dt.Rows.Count == 0)
            {
                jsonBuilder.Append("]");
                return jsonBuilder.ToString();
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var row = dt.Rows[i];
                jsonBuilder.Append("{");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    jsonBuilder.Append("'" + dt.Columns[j].ColumnName + "'");
                    jsonBuilder.Append(":'");
                    jsonBuilder.Append(row[j].ToString() + "'");

                    if (j < dt.Columns.Count - 1)
                        jsonBuilder.Append(",");
                }
                if (i < dt.Rows.Count - 1)
                    jsonBuilder.Append("},");
                else
                    jsonBuilder.Append("}");
            }

            jsonBuilder.Append("]");
            return jsonBuilder.ToString();
        }
    }

    public class RF<T> : RF where T: Entity, new()
    {
        public static void doSave(GaoSqlSugarClient db , T Entity)
        {
            if (db.LoginInfo == null)
                throw new InValidException("数据库Save必须要带LoginUser");

            if (Entity.PersistStatus == PersistStatus.NEW || Entity.Id < 0)
            {
                Entity.CreateDate = DateTime.Now;
                Entity.CreateId = db.LoginInfo.UserNo;
                Entity.UpdateDate = DateTime.Now;
                Entity.UpdateId = db.LoginInfo.UserNo;
                //取出 要插入的列 
                var cmd = db.Insertable<T>(Entity);

                //排除 非数据库列
                var ignoreCols = Entity.GetIgnoreProperys();
                cmd.IgnoreColumns("Id");
                foreach (var item in ignoreCols)
                {
                    cmd.IgnoreColumns(item);
                }
                //Entity = cmd.ExecuteReturnEntity();
                Entity.Id = cmd.ExecuteReturnIdentity();
                Entity.PersistStatus = PersistStatus.UNCHANGE;
            }
            else if (Entity.PersistStatus == PersistStatus.MODIFY || Entity.PersistStatus == PersistStatus.UNCHANGE)
            {
                Entity.UpdateDate = DateTime.Now;
                Entity.UpdateId = db.LoginInfo.UserNo;
                var cmd = db.Updateable<T>(Entity);
                //排除 非数据库列
                var ignoreCols = Entity.GetIgnoreProperys();
                cmd.IgnoreColumns("Id");
                foreach (var item in ignoreCols)
                {
                    cmd.IgnoreColumns(item);
                }
                cmd.Where(o => o.Id == Entity.Id);

                //if(cmd.WhereColumns())
                int cnt = cmd.ExecuteCommand();
                if (cnt == 0)
                    throw new InvalidCastException("无更新任何数据");

                Entity.PersistStatus = PersistStatus.UNCHANGE;
            }
            else if (Entity.PersistStatus == PersistStatus.DELETED)
            {
                var cmd = db.Deleteable<T>(Entity).In(Entity.Id);// Where(o => o.Id == Entity.Id);
                //cmd.Where(o => o.Id == Entity.Id);
                int cnt = cmd.ExecuteCommand();
                if (cnt == 0)
                    throw new InvalidCastException("无删除任何数据");
            }
        }

        /// <summary>
        /// 保存实体到数据库[使用实体指定的数据库,适用于固定的连接,如果TbrSystem内的实体]
        /// </summary>
        /// <param name="Entity"></param>
        /// <param name="DbName">指定数据库 ,如果无则找自己所属的库</param>
        /// <returns></returns>
        public static T SaveFixDllDB(T Entity, LoginInfo LoginUser)
        {
            //if (DbName.IsNullOrEmpty()) 
            string DbName = RunningControllers.CacheAssemblyDBs[Entity.GetType().Assembly];
            var db = SqlSugarBase.GetDB(DbName, LoginUser);

            doSave(db, Entity);

            return Entity;
        }

        public static List<T> SaveFixDllDB(List<T> Entitys, LoginInfo LoginUser)
        {
            //if (DbName.IsNullOrEmpty()) 
            string DbName = RunningControllers.CacheAssemblyDBs[Entitys.First().GetType().Assembly];
            var db = SqlSugarBase.GetDB(DbName, LoginUser);
            db.BeginTran();
            try
            {
                foreach (var item in Entitys)
                {
                    doSave(db, item);
                }
            }
            catch (Exception ex)
            {
                db.RollbackTran();
                throw ex;
            }

            db.CommitTran();
            return Entitys;
        }

        /// <summary>
        /// 保存实体到数据库
        /// </summary>
        /// <param name="Entity"></param>
        /// <param name="DbName">指定数据库 ,如果无则找自己所属的库</param>
        /// <returns></returns>
        public static T Save(T Entity, LoginInfo LoginUser)
        {
            if (LoginUser == null)
                throw new InValidException("未登录！");
            var db = SqlSugarBase.GetDB(LoginUser);
            doSave(db, Entity);
            return Entity;
        }

        public static List<T> Save(List<T> Entitys, LoginInfo LoginUser)
        {
            if (LoginUser == null)
                throw new InValidException("未登录！");

            var db = SqlSugarBase.GetDB(LoginUser);
            db.BeginTran();

            try
            {
                foreach (var item in Entitys)
                {
                    doSave(db, item);
                }
            }
            catch (Exception ex)
            {
                db.RollbackTran();
                throw ex;
            }

            db.CommitTran();
            return Entitys;
        }

        
        /// <summary>
        /// 生成查询  带出Left join 字段
        /// </summary>
        /// <param name="DbName">指定 数据库</param>
        /// <returns></returns>
        public static ISugarQueryable<T> GetSqlQueryWithViewProperty(string DbName = "")
        {
            SqlSugarClient db = null;
            if (DbName.IsNotEmpty())
                db = SqlSugarBase.GetDB(DbName);
            else
                db = SqlSugarBase.GetDB(RunningControllers.CacheAssemblyDBs[typeof(T).Assembly]);

            var q = db.Queryable<T>("o");
            foreach (var item2 in  Entity.ViewPropertys(typeof(T)))
            {
                var itemKey = item2.Key;
                var item = item2.Value;
                if(item.RightEqualFieldName.IsNullOrEmpty())
                    q.AddJoinInfo(item.TableName, item.ShortName, "o.{0} = {1}.Id".FormatOrg(item.LeftJoinFieldName, item.ShortName), JoinType.Left);
                else
                    q.AddJoinInfo(item.TableName, item.ShortName, "o.{0} = {1}.{2}".FormatOrg(item.LeftJoinFieldName, item.ShortName, item.RightEqualFieldName), JoinType.Left);
            }
            string selects = "o.*";
            foreach (var item in Entity.ViewPropertys(typeof(T)))
            {
                if(item.Value.TakeFieldAsName.IsNotEmpty())
                    selects += ",{0}.{1} {2}".FormatOrg(item.Value.ShortName,  item.Value.TakeFieldName, item.Value.TakeFieldAsName);
                else
                    selects += ",{0}.{1} {2}".FormatOrg(item.Value.ShortName, item.Value.TakeFieldName, item.Key.Name);
            }
            q.Select(selects);
            q = q.MergeTable();

            return q;
        }
    }

}
