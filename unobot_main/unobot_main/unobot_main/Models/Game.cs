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
            if (this.Players.Count >= 4)
            {
                return false;
            }

            this.Players.Add(player);
            this.Hands.Add(new Hand()
            {
                Player = player,
                Cards = this.Deck.DealHand()
            });
            return true;
        }
    }
}