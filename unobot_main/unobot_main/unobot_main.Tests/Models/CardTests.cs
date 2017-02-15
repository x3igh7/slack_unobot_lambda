using System;
using unobot_main.Models;
using unobot_main.Models.Enums;
using Xunit;

namespace unobot_main.Tests.Models
{
    public class CardTests
    {
        [Fact]
        public void CreateDrawTwoCardTest()
        {
            var input = "bD2";
            var result = Card.Create(input);

            Assert.True(result.Color == Color.Blue);
            Assert.True(result.Value == "D2");
            Assert.True(result.Display == "bD2");
        }

        [Fact]
        public void CreateRegularCardTest()
        {
            var input = "b4";
            var result = Card.Create(input);

            Assert.True(result.Color == Color.Blue);
            Assert.True(result.Value == "4");
            Assert.True(result.Display == "b4");
        }

        [Fact]
        public void CreateReverseCardTest()
        {
            var input = "bR";
            var result = Card.Create(input);

            Assert.True(result.Color == Color.Blue);
            Assert.True(result.Value == "R");
            Assert.True(result.Display == "bR");
        }

        [Fact]
        public void CreateSkipCardTest()
        {
            var input = "bS";
            var result = Card.Create(input);

            Assert.True(result.Color == Color.Blue);
            Assert.True(result.Value == "S");
            Assert.True(result.Display == "bS");
        }

        [Fact]
        public void CreateSpecificCardTest()
        {
            var result = Card.CreateSpecificCard(Color.Blue, "b", "2");

            Assert.True(result.Color == Color.Blue);
            Assert.True(result.Value == "2");
            Assert.True(result.Display == "b2");
        }

        [Fact]
        public void CreateWildCardFromInputTest()
        {
            var input = "w r";
            var result = Card.Create(input);

            Assert.True(result.Color == Color.Wild);
            Assert.True(result.Value == string.Empty);
            Assert.True(result.Display == "w");
            Assert.True(result.NewColor == Color.Red);
        }

        [Fact]
        public void CreateWildCardTest()
        {
            var result = Card.CreateWildCard("D4", Color.Blue);

            Assert.True(result.NewColor == Color.Blue);
            Assert.True(result.Color == Color.Wild);
            Assert.True(result.Value == "D4");
            Assert.True(result.Display == "wD4");
        }

        [Fact]
        public void CreateWildDrawFourCardFromInputTest()
        {
            var input = "wD4 r";
            var result = Card.Create(input);

            Assert.True(result.Color == Color.Wild);
            Assert.True(result.Value == "D4");
            Assert.True(result.Display == "wD4");
            Assert.True(result.NewColor == Color.Red);
        }

        [Fact]
        public void GetBlueColorFromPrefixTest()
        {
            var result = Card.GetColorFromPrefix("b4");

            Assert.True(result == Color.Blue);
        }

        [Fact]
        public void GetBluePrefixFromColorTest()
        {
            var result = Card.GetPrefixFromColor(Color.Blue);

            Assert.True(result == "b");
        }

        [Fact]
        public void GetColorFromPrefixTestErrorMaximumInput()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Card.GetColorFromPrefix("wd4 red"));
        }

        [Fact]
        public void GetColorFromPrefixTestErrorMinimumInput()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Card.GetColorFromPrefix(string.Empty));
        }

        [Fact]
        public void GetGreenColorFromPrefixTest()
        {
            var result = Card.GetColorFromPrefix("g4");

            Assert.True(result == Color.Green);
        }

        [Fact]
        public void GetGreenPrefixFromColorTest()
        {
            var result = Card.GetPrefixFromColor(Color.Green);

            Assert.True(result == "g");
        }

        [Fact]
        public void GetPrefixFromColorErrorTest()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Card.GetPrefixFromColor((Color) 10));
        }

        [Fact]
        public void GetRedColorFromPrefixTest()
        {
            var result = Card.GetColorFromPrefix("r4");

            Assert.True(result == Color.Red);
        }

        [Fact]
        public void GetRedPrefixFromColorTest()
        {
            var result = Card.GetPrefixFromColor(Color.Red);

            Assert.True(result == "r");
        }

        [Fact]
        public void GetWildColorFromPrefixTest()
        {
            var result = Card.GetColorFromPrefix("w");

            Assert.True(result == Color.Wild);
        }

        [Fact]
        public void GetWildPrefixFromColorTest()
        {
            var result = Card.GetPrefixFromColor(Color.Wild);

            Assert.True(result == "w");
        }

        [Fact]
        public void GetYellowColorFromPrefixTest()
        {
            var result = Card.GetColorFromPrefix("y4");

            Assert.True(result == Color.Yellow);
        }

        [Fact]
        public void GetYellowPrefixFromColorTest()
        {
            var result = Card.GetPrefixFromColor(Color.Yellow);

            Assert.True(result == "y");
        }

        [Fact]
        public void IsWildCardFalseTest()
        {
            var card = new Card
            {
                Color = Color.Blue,
                Value = "3",
                Display = "b3"
            };

            var result = Card.IsWildCard(card);
            Assert.False(result);
        }

        [Fact]
        public void IsWildCardTrueTest()
        {
            var card = new Card
            {
                Color = Color.Wild,
                Value = "D4",
                Display = "wD4"
            };

            var result = Card.IsWildCard(card);
            Assert.True(result);
        }
    }
}