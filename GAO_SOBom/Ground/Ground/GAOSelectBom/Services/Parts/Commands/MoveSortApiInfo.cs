using System;
using System.Collections.Generic;
using System.Text;

namespace GAOSelectBom.Services.Parts
{
    public  class MoveSortApiInfo
    {
        public bool IsMoveUp { get; set; } = true;
        //public int TargetEntityId { get; set; }
        public int CurrentEntityId { get; set; }
    }
}
