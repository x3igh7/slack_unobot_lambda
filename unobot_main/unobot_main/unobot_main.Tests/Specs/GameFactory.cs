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

            return game;
        }

        public static Game InProgress(int numberOfPlayers = 2)
        {
            var game = new Game
            {
                Status = GameStatus.InProgress
            };

            var deck = new Deck(game);
            deck.New();

            game.Deck = deck;

            var players = new List<Player>();
            for (var i = 1; i <= numberOfPlayers; i++)
            {
                players.Add(PlayerFactory.New(i));
            }

            var hands = players.Select(t => HandFactory.New(deck.DealHand(), t)).ToList();

            var discard = new Stack<Card>();
            discard.Push(deck.Draw());

            game.CurrentColor = discard.Peek().Color;
            game.CurrentValue = discard.Peek().Value;
            game.Discard = discard;
            game.Hands = hands;
            game.Players = players;

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