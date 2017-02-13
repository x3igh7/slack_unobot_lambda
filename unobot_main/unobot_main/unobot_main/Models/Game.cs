using System.Collections.Generic;

namespace unobot_main.Models
{
    public class Game
    {
        public IList<Player> Players { get; set; }
        public Deck Deck { get; set; }
        public IList<Hand> Hands { get; set; }
        public int Turn { get; set; }

        public void Create()
        {
            var deck = this.NewDeck();

            this.Players = new List<Player>();
            this.Deck = deck;
            this.Hands = new List<Hand>();
            this.Turn = 0;
        }

        public bool AddPlayer(Player player)
        {
            if (this.Players.Count >= 4)
            {
                return false;
            }

            this.Players.Add(player);
            this.Hands.Add(
                new Hand
                {
                    Player = player,
                    Cards = this.Deck.DealHand()
                });
            return true;
        }

        private Deck NewDeck()
        {
            var deck = new Deck();
            deck.New();
            return deck;
        }
    }
}