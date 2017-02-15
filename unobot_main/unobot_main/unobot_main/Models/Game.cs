using System.Collections.Generic;
using unobot_main.Models.Enums;

namespace unobot_main.Models
{
    public class Game
    {
        public Color CurrentColor { get; set; }
        public string CurrentValue { get; set; }
        public Deck Deck { get; set; }
        public Stack<Card> Discard { get; set; }
        public List<Hand> Hands { get; set; }
        public List<Player> Players { get; set; }
        public GameStatus Status { get; set; }
        public Turn Turn { get; set; }

        public Game()
        {
            this.Turn = new Turn(this);
        }

        public bool AddPlayer(Player player)
        {
            if (this.Players.Count == 4)
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

        public void Create()
        {
            var deck = this.NewDeck();

            this.Deck = deck;
            this.Players = new List<Player>();
            this.Hands = new List<Hand>();
            this.Discard = new Stack<Card>();
            this.Status = GameStatus.Preparing;
            this.CurrentColor = Color.Wild;
            this.CurrentValue = string.Empty;
        }

        public void Load()
        {
        }

        public void Pass()
        {
            var action = new Action(this);
            action.Pass();
        }

        public void Play(string input)
        {
            var card = Card.Create(input);
            var action = new Action(this);

            action.TakeAction(card);

            if (this.IsUno())
            {
                // return uno message
            }

            if (this.IsVictory())
            {
                // return is victory
                this.Status = GameStatus.Completed;
            }
        }

        public void RecycleDiscard()
        {
            var topCard = this.Discard.Pop();
            this.Deck.Cards = this.Discard;
            this.Deck.Shuffle();
            this.Discard = new Stack<Card>();
            this.Discard.Push(topCard);
        }

        private Stack<Card> CreateDiscard(Deck deck)
        {
            var discard = new Stack<Card>();
            var draw = deck.Draw();

            // Draw another card if WD4
            while (draw.Display == "WD4")
            {
                draw = deck.Draw();
            }

            // TODO: if W the next player chooses the color (maybe we just random for first pass)

            discard.Push(draw);
            return discard;
        }

        private bool IsUno()
        {
            return this.Hands[this.Turn.PreviousValue].Cards.Count == 1;
        }

        private bool IsVictory()
        {
            return this.Hands[this.Turn.PreviousValue].Cards.Count == 0;
        }

        private Deck NewDeck()
        {
            var deck = new Deck(this);
            deck.New();
            return deck;
        }
    }
}