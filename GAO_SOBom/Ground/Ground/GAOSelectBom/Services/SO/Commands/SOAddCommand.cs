using GaoCore;
using GaoCore.ViewConfigs;
using GAOSelectBom.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GAOSelectBom.Services.SO
{
    public class SOAddCommand : ExtjsViewCommand
    {
        public SOAddCommand()
        {
            this.IconCls = "x-fa fa-plus";
            this.Text = "添加";
            this.CommandPath = "ExtGAO.view.so.commands.SOAddCommand";
        }

        public override object Execute(string PostData)
        {
            var db = SqlSugarBase.GetDB(LoginUser);
            var postDataBox = JsonConvert.DeserializeObject<TabelSOApiData>(PostData);
            bool hasTParts = postDataBox.DetailList.Any(o => o.Z_T_PRD_NO.IsNotEmpty());
            postDataBox.DetailZList = new List<TF_Pos_Z>();
            //var a = JsonConvert.DeserializeObject(postDataBox.DetailList[0].Z_T_JSON);
            postDataBox.Header.PersistStatus = PersistStatus.NEW;
            
            var service = new SOServices(db);

            for (int i = 0; i < postDataBox.DetailList.Count; i++)
            {
                var row = postDataBox.DetailList[i];
                if (row.Id <= -1)
                {
                    row.OS_NO = postDataBox.Header.OS_NO;
                   // int itm = postDataBox.DetailList[i].ITM;
                    int maxItm = postDataBox.DetailList.Max(o => o.ITM);
                    row.ITM = maxItm <= 0 ? 1 : ++maxItm;
                    row.PersistStatus = PersistStatus.NEW;
                    //设Z记录的Itm
                    if (hasTParts)
                    {
                        TF_Pos_Z tfPosZ = new TF_Pos_Z();
                        tfPosZ.PersistStatus = PersistStatus.NEW;
                        tfPosZ.OS_ID = row.OS_ID;
                        tfPosZ.OS_NO = row.OS_NO;
                        tfPosZ.ITM = row.ITM;
                        tfPosZ.Z_T_PRD_NO = row.Z_T_PRD_NO;
                        tfPosZ.Z_T_PRD_NAME = row.Z_T_PRD_NAME;
                        tfPosZ.Z_T_HIS_ID_NO = row.Z_T_HIS_ID_NO;
                        tfPosZ.Z_T_JSON = row.Z_T_JSON;
                        postDataBox.DetailZList.Add(tfPosZ);
                    }
                }
            }

            try
            {
                db.BeginTran();
                postDataBox.Header.OS_NO = service.GetNewOSNo(
                                            postDataBox.Header.OS_DD, 
                                            postDataBox.SOTableFormat);
                
                postDataBox.Header.USR = LoginUser.UserNo;

                RF<MF_Pos>.doSave(db, postDataBox.Header);
                foreach (var item in postDataBox.DetailList)
                {
                    if (item.Z_T_PRD_NO.IsNotEmpty() && item.Z_T_JSON.IsNotEmpty())
                    {
                        var partValues = service.GetZPartValue(item.Z_T_JSON);
                        string matchBomNo = service.ValidateBomZParts(item.Z_T_PRD_NO, partValues);

                        Prdt newPrdt = service.GenarateBomSO(matchBomNo, partValues, postDataBox.Header, "");
                        item.ID_NO = newPrdt.Prd_No + "->";
                        item.PRD_NO = newPrdt.Prd_No;
                        item.PRD_NAME = newPrdt.Name;
                    }

                    RF<TF_Pos>.doSave(db, item);
                    var tf_pos_Z2 = postDataBox.DetailZList.Where(o => o.ITM == item.ITM).FirstOrDefault();
                    if(tf_pos_Z2 != null)
                        RF<TF_Pos_Z>.doSave(db, tf_pos_Z2);
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
    }
}
