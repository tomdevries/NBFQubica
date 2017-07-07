using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBF.Qubica.Common
{
    public enum Status
    {
        NotPlayed = 0,
        Playing = 1,
        Played = 2
    }

    public enum OpenMode
    {
        Single = 0,
        Pair = 1,
        Group = 2,
        League = 3,
        Tournament = 4
    }
}
