using System.Collections.Generic;
using unobot_main.Models;
using unobot_main.Models.Enums;

namespace unobot_main.Tests.Specs
{
    public static class HandFactory
    {
        public static Hand DrawFour(Player player)
        {
            return SingleColorAndValue("D4", Color.Wild, player);
        }

        public static Hand DrawTwo(Color color, Player player)
        {
            return SingleColorAndValue("D2", color, player);
        }

        public static Hand New(List<Card> cards, Player player)
        {
            return new Hand
            {
                Cards = cards,
                Player = player
            };
        }

        public static Hand Reverse(Color color, Player player)
        {
            return SingleColorAndValue("R", color, player);
        }

        public static Hand SingleColor(Color color, Player player)
        {
            return new Hand
            {
                Cards = new List<Card>
                {
                    new Card
                    {
                        Color = color,
                        Display = $"{Card.GetPrefixFromColor(color)}0",
                        Value = "0"
                    },
                    new Card
                    {
                        Color = color,
                        Display = $"{Card.GetPrefixFromColor(color)}1",
                        Value = "1"
                    }
                },
                Player = player
            };
        }

        public static Hand SingleColorAndValue(string value, Color color, Player player)
        {
            return new Hand
            {
                Cards = new List<Card>
                {
                    new Card
                    {
                        Color = color,
                        Display = $"{Card.GetPrefixFromColor(color)}{value}",
                        Value = value
                    },
                    new Card
                    {
                        Color = color,
                        Display = $"{Card.GetPrefixFromColor(color)}{value}",
                        Value = value
                    }
                },
                Player = player
            };
        }

        public static Hand Skip(Color color, Player player)
        {
            return SingleColorAndValue("S", color, player);
        }

        public static Hand Wild(Player player)
        {
            return SingleColorAndValue(string.Empty, Color.Wild, player);
        }
    }
}