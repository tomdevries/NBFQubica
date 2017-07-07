using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBF.Qubica.Common;

namespace NBF.Qubica.Classes
{
    public class S_Event
    {
        public long id { get; set; }
        public long eventCode { get; set; }
        public long scoresId { get; set; }
        public DateTime openDateTime { get; set; }
        public DateTime? closeDateTime  { get; set; }
        public Status status { get; set; }
        public OpenMode openMode { get; set; }
        public List<S_Game> games { get; set; }
    }
}
