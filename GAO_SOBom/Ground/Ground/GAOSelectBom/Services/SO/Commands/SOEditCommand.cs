using GaoCore;
using GaoCore.ViewConfigs;
using GAOSelectBom.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GAOSelectBom.Services.SO
{
    public class SOEditCommand : ExtjsViewCommand
    {
        public SOEditCommand()
        {
            this.IconCls = "x-fa fa-pencil-square-o";
            this.Text = "修改";
            this.CommandPath = "ExtGAO.view.so.commands.SOEditCommand";
        }

        public override object Execute(string PostData)
        {
            var db = SqlSugarBase.GetDB(LoginUser);
            var postDataBox = JsonConvert.DeserializeObject<TabelSOApiData>(PostData, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
            bool hasTParts = postDataBox.DetailList.Any(o => o.Z_T_JSON.IsNotEmpty());
            postDataBox.DetailZList = new List<TF_Pos_Z>();
            string OS_NO = postDataBox.Header.OS_NO;


            var service = new SOServices(db);
            if (service.SoIsCheck(OS_NO))
            {
                throw new InValidException("订单已审核不能修改!");
            }

            postDataBox.Header.PersistStatus = PersistStatus.MODIFY;
            try
            {
                db.BeginTran();
                RF<MF_Pos>.doSave(db, postDataBox.Header);//.Updateable<MF_Pos>(postDataBox.Header).Where(o => o.Id == postDataBox.Header.Id).ExecuteCommand();
                int maxItm = db.Queryable<TF_Pos>()
                                .Where(o => o.OS_NO == OS_NO)
                                .Max(O => O.ITM);


                foreach (var item in postDataBox.DetailList)
                {
                    if (item.PRD_NO.IsNullOrEmpty())
                        throw new InValidException("货号不能为空");

                    if (item.PersistStatus == PersistStatus.NEW)
                    {
                        item.OS_NO = OS_NO;
                        item.ITM = ++maxItm;
                        item.PersistStatus = PersistStatus.NEW;

                        if (item.Z_T_JSON.IsNotEmpty())
                        {
                            var partValues = service.GetZPartValue(item.Z_T_JSON);
                            string matchBomNo = service.ValidateBomZParts(item.Z_T_PRD_NO, partValues);

                            Prdt newPrdt = service.GenarateBomSO(matchBomNo, partValues, postDataBox.Header, "");
                            item.ID_NO = newPrdt.Prd_No + "->";
                            item.PRD_NO = newPrdt.Prd_No;
                            item.PRD_NAME = newPrdt.Name;
                        }

                        RF<TF_Pos>.doSave(db, item);
                        //设Z记录的Itm
                        if (hasTParts)
                        {
                            TF_Pos_Z tfPosZ = new TF_Pos_Z();
                            tfPosZ.PersistStatus = PersistStatus.NEW;
                            setTFZInfo(item, tfPosZ);
                            RF<TF_Pos_Z>.doSave(db, tfPosZ);
                        }
                    }
                    else if (item.PersistStatus == PersistStatus.MODIFY)
                    {
                        // todo 检测有无SA过？

                        var tfPosZDB = db.Queryable<TF_Pos_Z>().Where(o => o.OS_NO == OS_NO && o.ITM == item.ITM).First();
                        //重建 或 增加Bom
                        NewOrReBulidBom(tfPosZDB, item, service, postDataBox);

                        if (tfPosZDB == null)
                        {
                            tfPosZDB = new TF_Pos_Z();
                            tfPosZDB.PersistStatus = PersistStatus.NEW;
                        }
                        else
                        {
                            tfPosZDB.PersistStatus = PersistStatus.MODIFY;
                        }

                        RF<TF_Pos>.doSave(db, item);
                        setTFZInfo(item, tfPosZDB);
                        RF<TF_Pos_Z>.doSave(db, tfPosZDB);
                    }
                    else if (item.PersistStatus == PersistStatus.DELETED)
                    {
                        // todo 检测有无SA过？
                        var tfPosZ = db.Queryable<TF_Pos_Z>().Where(o => o.OS_NO == OS_NO && o.ITM == item.ITM).First();
                        if (tfPosZ != null && tfPosZ.Z_T_JSON.IsNotEmpty())
                        {
                            var tfOld = db.Queryable<TF_Pos>().Where(O => O.OS_NO == OS_NO && O.ITM == item.ITM).First();
                            string tBomNo = tfOld.ID_NO;
                            //以防有人改了配方,把其他的订单配方删除了
                            if (tBomNo.StartsWith(tfOld.PRD_NO) == true && tBomNo.Length > tfOld.PRD_NO.Length)
                            {
                                db.Ado.ExecuteCommand("Delete TF_BOM_SO where BOM_NO = '{0}'".FormatOrg(tfOld.ID_NO));
                                db.Ado.ExecuteCommand("Delete MF_BOM_SO where BOM_NO = '{0}'".FormatOrg(tfOld.ID_NO));
                            }
                        }

                        //设Z记录的Itm
                        RF<TF_Pos>.doSave(db, item);
                        if (tfPosZ != null)
                        {
                            tfPosZ.PersistStatus = PersistStatus.DELETED;
                            RF<TF_Pos_Z>.doSave(db, tfPosZ);
                        }
                    }
                }
                db.CommitTran();
            }
            catch(Exception ex)
            {
                db.RollbackTran();
                throw ex;
            }
            return true;
        }

        /// <summary>
        /// 检测有无变过选配项
        /// </summary>
        /// <param name="OldZ"></param>
        /// <param name="NowTF"></param>
        public void NewOrReBulidBom(TF_Pos_Z OldZ, TF_Pos NowTF, SOServices service, TabelSOApiData PostDataBox)
        {
            var db = service.DB;
            string oldTJson = OldZ == null ? "" : OldZ.Z_T_JSON;
            string newTJson = NowTF.Z_T_JSON;

            if (oldTJson == newTJson)
                return;

            //旧无,新有 ,新建
            if (oldTJson.IsNullOrEmpty() && newTJson.IsNotEmpty())
            {
                var partValues = service.GetZPartValue(newTJson);
                string matchBomNo = service.ValidateBomZParts(NowTF.Z_T_PRD_NO, partValues);

                Prdt newPrdt = service.GenarateBomSO(matchBomNo, partValues, PostDataBox.Header, "");
                NowTF.ID_NO = newPrdt.Prd_No + "->";
                NowTF.PRD_NO = newPrdt.Prd_No;
                NowTF.PRD_NAME = newPrdt.Name;
            }

            //旧与新不一样,重建
            if (oldTJson.IsNotEmpty() && newTJson.IsNotEmpty())
            {
                var tfOld = db.Queryable<TF_Pos>().Where(O => O.OS_NO == OldZ.OS_NO && O.ITM == OldZ.ITM).First();
                string tBomNo = tfOld.ID_NO;
                //以防有人改了配方,把其他的订单配方删除了
                if (tBomNo.StartsWith(tfOld.PRD_NO) == false && tBomNo.Length == tfOld.PRD_NO.Length)
                    return;

                var partValues = service.GetZPartValue(newTJson);
                string matchBomNo = service.ValidateBomZParts(NowTF.Z_T_PRD_NO, partValues);

                db.Ado.ExecuteCommand("Delete TF_BOM_SO where BOM_NO = '{0}'".FormatOrg(tfOld.ID_NO));
                db.Ado.ExecuteCommand("Delete MF_BOM_SO where BOM_NO = '{0}'".FormatOrg(tfOld.ID_NO));

                Prdt newPrdt = service.GenarateBomSO(matchBomNo, partValues, PostDataBox.Header, NowTF.PRD_NO);
                //NowTF.ID_NO = newPrdt.Prd_No + "->";
                //NowTF.PRD_NO = newPrdt.Prd_No;
                NowTF.PRD_NAME = newPrdt.Name;
            }
            //旧有,新无 , 删除
            if (oldTJson.IsNotEmpty() && newTJson.IsNullOrEmpty())
            {
                var tfOld = db.Queryable<TF_Pos>().Where(O => O.OS_NO == OldZ.OS_NO && O.ITM == OldZ.ITM).First();
                string tBomNo = tfOld.ID_NO;
                //以防有人改了配方,把其他的订单配方删除了
                if (tBomNo.StartsWith(tfOld.PRD_NO) == false && tBomNo.Length == tfOld.PRD_NO.Length)
                    return;

                db.Ado.ExecuteCommand("Delete TF_BOM_SO where BOM_NO = '{0}'".FormatOrg(tfOld.ID_NO));
                db.Ado.ExecuteCommand("Delete MF_BOM_SO where BOM_NO = '{0}'".FormatOrg(tfOld.ID_NO));
                NowTF.ID_NO = "";
                //NowTF.PRD_NO = newPrdt.Prd_No;
                //NowTF.PRD_NAME = newPrdt.Name;
            }
        }
        

        public void setTFZInfo(TF_Pos item, TF_Pos_Z targerTfPosZ)
        {
            targerTfPosZ.OS_ID = item.OS_ID;
            targerTfPosZ.OS_NO = item.OS_NO;
            targerTfPosZ.ITM = item.ITM;
            targerTfPosZ.Z_T_PRD_NO = item.Z_T_PRD_NO;
            targerTfPosZ.Z_T_PRD_NAME = item.Z_T_PRD_NAME;
            targerTfPosZ.Z_T_HIS_ID_NO = item.Z_T_HIS_ID_NO;
            targerTfPosZ.Z_T_JSON = item.Z_T_JSON;
        }
    }
}
