using System.Collections.Generic;

namespace unobot_main.Models
{
    public class Hand
    {
        public List<Card> Cards { get; set; }
        public Player Player { get; set; }
    }
}