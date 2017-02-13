using System.Collections.Generic;

namespace unobot_main.Models
{
    public class Game
    {
        public IList<Player> Players { get; set; }
        public Deck Deck { get; set; }
        public IList<Hand> Hands { get; set; }
        public int Turn { get; set; }

        public bool AddPlayer(Player player)
        {
            if (Players.Count >= 4)
            {
                return false;
            }

            Players.Add(player);
            return true;
        }
    }
}