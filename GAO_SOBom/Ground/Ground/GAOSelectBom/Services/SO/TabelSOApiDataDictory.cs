using GAOSelectBom.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GAOSelectBom.Services.SO
{
    [Serializable]
    public class TabelSOApiDataDictory
    {
        public Dictionary<string, string> Header { get; set; }
        public List<Dictionary<string, string>> DetailList { get; set; }
        public List<Dictionary<string, string>> DetailZList { get; set; }
    }

}
