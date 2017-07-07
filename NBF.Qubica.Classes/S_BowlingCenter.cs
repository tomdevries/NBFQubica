using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBF.Qubica.Classes
{
    public class S_BowlingCenter
    {
        public long id { get; set; }
        public string name { get; set; }
        public string uri { get; set; }
        public int? centerId { get; set; }
        public string APIversion { get; set; }
        public int? numberOfLanes { get; set; }
        public DateTime? lastSyncDate { get; set; }
        public S_Scores scores { get; set; }
        public string appname { get; set; }
        public string secretkey { get; set; }

        public string address { get; set; }
        public string city { get; set; }
        public string email { get; set; }
        public string logo { get; set; }
        public string phonenumber { get; set; }
        public string website { get; set; }
        public string zipcode { get; set; }
    }
}
