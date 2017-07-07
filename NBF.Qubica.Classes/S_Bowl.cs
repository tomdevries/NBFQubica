using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBF.Qubica.Classes
{
    public class S_Bowl
    {
        public long id { get; set; }
        public long frameId { get; set; }
        public int bowlNumber { get; set; }
        public int? total { get; set; }
        public bool isStrike { get; set; }
        public bool isSpare { get; set; }
        public bool isSplit { get; set; }
        public bool isGutter { get; set; }
        public bool isFoul { get; set; }
        public bool isManuallyModified { get; set; }
        public String pins { get; set; }
    }
}
