using System.Linq;
using unobot_main.Models.Enums;

namespace unobot_main.Models
{
    public class Action
    {
        public Game Game { get; set; }

        public Action(Game game)
        {
            this.Game = game;
        }

        public Hand GetHand(int turnIndex)
        {
            var player = this.Game.Players[turnIndex];
            return this.Game.Hands.Single(x => x.Player == player);
        }

        public void TakeAction(Card card)
        {
            if (!this.IsValidPlay(card))
            {
                return;
            }

            this.Game.CurrentColor = card.Color == Color.Wild ? card.NewColor : card.Color;
            this.Game.CurrentValue = card.Value;

            this.HandleSpecialActions(card);

            this.Game.Turn.ProgressTurn();
        }

        private void Draw(int draw)
        {
            var hand = this.GetHand(this.Game.Turn.GetNextTurnIndex());

            for (var i = 0; i < draw; i++)
                hand.Cards.Add(this.Game.Deck.Draw());
        }

        private void HandleSpecialActions(Card card)
        {
            switch (card.Value.ToLower())
            {
                case "r":
                    this.ReverseTurn();
                    break;
                case "s":
                    this.Game.Turn.ProgressTurn();
                    break;
                case "d2":
                    this.Draw(2);
                    this.Game.Turn.ProgressTurn();
                    break;
                case "d4":
                    this.Draw(4);
                    this.Game.Turn.ProgressTurn();
                    break;
                default:
                    break;
            }
        }

        private bool IsValidPlay(Card card)
        {
            if (card.Color == this.Game.CurrentColor)
            {
                return true;
            }

            if (card.Value == this.Game.CurrentValue)
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
                var hand = this.GetHand(this.Game.Turn.Value);
                var haveCurrentColor = hand.Cards.Any(c => c.Color == this.Game.CurrentColor);
                var haveCurrentValue = hand.Cards.Any(c => c.Value == this.Game.CurrentValue);

                return !haveCurrentValue && !haveCurrentColor;
            }

            return false;
        }

        private void ReverseTurn()
        {
            this.Game.Players.ToList().Reverse();
        }
    }
}