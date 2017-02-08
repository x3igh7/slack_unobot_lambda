using Amazon.Lambda.Core;
using Newtonsoft.Json;
using unobot_main.Models;
using JsonSerializer = Amazon.Lambda.Serialization.Json.JsonSerializer;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.

[assembly: LambdaSerializer(typeof(JsonSerializer))]

namespace unobot_main
{
    public class Main
    {
        /// <summary>
        ///     A simple function that takes a string and does a ToUpper
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string MainHandler(ILambdaContext context)
        {
            var deck = new Deck();
            deck.New();
            return JsonConvert.SerializeObject(deck.Cards);
        }
    }
}