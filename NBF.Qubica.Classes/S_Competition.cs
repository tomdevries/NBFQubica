using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBF.Qubica.Classes
{
    public class S_Competition
    {
        public long id { get; set; }
        public long challengeid { get; set; }
        public string description { get; set; }
        public string price { get; set; }
        public DateTime startdate { get; set; }
        public DateTime enddate { get; set; }
    }
}
