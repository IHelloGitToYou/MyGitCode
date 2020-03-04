using GaoCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gao.Models
{
    [RootEntity]
    public class ChangePassWordViewModel : ViewModel
    {
        public int UserId { get; set; }

        public int CompId { get; set; }

        [Label("原密码")]
        public string PassWork { get; set; }

        [Label("新密码")]
        public string NewPassWork { get; set; }

        [Label("新密码确认")]
        public string NewPassWork2 { get; set; }
    }
}
