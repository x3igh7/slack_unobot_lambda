using System;
using System.Collections.Generic;
using System.Linq;
using unobot_main.Models;
using unobot_main.Models.Enums;
using unobot_main.Tests.Specs;
using Xunit;

namespace unobot_main.Tests.Models
{
    public class DeckTests
    {
        [Fact]
        public void DeckDealHandTest()
        {
            var game = GameFactory.New();
            var deck = new Deck(game);
            deck.New();

            var hand = deck.DealHand();

            Assert.True(deck.Cards.Count == 101);
            Assert.True(hand.Count == 7);
        }

        [Fact]
        public void DeckDraw()
        {
            var game = GameFactory.New();
            var deck = new Deck(game);
            deck.New();

            var draw = deck.Draw();

            Assert.True(deck.Cards.Count == 107);
            Assert.True(draw.GetType() == typeof(Card));
        }

        [Fact]
        public void DeckDrawWithDiscardRecycle()
        {
            var game = GameFactory.InProgress();
            game.Discard = game.Deck.Cards;
            var deck = new Deck(game) {Cards = new Stack<Card>()};
            deck.Cards.Push(new Card
            {
                Color = Color.Red,
                Display = "r1",
                Value = "1"
            });

            var discardCount = game.Discard.Count;
            var discardTop = game.Discard.Peek();
            deck.Draw();

            Assert.True(game.Discard.Count == 1);
            Assert.True(game.Deck.Cards.Count == discardCount - 1);
            Assert.True(game.Discard.Peek().Display == discardTop.Display);
        }

        [Fact]
        public void DeckShuffleTest()
        {
            var game = GameFactory.New();
            var seed = new Random(1);
            var deck = new Deck(game)
            {
                Cards = new Stack<Card>()
            };

            for (var i = 1; i <= 5; i++)
            {
                deck.Cards.Push(
                new Card
                {
                    Value = i.ToString()
                });
            }

            deck.Shuffle(seed);

            Assert.True(deck.Cards.First().Value == "2");
        }

        [Fact]
        public void NewDeckTest()
        {
            var game = GameFactory.New();
            var deck = new Deck(game);
            deck.New();

            Assert.True(deck.Cards.Count == 108);
        }
    }
}