using unobot_main.Models;

namespace unobot_main.Tests.Specs
{
    public class TurnFactory
    {
        public static Turn New(Game game)
        {
            return new Turn(game);
        }

        public static Turn TwoPlayer()
        {
            var game = GameFactory.InProgress(2);
            return New(game);
        }

        public static Turn ThreePlayer()
        {
            var game = GameFactory.InProgress(3);
            return New(game);
        }

        public static Turn FourPlayer()
        {
            var game = GameFactory.InProgress(4);
            return New(game);
        }
    }
}