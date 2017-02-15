using System.Linq;
using unobot_main.Models;
using unobot_main.Models.Enums;
using unobot_main.Tests.Specs;
using Xunit;

namespace unobot_main.Tests.Models
{
    public class ActionTests
    {
        [Fact]
        public void GetHandTest()
        {
            var game = GameFactory.InProgress(2);
            var action = new Action(game);

            var hand = action.GetHand(1);

            Assert.True(hand.Player.Name == "Player2");
        }

        [Fact]
        public void TakeActionWithCardInHandTest()
        {
            var game = GameFactory.InProgress();
            var action = new Action(game);
            var currentTurn = game.Turn.Value;
            var currentHand = action.GetHand(currentTurn);

            // add a valid card
            currentHand.Cards.Add(new Card {Color = Color.Red, Display = "r1", Value = "1"});

            // set the current cards for a valid play to be possible
            game.CurrentColor = Color.Red;
            game.CurrentValue = "1";

            var input = "r1";

            var inHandCount = currentHand.Cards.Count(c => c.Display == input);
            var discardCount = game.Discard.Count;
            var card = Card.Create(input);

            action.TakeAction(card);

            Assert.True(game.Turn.Value == currentTurn + 1);
            Assert.True(game.Discard.Peek().Display == card.Display);
            Assert.True(game.Discard.Count == discardCount + 1);
            Assert.True(game.CurrentColor == card.Color);
            Assert.True(game.CurrentValue == card.Value);

            var newHand = action.GetHand(currentTurn);
            var newInHandCount = newHand.Cards.Count(c => c.Display == card.Display);
            Assert.True(newInHandCount < inHandCount);
        }

        [Fact]
        public void TakeActionWithCardNotInHandTest()
        {
            var game = GameFactory.InProgress();
            var action = new Action(game);
            var currentTurn = game.Turn.Value;
            var currentHand = action.GetHand(currentTurn);
            var input = currentHand.Cards.First().Display;
            currentHand.Cards.RemoveAll(c => c.Color == Color.Wild);
            currentHand.Cards.RemoveAll(c => c.Display == input);
            var inHandCount = currentHand.Cards.Count(c => c.Display == input);
            var discardCount = game.Discard.Count;
            var discardTopCard = game.Discard.Peek().Display;
            var card = Card.Create(input);

            action.TakeAction(card);

            Assert.True(game.Turn.Value == currentTurn);
            Assert.True(game.Discard.Peek().Display == discardTopCard);
            Assert.True(game.Discard.Count == discardCount);

            var newHand = action.GetHand(currentTurn);
            var newInHandCount = newHand.Cards.Count(c => c.Display == input);
            Assert.True(newInHandCount == inHandCount);
        }

        [Fact]
        public void TakeActionWithCardNotValidTest()
        {
            var game = GameFactory.InProgress();
            game.CurrentColor = Color.Blue;
            game.CurrentValue = "1";
            game.Hands[0] = HandFactory.SingleColorAndValue("0", Color.Red, game.Hands[0].Player);

            var action = new Action(game);
            var currentTurn = game.Turn.Value;
            var currentHand = action.GetHand(currentTurn);
            var input = currentHand.Cards.First().Display;
            var inHandCount = currentHand.Cards.Count(c => c.Display == input);
            var discardCount = game.Discard.Count;
            var discardTopCard = game.Discard.Peek().Display;
            var card = Card.Create(input);

            action.TakeAction(card);

            Assert.True(game.Turn.Value == currentTurn);
            Assert.True(game.Discard.Peek().Display == discardTopCard);
            Assert.True(game.Discard.Count == discardCount);

            var newHand = action.GetHand(currentTurn);
            var newInHandCount = newHand.Cards.Count(c => c.Display == input);
            Assert.True(newInHandCount == inHandCount);
        }

        [Fact]
        public void TakeActionWithDrawFourCardInvalidPlayTest()
        {
            var game = GameFactory.InProgress();
            game.CurrentColor = Color.Red;
            game.Hands[0] = HandFactory.DrawFour(game.Hands[0].Player);
            game.Hands[0].Cards.Add(new Card {Color = Color.Red, Display = "r1", Value = "1"});
            var action = new Action(game);
            var currentTurn = game.Turn.Value;
            var currentHand = action.GetHand(currentTurn);

            var input = $"{currentHand.Cards.First(c => c.Value == "D4").Display} y";
            var discardCount = game.Discard.Count;
            var discardTopCard = game.Discard.Peek().Display;

            var card = Card.Create(input);
            var inHandCount = currentHand.Cards.Count(c => c.Display == card.Display);

            action.TakeAction(card);

            Assert.True(game.Turn.Value == currentTurn);
            Assert.True(game.Discard.Peek().Display == discardTopCard);
            Assert.True(game.Discard.Count == discardCount);

            var newHand = action.GetHand(currentTurn);
            var newInHandCount = newHand.Cards.Count(c => c.Display == card.Display);
            Assert.True(newInHandCount == inHandCount);
        }

        [Fact]
        public void TakeActionWithDrawFourCardTest()
        {
            var game = GameFactory.InProgress();
            game.CurrentColor = Color.Red;
            game.Hands[0] = HandFactory.DrawFour(game.Hands[0].Player);
            var action = new Action(game);
            var currentTurn = game.Turn.Value;
            var currentHand = action.GetHand(currentTurn);

            var input = $"{currentHand.Cards.First(c => c.Value == "D4").Display} y";
            var discardCount = game.Discard.Count;
            var deckCount = game.Deck.Cards.Count;
            var card = Card.Create(input);
            var inHandCount = currentHand.Cards.Count(c => c.Display == card.Display);

            action.TakeAction(card);

            // assuming a 2 player game, the turn index shuold appear the same
            Assert.True(game.Turn.Value == currentTurn);
            Assert.True(game.Discard.Peek().Display == card.Display);
            Assert.True(game.Discard.Count == discardCount + 1);
            Assert.True(game.CurrentColor == card.NewColor);
            Assert.True(game.CurrentValue == card.Value);
            Assert.True(game.Deck.Cards.Count == deckCount - 4);

            var newHand = action.GetHand(currentTurn);
            var newInHandCount = newHand.Cards.Count(c => c.Display == input);
            Assert.True(newInHandCount < inHandCount);
        }

        [Fact]
        public void TakeActionWithDrawTwoCardTest()
        {
            var game = GameFactory.InProgress();
            game.CurrentColor = Color.Red;
            game.Hands[0] = HandFactory.DrawTwo(Color.Red, game.Hands[0].Player);
            var action = new Action(game);
            var currentTurn = game.Turn.Value;
            var currentHand = action.GetHand(currentTurn);

            var input = currentHand.Cards.First(c => c.Value == "D2").Display;
            var inHandCount = currentHand.Cards.Count(c => c.Display == input);
            var discardCount = game.Discard.Count;
            var deckCount = game.Deck.Cards.Count;
            var card = Card.Create(input);

            action.TakeAction(card);

            // assuming a 2 player game, the turn index shuold appear the same
            Assert.True(game.Turn.Value == currentTurn);
            Assert.True(game.Discard.Peek().Display == input);
            Assert.True(game.Discard.Count == discardCount + 1);
            Assert.True(game.CurrentColor == card.Color);
            Assert.True(game.CurrentValue == card.Value);
            Assert.True(game.Deck.Cards.Count == deckCount - 2);

            var newHand = action.GetHand(currentTurn);
            var newInHandCount = newHand.Cards.Count(c => c.Display == input);
            Assert.True(newInHandCount < inHandCount);
        }

        [Fact]
        public void TakeActionWithReverseCardTest()
        {
            var game = GameFactory.InProgress(4);
            game.Turn.Value = 0;
            game.CurrentColor = Color.Red;
            game.Hands[0] = HandFactory.Reverse(Color.Red, game.Hands[0].Player);
            var action = new Action(game);
            var currentTurn = game.Turn.Value;
            var currentHand = action.GetHand(currentTurn);

            var input = currentHand.Cards.First(c => c.Value == "R").Display;
            var inHandCount = currentHand.Cards.Count(c => c.Display == input);
            var discardCount = game.Discard.Count;
            var card = Card.Create(input);

            action.TakeAction(card);

            // with 4 player game, the index should not change with reverse and should now be the last player
            Assert.True(game.Turn.Value == currentTurn);
            Assert.True(game.Players[game.Turn.Value].Name == "Player4");
            Assert.True(game.Discard.Peek().Display == input);
            Assert.True(game.Discard.Count == discardCount + 1);
            Assert.True(game.CurrentColor == card.Color);
            Assert.True(game.CurrentValue == card.Value);

            var newHand = action.GetHand(currentTurn);
            var newInHandCount = newHand.Cards.Count(c => c.Display == input);
            Assert.True(newInHandCount < inHandCount);
        }

        [Fact]
        public void TakeActionWithSkipCardTest()
        {
            var game = GameFactory.InProgress();
            game.CurrentColor = Color.Red;
            game.Hands[0] = HandFactory.Skip(Color.Red, game.Hands[0].Player);
            var action = new Action(game);
            var currentTurn = game.Turn.Value;
            var currentHand = action.GetHand(currentTurn);

            var input = currentHand.Cards.First(c => c.Value == "S").Display;
            var inHandCount = currentHand.Cards.Count(c => c.Display == input);
            var discardCount = game.Discard.Count;
            var card = Card.Create(input);

            action.TakeAction(card);

            // assuming a 2 player game, the turn index shuold appear the same
            Assert.True(game.Turn.Value == currentTurn);
            Assert.True(game.Discard.Peek().Display == input);
            Assert.True(game.Discard.Count == discardCount + 1);
            Assert.True(game.CurrentColor == card.Color);
            Assert.True(game.CurrentValue == card.Value);

            var newHand = action.GetHand(currentTurn);
            var newInHandCount = newHand.Cards.Count(c => c.Display == input);
            Assert.True(newInHandCount < inHandCount);
        }

        [Fact]
        public void TakeActionWithWildCardTest()
        {
            var game = GameFactory.InProgress();
            game.CurrentColor = Color.Red;
            game.Hands[0] = HandFactory.Wild(game.Hands[0].Player);
            var action = new Action(game);
            var currentTurn = game.Turn.Value;
            var currentHand = action.GetHand(currentTurn);

            var input = $"{currentHand.Cards.First(c => c.Value == string.Empty).Display} y";
            var discardCount = game.Discard.Count;
            var card = Card.Create(input);
            var inHandCount = currentHand.Cards.Count(c => c.Display == card.Display);

            action.TakeAction(card);

            // assuming a 2 player game, the turn index shuold appear the same
            Assert.True(game.Turn.Value == currentTurn + 1);
            Assert.True(game.Discard.Peek().Display == card.Display);
            Assert.True(game.Discard.Count == discardCount + 1);
            Assert.True(game.CurrentColor == card.NewColor);
            Assert.True(game.CurrentValue == card.Value);

            var newHand = action.GetHand(currentTurn);
            var newInHandCount = newHand.Cards.Count(c => c.Display == input);
            Assert.True(newInHandCount < inHandCount);
        }

        [Fact]
        public void PassTest()
        {
            var game = GameFactory.InProgress();
            var currentTurn = game.Turn.Value;
            var playerHandCount = game.Hands[currentTurn].Cards.Count;

            var action = new Action(game);
            action.Pass();

            Assert.True(game.Hands[currentTurn].Cards.Count == playerHandCount + 1);
            Assert.True(game.Turn.Value == currentTurn + 1);
        }
    }
}