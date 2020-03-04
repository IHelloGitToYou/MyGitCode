using GaoCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GAOSelectBom.Models
{
    /// <summary>
    /// Bom类型
    /// </summary>
    [Serializable]
    public enum BomType
    {
        /// <summary>
        /// 1.记应收帐
        /// </summary>
        [Label("1.标准BOM")]
        Bom = 1,
        /// <summary>
        /// 2.不立帐
        /// </summary>
        [Label("2.订单BOM")]
        Bom_SO= 2
    }

}
