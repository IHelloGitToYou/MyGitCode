using GAOWebAPI.Register;
using GAOWebAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static GAOWebAPI.Services.SqlSugarBase;

namespace GAOWebAPI.Controllers
{
    [Route("api/[controller]/[action]/{id?}")]
    public class BomRegisterController : BaseController
    {
        BomRegisterService Service;
        public BomRegisterController()
        {
            Service = new BomRegisterService(this);
        }


        [HttpGet]
        public bool? Get_IsValidClient()
        {
            bool? result = Service.Get_IsValidClient();
            return result;
        }

        [HttpGet]
        public bool TryGet(DateTime AskClientDate)
        {
            bool result = false;
            var now = Service.TryGet(AskClientDate);
            if (now == null)
            {
                result = false;
            }

            return result;
        }

        [HttpPost]
        public bool TryToRegister([FromBody]TryToRegisterPostData Register)
        {
            ClientInfo cInfo = SqlSugarBase.GetClientInfo();
            if (cInfo == null)
                return false;

            var serial = Register.Serials.Where(o => o.NO == cInfo.SerialId).FirstOrDefault();
            if (serial == null)
                return false;

            //serial.DATE = new DateTime(2019,12,26);
            //serial.PASSWORK = "0k2HjKrOFDpLV4Ns98mkpQ==";//20 "diI8qkOVaoJJawcR0hFEfA ==";
            bool result = Service.Register(serial.NO, serial.DATE, serial.PASSWORK, Register.NetWorkTime);
            //注册成功了，有可能第一次就是失效的
            if(result == true)
            {
                result = Service.TryGet(Register.NetWorkTime) != null ? true : false;
            }
            return result;
        }


        public class TryToRegisterPostData
        {
            public TryToRegisterPostData()
            {
                Serials = new List<RegisterSerial>();
            }

            public DateTime NetWorkTime
            {
                get; set;
            }
            public List<RegisterSerial> Serials;
        }

        [Serializable]
        public class RegisterSerial
        {
            public string NO { get; set; }
            public DateTime DATE { get; set; }
            public string PASSWORK { get; set; }
        }
       




    }
}
