using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBF.Qubica.Classes
{
    public class S_Scores
    {
        public long id { get; set; }
        public long bowlingCenterId { get; set; }
        public List<S_Event> Events { get; set; }
    }
}
