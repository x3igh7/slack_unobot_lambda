using System;

namespace unobot_main.Models
{
    public class Turn
    {
        public int PlayerCount { get; set; }
        public int Value { get; set; }

        public Turn()
        {
            this.Value = 0;
            this.PlayerCount = 1;
        }

        public int GetNextTurnIndex()
        {
            int turn;
            var turnLimit = this.PlayerCount;
            var turnCalc = this.Value + 1;
            if (turnCalc > turnLimit)
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