using unobot_main.Models;

namespace unobot_main.Tests.Specs
{
    public static class PlayerFactory
    {
        public static Player New(int id)
        {
            return new Player
            {
                Name = $"Player{id}"
            };
        }
    }
}