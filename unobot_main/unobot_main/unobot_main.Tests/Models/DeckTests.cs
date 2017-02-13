using unobot_main.Models;
using Xunit;

namespace unobot_main.Tests.Models
{
    public class DeckTests
    {
        [Fact]
        public void NewDeckTest()
        {
            var deck = new Deck();
            deck.New();

            Assert.True(deck.Cards.Count == 108);
        }
    }
}