using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBF.Qubica.Classes
{
    public class S_User
    {
        public long id { get; set; }
        public string name { get; set; }
        public string password { get; set; }
        public Role roleid { get; set; }
        public double? logindatetime { get; set; }
        public string email { get; set; }
        public string address { get; set; }
        public string username { get; set; }
        public long frequentbowlernumber { get; set; }
        public string city { get; set; }
        public bool isMember { get; set; }
        public int? memberNumber { get; set; }
        public bool isRegistrationConfirmed { get; set; }
    }
}
