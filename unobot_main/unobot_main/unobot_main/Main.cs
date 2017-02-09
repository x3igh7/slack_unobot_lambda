using System.Net.Http;
using System.Threading.Tasks;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
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
        private string _body;

        /// <summary>
        ///     A simple function that takes a string and does a ToUpper
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task<APIGatewayProxyResponse> MainHandler(APIGatewayProxyRequest input, ILambdaContext context)
        {
            var message = MapToSlackMessage(input.Body);

            return response;
        }

        public SlackMessage MapToSlackMessage(string body)
        {
            _body = body;
            var order = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(_body);

            var slackMessage = new SlackMessage
            {
                Token = order["token"],
                TeamId = order["team_id"],
                TeamDomain = order["team_domain"],
                ChannelId = order["channel_id"],
                ChannelName = order["channel_name"],
                UserId = order["user_id"],
                Username = order["user_name"],
                Text = order["text"],
                ResponseUrl = order["response_url"]
            };

            return slackMessage;
        }

        private async Task<APIGatewayProxyResponse> Delagator(SlackMessage order)
        {

            var response = new APIGatewayProxyResponse
            {
                StatusCode = 200,
                Body = "An error occured and I don't know what to do with myself"
            };

            var commands = order.Text.ToString().Split(' ');
            var command = commands[0] ?? string.Empty;

            switch (command)
            {

                case "debug":
                    response.Body = await CreateDebugBody(order);
                    break;

            }

            return response;
        }

        public async Task CreateDeck(SlackMessage message)
        {

            var deck = new Deck();
            deck.New();
            var payload = new Payload
            {
                Channel = message.ChannelId,
                Username = "UnoBot",
                Text = JsonConvert.SerializeObject(deck.Cards)
            };

            using (var client = new HttpClient())
            {
                await client.PostAsync(message.ResponseUrl, new StringContent(JsonConvert.SerializeObject(payload)));
            }
        }

        private async Task<string> CreateDebugBody(SlackMessage order)
        {
            var payload = new Payload
            {
                Channel = order.ChannelId,
                Username = "Hi-Command",
                Text = JsonConvert.SerializeObject(_body)
            };

            return JsonConvert.SerializeObject(payload);
        }
    }
}