using System.Collections.Generic;

namespace unobot_main.Models
{
    public class Hand
    {
        public Player Player { get; set; }

        public IList<Card> Cards { get; set; }
    }
}