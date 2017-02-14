using System.Collections.Generic;
using System.Linq;
using unobot_main.Models;
using unobot_main.Models.Enums;

namespace unobot_main.Tests.Specs
{
    public static class GameFactory
    {
        public static Game New()
        {
            var game = new Game();
            game.Create();

            return game;
        }

        public static Game InProgress(int numerOfPlayers = 2)
        {
            var deck = new Deck();
            deck.New();

            var players = new List<Player>();
            for (var i = 1; i <= numerOfPlayers; i++)
            {
                players.Add(PlayerFactory.New(i));
            }

            var hands = players.Select(t => HandFactory.New(deck.DealHand(), t)).ToList();

            var discard = new Stack<Card>();
            discard.Push(deck.Draw());

            var game = new Game
            {
                CurrentColor = discard.Peek().Color,
                CurrentValue = discard.Peek().Value,
                Deck = deck,
                Discard = discard,
                Hands = hands,
                Players = players,
                Status = GameStatus.InProgress
            };

            return game;
        }

        //public static Game Completed()
        //{
        //    var game = new Game();
        //    game.Create();

        //    return game;
        //}
    }
}