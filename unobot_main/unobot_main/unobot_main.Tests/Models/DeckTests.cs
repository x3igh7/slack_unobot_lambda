using System;
using System.Collections.Generic;
using System.Linq;
using unobot_main.Models;
using Xunit;

namespace unobot_main.Tests.Models
{
    public class DeckTests
    {
        [Fact]
        public void DeckDealHandTest()
        {
            var deck = new Deck();
            deck.New();

            var hand = deck.DealHand();

            Assert.True(deck.Cards.Count == 101);
            Assert.True(hand.Count == 7);
        }

        [Fact]
        public void DeckDraw()
        {
            var deck = new Deck();
            deck.New();

            var draw = deck.Draw();

            Assert.True(deck.Cards.Count == 107);
            Assert.True(draw.GetType() == typeof(Card));
        }

        [Fact]
        public void DeckShuffleTest()
        {
            var seed = new Random(1);
            var deck = new Deck
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
            var deck = new Deck();
            deck.New();

            Assert.True(deck.Cards.Count == 108);
        }
    }
}