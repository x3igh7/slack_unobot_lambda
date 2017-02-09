using Newtonsoft.Json;

namespace unobot_main.SlackApi
{
    public class SlashCommandMessage : SlackMessage
    {
        [JsonProperty("command")]
        public string Command { get; set; }

        [JsonProperty("response_url")]
        public string ResponseUrl { get; set; }
    }
}