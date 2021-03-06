﻿using System;
using unobot_main.Models.Enums;

namespace unobot_main.Models
{
    public class Card
    {
        public Color Color { get; set; }
        public string Display { get; set; }
        public Color NewColor { get; set; }
        public string Value { get; set; }

        public static Card Create(string input)
        {
            input = input.Trim();
            Card card;
            if (HandleWildCard(input, out card))
            {
                return card;
            }

            if (HandleWildDrawFour(input, out card))
            {
                return card;
            }

            var color = GetColorFromPrefix(input);
            var value = GetValueFromInput(input);
            if (IsValidValue(value))
            {
                return CreateSpecificCard(color, GetPrefixFromColor(color), value);
            }

            throw new ArgumentException();
        }

        public static Card CreateSpecificCard(Color color, string prefix, string value)
        {
            return new Card
            {
                Color = color,
                Value = value,
                Display = $"{prefix}{value}"
            };
        }

        public static Card CreateWildCard(string value, Color newColor)
        {
            var card = CreateSpecificCard(Color.Wild, GetPrefixFromColor(Color.Wild), value);
            card.NewColor = newColor;
            return card;
        }

        public static Color GetColorFromPrefix(string input)
        {
            const int minInputLength = 1;
            const int maxInputLength = 5;
            if (input.Length < minInputLength || input.Length > maxInputLength)
            {
                throw new ArgumentOutOfRangeException(nameof(input), input, null);
            }

            var prefix = input[0].ToString().ToLower();
            switch (prefix)
            {
                case "r":
                    return Color.Red;
                case "b":
                    return Color.Blue;
                case "g":
                    return Color.Green;
                case "y":
                    return Color.Yellow;
                case "w":
                    return Color.Wild;
                default:
                    throw new ArgumentOutOfRangeException(nameof(input), prefix, null);
            }
        }

        public static string GetPrefixFromColor(Color color)
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

        public static bool IsWildCard(Card card)
        {
            return card.Color == Color.Wild;
        }

        private static string GetValueFromInput(string input)
        {
            return input.Substring(1).Trim();
        }

        private static bool HandleWildCard(string input, out Card card)
        {
            if (input.Length == 3 && input[0].ToString().ToLower() == "w")
            {
                var split = input.Split(' ');
                {
                    card = CreateWildCard(
                        string.Empty,
                        GetColorFromPrefix(split[1]));
                    return true;
                }
            }

            card = null;
            return false;
        }

        private static bool HandleWildDrawFour(string input, out Card card)
        {
            if (input.Length == 5)
            {
                var split = input.Split(' ');
                var play = split[0];
                var value = GetValueFromInput(play);
                if (IsValidValue(value))
                {
                    {
                        card = CreateWildCard(
                            value,
                            GetColorFromPrefix(split[1]));
                        return true;
                    }
                }
            }

            card = null;
            return false;
        }

        private static bool IsValidValue(string value)
        {
            switch (value.ToLower())
            {
                case "d2":
                    return true;
                case "r":
                    return true;
                case "s":
                    return true;
                case "d4":
                    return true;
                case "0":
                    return true;
                case "1":
                    return true;
                case "2":
                    return true;
                case "3":
                    return true;
                case "4":
                    return true;
                case "5":
                    return true;
                case "6":
                    return true;
                case "7":
                    return true;
                case "8":
                    return true;
                case "9":
                    return true;
                default:
                    return false;
            }
        }
    }
}