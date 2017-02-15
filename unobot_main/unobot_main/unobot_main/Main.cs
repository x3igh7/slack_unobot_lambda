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
        private readonly string _userName = "UnoBot";
        private readonly string _incomingWebHookUrl = "localhost";

        /// <summary>
        ///     A simple function that takes a string and does a ToUpper
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task<APIGatewayProxyResponse> MainHandler(APIGatewayProxyRequest input, ILambdaContext context)
        {
            var message = this.MapToSlackMessage(input.Body);

            var response = await this.Delagator(message);

            return response;
        }

        public OutgoingWebookMessage MapToSlackMessage(string body)
        {
            this._body = body;
            var order = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(this._body);

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

        private async Task<APIGatewayProxyResponse> Delagator(OutgoingWebookMessage order)
        {

            var response = new APIGatewayProxyResponse
            {
                StatusCode = 200,
                Body = "An error occured and I don't know what to do with myself"
            };

            var commands = order.Text.Split(' ');
            var command = commands[1] ?? string.Empty;

            switch (command)
            {

                case "debug":
                    response.Body = this.CreateDebugBody(order);
                    break;

                case "create":
                    response.Body = await this.CreateDeck(order);
                    break;

            }

            return response;
        }

        public async Task<string> CreateDeck(OutgoingWebookMessage message)
        {

            var deck = new Deck();
            deck.New();
            var payload = new 
            {
                text = JsonConvert.SerializeObject(deck.Cards)
            };

            if (message.TriggerWord.Equals("debug"))
            {
                return JsonConvert.SerializeObject(payload);
            }

            //using (var client = new HttpClient())
            //{
            //    await client.PostAsync(this._incomingWebHookUrl, new StringContent(JsonConvert.SerializeObject(payload)));
            //}

            return JsonConvert.SerializeObject(payload);
        }

        private string CreateDebugBody(SlackMessage order)
        {
            var payload = new Payload
            {
                Channel = order.ChannelId,
                Username = this._userName,
                Text = JsonConvert.SerializeObject(this._body)
            };

            return JsonConvert.SerializeObject(payload);
        }
    }
}