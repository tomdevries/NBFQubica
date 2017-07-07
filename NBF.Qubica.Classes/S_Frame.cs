using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBF.Qubica.Classes
{
    public class S_Frame
    {
        public long id { get; set; }
        public long gameId { get; set; }
        public int frameNumber { get; set; }
        public int? progressiveTotal { get; set; }
        public bool isConvertedSplit { get; set; }
        public S_Bowl bowl1 { get; set; }
        public S_Bowl bowl2 { get; set; }
        public S_Bowl bowl3 { get; set; }
    }
}
