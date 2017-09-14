using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBF.Qubica.Classes
{
    public class S_BowlScore
    {
        public int gamecode { get; set; }
        public int framenumber { get; set; }
        public int progressivetotal { get; set; }
        public int bowlnumber { get; set; }
        public int? total { get; set; }
        public bool isStrike { get; set; }
        public bool isSpare { get; set; }
    }
}
