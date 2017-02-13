using System.Collections.Generic;
using unobot_main.Models.Enums;

namespace unobot_main.Models
{
    public class Game
    {
        public IList<Player> Players { get; set; }
        public Deck Deck { get; set; }
        public IList<Hand> Hands { get; set; }
        public Stack<Card> Discard { get; set; }
        public int Turn { get; set; }
        public GameStatus GameStatus { get; set; }

        public void Create()
        {
            var deck = this.NewDeck();
            var discard = this.NewDiscard(deck);

            this.Players = new List<Player>();
            this.Deck = deck;
            this.Hands = new List<Hand>();
            this.Discard = discard;
            this.Turn = 0;
            this.GameStatus = GameStatus.Preparing;
        }

        public void Load()
        {
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

        private Stack<Card> NewDiscard(Deck deck)
        {
            var discard = new Stack<Card>();
            discard.Push(deck.Draw());
            return discard;
        }
    }
}