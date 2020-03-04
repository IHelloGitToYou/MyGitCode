using System;
using System.Collections.Generic;
using System.Text;

namespace GaoCore
{
    public enum PersistStatus
    {
        /// <summary>
        /// 新建中
        /// </summary>
        NEW = 0,
        /// <summary>
        /// 已修改
        /// </summary>
        MODIFY = 1,
        /// <summary>
        /// 在数据库记录 未变更过
        /// </summary>
        UNCHANGE =2,
        /// <summary>
        /// 数据库存在记录, 删除
        /// </summary>
        DELETED= 3
    }
}
