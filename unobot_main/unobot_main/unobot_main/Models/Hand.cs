using System.Collections.Generic;

namespace unobot_main.Models
{
    public class Hand
    {
        public IList<Card> Cards { get; set; }
        public Player Player { get; set; }
    }
}