using System;

namespace GaoCore
{
    [Serializable]
    public class LoginInfo
    {
        public string UserNo { get; set; }
        public string Password { get; set; }
        public string DB { get; set; }
    }
}
