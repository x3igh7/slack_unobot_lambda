using System.Collections.Generic;
using System.Linq;
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
        public GameStatus Status { get; set; }
        public string CurrentValue { get; set; }
        public Color CurrentColor { get; set; }

        public void Create()
        {
            var deck = this.NewDeck();

            this.Deck = deck;
            this.Players = new List<Player>();
            this.Hands = new List<Hand>();
            this.Discard = new Stack<Card>();
            this.Turn = 0;
            this.Status = GameStatus.Preparing;
            this.CurrentColor = Color.Wild;
            this.CurrentValue = string.Empty;
        }

        public void Load()
        {
        }

        public void Play(Card card)
        {
            if (this.IsValidPlay(card))
            {
                this.TakeAction(card);
            }
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

        public void ProgressTurn()
        {
            var turn = this.GetNextTurnIndex();

            this.Turn = turn;
        }

        private int GetNextTurnIndex()
        {
            int turn;
            var turnLimit = this.Players.Count;
            var turnCalc = this.Turn + 1;
            if ((turnCalc) > turnLimit)
            {
                turn = (turnCalc) - turnLimit;
            }
            else
            {
                turn = turnCalc;
            }
            return turn;
        }

        private void TakeAction(Card card)
        {
            this.CurrentColor = card.Color == Color.Wild ? card.NewColor : card.Color;
            this.CurrentValue = card.Value;

            this.HandleSpecialActions(card);

            this.ProgressTurn();
        }

        private void HandleSpecialActions(Card card)
        {
            switch (card.Value.ToLower())
            {
                case "r":
                    this.ReverseTurn();
                    break;
                case "s":
                    this.ProgressTurn();
                    break;
                case "d2":
                    this.Draw(2);
                    this.ProgressTurn();
                    break;
                case "d4":
                    this.Draw(4);
                    this.ProgressTurn();
                    break;
                default:
                    break;
            }
        }

        private void ReverseTurn()
        {
            this.Players.ToList().Reverse();
        }

        private void Draw(int draw)
        {
            var hand = this.GetHand(this.GetNextTurnIndex());

            for (var i = 0; i < draw; i++)
            {
                hand.Cards.Add(this.Deck.Draw());
            }
        }

        private Hand GetHand(int turnIndex)
        {
            var player = this.Players[turnIndex];
            return this.Hands.Single(x => x.Player == player);
        }

        private bool IsValidPlay(Card card)
        {
            if (card.Color == this.CurrentColor)
            {
                return true;
            }

            if (card.Value == this.CurrentValue)
            {
                return true;
            }

            if (Card.IsWildCard(card))
            {
                if (card.Value != "d4")
                {
                    return true;
                }

                // if playing d4, you can't have another valid play.
                var hand = this.GetHand(this.Turn);
                var haveCurrentColor = hand.Cards.Any(c => c.Color == this.CurrentColor);
                var haveCurrentValue = hand.Cards.Any(c => c.Value == this.CurrentValue);

                return !haveCurrentValue && !haveCurrentColor;
            }

            return false;
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

            // TODO: if W the next player chooses the color

            discard.Push(draw);
            return discard;
        }

        private Deck NewDeck()
        {
            var deck = new Deck();
            deck.New();
            return deck;
        }
    }
}