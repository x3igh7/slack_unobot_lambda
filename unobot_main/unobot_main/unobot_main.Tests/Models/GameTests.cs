using System.Linq;
using unobot_main.Models;
using unobot_main.Models.Enums;
using Xunit;

namespace unobot_main.Tests.Models
{
    public class GameTests
    {
        [Fact]
        public void GameCreateTest()
        {
            var game = new Game();
            game.Create();

            Assert.True(game.Deck.Cards.Count == 108);
            Assert.True(game.Discard.Count == 0);
            Assert.True(game.Hands.Count == 0);
            Assert.True(game.Players.Count == 0);
            Assert.True(game.Turn.Value == 0);
            Assert.True(game.Status == GameStatus.Preparing);
            Assert.True(game.CurrentColor == Color.Wild);
            Assert.True(game.CurrentValue == string.Empty);
        }

        [Fact]
        public void GameAddPlayer()
        {
            var game = new Game();
            game.Create();

            var playerCount = game.Players.Count;
            var handCount = game.Hands.Count;
            var deckCount = game.Deck.Cards.Count;
            var player = new Player
            {
                Name = "Player1"
            };

            var result = game.AddPlayer(player);

            Assert.True(result);
            Assert.True(game.Players.Count == playerCount + 1);
            Assert.True(game.Hands.Count == handCount + 1);
            Assert.True(game.Hands.FirstOrDefault(h => h.Player.Name == player.Name) != null);
            Assert.True(game.Hands.First(h => h.Player.Name == player.Name).Cards.Count == 7);
            Assert.True(game.Deck.Cards.Count == deckCount - 7);
        }

        [Fact]
        public void GameAddTooManyPlayers()
        {
            var game = new Game();
            game.Create();

            for (var i = 1; i <= 4; i++)
            {
                var player = new Player
                {
                    Name = $"Player{i}"
                };

                game.AddPlayer(player);
            }

            var finalPlayer = new Player
            {
                Name = "Player5"
            };
            var result = game.AddPlayer(finalPlayer);

            Assert.True(!result);
            Assert.True(game.Players.Count == 4);
            Assert.True(game.Hands.FirstOrDefault(h => h.Player.Name == finalPlayer.Name) == null);
        }
    }
}