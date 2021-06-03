using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess_Game
{
    class MatchResult : IComparable<MatchResult>
    {
        public int Turns;
        public Winner Winner;

        public int CompareTo(MatchResult other)
        {
            if (other == null)
                return 1;

            return Turns.CompareTo(other.Turns);
        }
    }
}
