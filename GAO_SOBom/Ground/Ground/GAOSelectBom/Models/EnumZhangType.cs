using GaoCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GAOSelectBom.Models
{
    /// <summary>
    /// 立帐方式
    /// </summary>
    [Serializable]
    public enum EnumZhangType
    {
        //[Label("立帐方式 1. 记应收帐  2.不立帐  3.开票记账")]
        /// <summary>
        /// 1.记应收帐
        /// </summary>
        [Label("1.记应收帐")]
        YingShou = 1,
        /// <summary>
        /// 2.不立帐
        /// </summary>
        [Label("2.不立帐")]
        No = 2,
        /// <summary>
        /// 3.开票记账
        /// </summary>
        [Label("3.开票记账")]
        KaiPiao = 3
    }

}
