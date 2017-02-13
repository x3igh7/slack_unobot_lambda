﻿using System.Collections.Generic;
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