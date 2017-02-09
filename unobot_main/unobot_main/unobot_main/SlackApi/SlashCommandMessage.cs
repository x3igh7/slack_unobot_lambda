using Newtonsoft.Json;

namespace unobot_main.SlackApi
{
    public class SlashCommandMessage : SlackMessage
    {
        [JsonProperty("command")]
        public string Command { get; set; }
    }
}