using System;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using unobot_main.Models;
using unobot_main.SlackApi;
using JsonSerializer = Amazon.Lambda.Serialization.Json.JsonSerializer;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.

[assembly: LambdaSerializer(typeof(JsonSerializer))]

namespace unobot_main
{
    public class Main
    {
        private readonly string _incomingWebHookUrl = "localhost";
        private readonly string _userName = "UnoBot";
        private string _body;
        private AmazonDynamoDBClient _client;
        private DynamoDBContext _context;

        /// <summary>
        ///     A simple function that takes a string and does a ToUpper
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task<APIGatewayProxyResponse> MainHandler(APIGatewayProxyRequest input, ILambdaContext context)
        {
            this._client = new AmazonDynamoDBClient();
            this._context = new DynamoDBContext(this._client);

            var message = this.MapToSlackMessage(input.Body);

            var response = await this.Delagator(message);

            return response;
        }

        public OutgoingWebookMessage MapToSlackMessage(string body)
        {
            this._body = body;
            var order = QueryHelpers.ParseQuery(this._body);

            var slackMessage = new OutgoingWebookMessage
            {
                Token = order["token"],
                TeamId = order["team_id"],
                TeamDomain = order["team_domain"],
                ChannelId = order["channel_id"],
                ChannelName = order["channel_name"],
                UserId = order["user_id"],
                Username = order["user_name"],
                Text = order["text"],
                Timestamp = order["timestamp"],
                TriggerWord = order["trigger_word"]
            };

            return slackMessage;
        }

        private string CreateDebugBody(SlackMessage order)
        {
            var payload = new ResponsePayload
            {
                Text = JsonConvert.SerializeObject(this._body)
            };

            return JsonConvert.SerializeObject(payload);
        }

        private async Task<APIGatewayProxyResponse> Delagator(OutgoingWebookMessage order)
        {
            var response = new APIGatewayProxyResponse
            {
                StatusCode = 200,
                Body = "An error occured and I don't know what to do with myself"
            };

            var commands = order.Text.Split(' ');
            var command = commands[1] ?? string.Empty;

            Team team = null;
            if (string.IsNullOrEmpty(command))
            {
                team = await Team.Load(this._context, order.TeamId, order.ChannelId);
            }

            switch (command)
            {
                case "debug":
                    response.Body = this.CreateDebugBody(order);
                    break;

                case "create":
                    if (team == null)
                    {
                        team = new Team(order.TeamId, order.ChannelId);
                    }
                    else
                    {
                        if (team.CurrentGame == null)
                        {
                            team.CurrentGame = new Game();
                        }
                        else
                        {
                            response.Body = "Game already in progress!";
                        }
                    }

                    team.Save(this._context);
                    response.Body = "Game created! Waiting for players to join...";
                    break;
                case "join":
                    if (team == null)
                    {
                        response.Body = "No game in progress. Create a game first.";
                        break;
                    }
                    
                    var player = new Player
                    {
                        Id = order.UserId,
                        Name = order.Username
                    };

                    if (team.CurrentGame.AddPlayer(player))
                    {
                        response.Body = $"{player.Name} joined the game! There are now {team.CurrentGame.Players.Count} / 4 players";
                    }

                    team.Save(this._context);
                    break;
            }

            return response;
        }
    }
}