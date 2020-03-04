using GAOSelectBom.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GAOSelectBom.Services.SO
{
    [Serializable]
    public class TabelSOApiData
    {
        public string SOTableFormat { get; set; }
        public MF_Pos Header { get; set; }
        public List<TF_Pos> DetailList { get; set; }
        public List<TF_Pos_Z> DetailZList { get; set; }
    }

}
