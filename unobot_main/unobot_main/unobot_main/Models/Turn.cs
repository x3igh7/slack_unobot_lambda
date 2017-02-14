using System;

namespace unobot_main.Models
{
    public class Turn
    {
        public Game Game { get; set; }
        // Value reprents the index of the player who currently is taking their turn
        public int Value { get; set; }

        public int PlayerCount => this.Game.Players.Count;

        public Turn(Game game)
        {
            this.Game = game;
            this.Value = 0;
        }

        public int GetNextTurnIndex()
        {
            int turn;
            var turnLimit = this.PlayerCount;
            var turnCalc = this.Value + 1;

            // because values are with 0 index, compare with index
            if (turnCalc >= turnLimit)
            {
                turn = turnCalc - turnLimit;
            }
            else
            {
                turn = turnCalc;
            }
            return turn;
        }

        public void ProgressTurn()
        {
            var turn = this.GetNextTurnIndex();
            this.Value = turn;
        }

        public void RandomizeTurnOrder()
        {
            var random = new Random();
            this.Value = random.Next(0, this.PlayerCount);
        }
    }
}