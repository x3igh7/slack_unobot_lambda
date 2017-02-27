using Newtonsoft.Json;

namespace unobot_main.SlackApi
{
    public class SlashCommandResponsePaylod : ResponsePayload
    {
        [JsonProperty("channel")]
        public string Channel { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }
    }
}