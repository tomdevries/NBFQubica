using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBF.Qubica.Classes
{
    public class S_Opentime
    {
        public long id { get; set; }
        public long bowlingCenterId { get; set; }
        public Day day { get; set; }
        public string openTime { get; set; }
        public string closeTime { get; set; }
    }
}
