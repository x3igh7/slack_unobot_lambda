﻿using System;
using System.Collections.Generic;
using System.Linq;
using Amazon.DynamoDBv2.DataModel;
using unobot_main.Models.Enums;

namespace unobot_main.Models
{
    public class Deck
    {
        public Stack<Card> Cards { get; set; }

        [DynamoDBIgnore]
        public Game Game { get; set; }

        public Deck(Game game)
        {
            this.Game = game;
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
            var card = this.Cards.Pop();
            if (this.Cards.Count == 0)
            {
                this.Game.RecycleDiscard();
            }
            return card;
        }

        // Loads a deck from json
        public void Load()
        {
        }

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

        public void Shuffle(Random rnd = null)
        {
            if (rnd == null)
            {
                rnd = new Random();
            }
            this.Cards = new Stack<Card>(this.Cards.OrderBy(item => rnd.Next()));
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

        private void AddSpecialCards(Color color, string prefix)
        {
            for (var i = 0; i < 2; i++)
            {
                this.Cards.Push(Card.CreateSpecificCard(color, prefix, "S"));
                this.Cards.Push(Card.CreateSpecificCard(color, prefix, "D2"));
                this.Cards.Push(Card.CreateSpecificCard(color, prefix, "R"));
            }

            this.Cards.Push(Card.CreateSpecificCard(Color.Wild, Card.GetPrefixFromColor(Color.Wild), string.Empty));
            this.Cards.Push(Card.CreateSpecificCard(Color.Wild, Card.GetPrefixFromColor(Color.Wild), "4"));
        }

        private void NewCardsByColor(Color color)
        {
            var prefix = Card.GetPrefixFromColor(color);
            this.AddNumberedCards(color, prefix);
            this.AddSpecialCards(color, prefix);
        }
    }
}