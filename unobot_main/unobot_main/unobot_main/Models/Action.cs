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
            var hand = this.GetHand(this.Game.Turn.Value);

            if (!this.IsValidPlay(hand, card))
            {
                // TODO: return some type of error message object to return to the user
                return;
            }

            this.Game.CurrentColor = card.Color == Color.Wild ? card.NewColor : card.Color;
            this.Game.CurrentValue = card.Value;

            if (!this.HandleSpecialActions(card))
            {
                this.Game.Turn.ProgressTurn();
            }

            this.Game.Discard.Push(this.RemoveCardFromHand(hand, card));
        }

        public void Pass()
        {
            var hand = this.GetHand(this.Game.Turn.Value);
            hand.Cards.Add(this.Game.Deck.Draw());
            this.Game.Turn.ProgressTurn();
        }

        private bool CardIsInHand(Hand hand, Card card)
        {
            return hand.Cards.Any(c => c.Display == card.Display);
        }

        private void Draw(int draw)
        {
            var hand = this.GetHand(this.Game.Turn.GetNextTurnIndex());

            for (var i = 0; i < draw; i++)
            {
                hand.Cards.Add(this.Game.Deck.Draw());
            }
        }

        private bool HandleSpecialActions(Card card)
        {
            switch (card.Value.ToLower())
            {
                case "r":
                    this.ReverseTurn();
                    return true;
                case "s":
                    this.Game.Turn.ProgressTurn(2);
                    return true;
                case "d2":
                    this.Draw(2);
                    this.Game.Turn.ProgressTurn(2);
                    return true;
                case "d4":
                    this.Draw(4);
                    this.Game.Turn.ProgressTurn(2);
                    return true;
                default:
                    return false;
            }
        }

        private bool IsValidPlay(Hand hand, Card card)
        {
            if (!this.CardIsInHand(hand, card))
            {
                return false;
            }

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
                if (card.Value.ToLower() != "d4")
                {
                    return true;
                }

                // if playing d4, you can't have another valid play.
                var haveCurrentColor = hand.Cards.Any(c => c.Color == this.Game.CurrentColor);
                var haveCurrentValue = hand.Cards.Any(c => c.Value == this.Game.CurrentValue) && this.Game.CurrentValue != "d4";

                return !haveCurrentValue && !haveCurrentColor;
            }

            return false;
        }

        private Card RemoveCardFromHand(Hand hand, Card card)
        {
            var inHand = hand.Cards.First(c => c.Display == card.Display);
            hand.Cards.Remove(inHand);
            return inHand;
        }

        private void ReverseTurn()
        {
            this.Game.Players.Reverse();
        }
    }
}