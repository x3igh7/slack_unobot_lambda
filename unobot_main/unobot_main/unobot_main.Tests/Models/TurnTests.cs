using unobot_main.Models;
using unobot_main.Tests.Specs;
using Xunit;

namespace unobot_main.Tests.Models
{
    public class TurnTests
    {
        [Fact]
        public void GetNextTurnIndexTest()
        {
            var turn = TurnFactory.TwoPlayer();
            var currentTurn = turn.Value;

            var nextTurnIndex = turn.GetNextTurnIndex();

            Assert.True(nextTurnIndex == currentTurn + 1);
        }

        [Fact]
        public void GetNextTurnIndexAtEndOfPlayersTest()
        {
            var turn = TurnFactory.TwoPlayer();
            turn.Value = 1;

            var nextTurnIndex = turn.GetNextTurnIndex();

            Assert.True(nextTurnIndex == 0);
        }

        [Fact]
        public void ProgressTurnTest()
        {
            var turn = TurnFactory.TwoPlayer();
            var currentTurn = turn.Value;

            turn.ProgressTurn();

            Assert.True(turn.Value == currentTurn + 1);
            Assert.True(turn.PreviousValue == currentTurn);
        }

        [Fact]
        public void ProgressTurnTwiceTest()
        {
            var turn = TurnFactory.TwoPlayer();
            var currentTurn = turn.Value;

            turn.ProgressTurn(2);

            // Turn should appear to be the same in a two person game
            Assert.True(turn.Value == currentTurn);
            Assert.True(turn.PreviousValue == currentTurn);
        }
    }
}