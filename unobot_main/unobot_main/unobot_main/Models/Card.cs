using System;
using unobot_main.Models.Enums;

namespace unobot_main.Models
{
    public class Card
    {
        public Color Color { get; set; }
        public string Value { get; set; }
        public string Display { get; set; }
        public Color NewColor { get; set; }

        public static Card Create(string input)
        {
            string[] split;
            string value;

            input = input.Trim();
            if (input.Length == 3 && input[0].ToString().ToLower() == "w")
            {
                split = input.Split(' ');
                return CreateWildCard(Color.Wild, GetPrefixFromColor(Color.Wild), string.Empty, GetColorFromPrefix(split[1]));
            }

            if (input.Length == 5)
            {
                split = input.Split(' ');
                var card = split[0];
                value = GetValueFromInput(card);
                if (IsValidValue(value))
                {
                    return CreateWildCard(
                        Color.Wild,
                        GetPrefixFromColor(Color.Wild),
                        value,
                        GetColorFromPrefix(split[1]));
                }
            }

            var color = GetColorFromPrefix(input);
            value = GetValueFromInput(input);
            if (IsValidValue(value))
            {
                return CreateSpecificCard(color, GetPrefixFromColor(color), value);
            }

            throw new ArgumentException();
        }

        public static bool IsWildCard(Card card)
        {
            return card.Color == Color.Wild;
        }

        public static Card CreateWildCard(Color color, string prefix, string value, Color newColor)
        {
            var card = CreateSpecificCard(color, prefix, value);
            card.NewColor = newColor;
            return card;
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

        public static Color GetColorFromPrefix(string input)
        {
            if (input.Length == 0)
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

        private static string GetValueFromInput(string input)
        {
            return input.Substring(1).Trim();
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