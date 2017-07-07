using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBF.Qubica.Classes
{
    public class S_Game
    {
        public long id { get; set; }
        public long gameCode { get; set; }
        public long eventId { get; set; }
        public int laneNumber { get; set; }
        public String playerName { get; set; }
        public String fullName { get; set; }
        public String freeEntryCode { get; set; }
        public int playerPosition { get; set; }
        public DateTime startDateTime { get; set; }
        public DateTime? endDateTime { get; set; }
        public int gameNumber { get; set; }
        public int? handicap { get; set; }
        public int? total { get; set; }
        public List<S_Frame> frames { get; set; }

        public int totalStrikesInGame { get; set; }
        public int totalSparesInGame  { get; set; }

        public int totalStrikesInMonth { get; set; }
        public int totalSparesInMonth { get; set; }
    }
}
