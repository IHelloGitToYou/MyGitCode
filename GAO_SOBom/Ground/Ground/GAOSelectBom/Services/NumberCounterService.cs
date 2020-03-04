using GaoCore;
using GAOSelectBom.Models;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace GAOSelectBom.Services
{
    public class NumberCounterService
    {
        GaoSqlSugarClient DB { get; set; }

        public NumberCounterService(GaoSqlSugarClient P_DB)
        {
            DB = P_DB;// Entity.GetDb(DbName);
        }

        public int NextCount(string TargerStr)
        {
            //自已一个线程,保证Id自增成功, 不管其他的事务是失败  
            //  Todo 为何还在同一事务内？ 
            var counter = DB.Queryable<NumberCounter>().Where(o => o.Targer == TargerStr).First();
            if (counter == null)
            {
                counter = new NumberCounter();
                counter.Targer = TargerStr;
                counter.NowNumber = 1;
                counter.PersistStatus = PersistStatus.NEW;
            }
            else
            {
                counter.PersistStatus = PersistStatus.MODIFY;
                counter.NowNumber = counter.NowNumber + 1;
            }

            RF<NumberCounter>.doSave(DB, counter);
            return counter.NowNumber;
        }
    }
}
