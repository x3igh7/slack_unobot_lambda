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
        }
    }
}