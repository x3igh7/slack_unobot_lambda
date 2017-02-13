using System;
using System.Collections.Generic;
using System.Linq;
using unobot_main.Models.Enums;

namespace unobot_main.Models
{
    public class Deck
    {
        public Stack<Card> Cards { get; set; }

        // Creates a new deck
        public void New()
        {
            this.Cards = new Stack<Card>();
            this.NewCardsByColor(Color.Blue);
            this.NewCardsByColor(Color.Green);
            this.NewCardsByColor(Color.Red);
            this.NewCardsByColor(Color.Yellow);

            this.Shuffle();
        }

        // Loads a deck from json
        public void Load()
        {
        }

        public void Shuffle(Random rnd = null)
        {
            if (rnd == null)
            {
                rnd = new Random();
            }
            this.Cards = new Stack<Card>(this.Cards.OrderBy(item => rnd.Next()));
        }

        public List<Card> DealHand()
        {
            var hand = new List<Card>();
            for (var i = 0; i < 7; i++)
            {
                this.Shuffle();
                hand.Add(this.Cards.Pop());
            }

            return hand;
        }

        public Card Draw()
        {
            return this.Cards.Pop();
        }

        private void NewCardsByColor(Color color)
        {
            var prefix = this.GetColorPrefix(color);
            this.AddNumberedCards(color, prefix);
            this.AddSpecialCards(color, prefix);
        }

        private void AddNumberedCards(Color color, string prefix)
        {
            this.Cards.Push(
                    new Card
                    {
                        Color = color,
                        Value = 0.ToString(),
                        Display = $"{prefix}{0}"
                    });

            for (var j = 0; j < 2; j++)
            {
                for (var i = 1; i <= 9; i++)
                {
                    this.Cards.Push(
                        new Card
                        {
                            Color = color,
                            Value = i.ToString(),
                            Display = $"{prefix}{i}"
                        });
                }
            }
        }

        private void AddSpecialCards(Color color, string prefix)
        {
            for (var i = 0; i < 2; i++)
            {
                this.AddSpecialCard(color, prefix, "S");
                this.AddSpecialCard(color, prefix, "D2");
                this.AddSpecialCard(color, prefix, "R");
            }

            this.AddSpecialCard(Color.Wild, this.GetColorPrefix(Color.Wild), string.Empty);
            this.AddSpecialCard(Color.Wild, this.GetColorPrefix(Color.Wild), "4");
        }

        private void AddSpecialCard(Color color, string prefix, string value)
        {
            this.Cards.Push(
                new Card
                {
                    Color = color,
                    Value = value,
                    Display = $"{prefix}{value}"
                });
        }

        private string GetColorPrefix(Color color)
        {
            switch (color)
            {
                case Color.Red:
                    return "r";
                case Color.Blue:
                    return "b";
                case Color.Green:
                    return "g";
                case Color.Yellow:
                    return "y";
                case Color.Wild:
                    return "w";
                default:
                    throw new ArgumentOutOfRangeException(nameof(color), color, null);
            }
        }
    }
}