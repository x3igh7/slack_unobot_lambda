using Newtonsoft.Json;

namespace unobot_main.SlackApi
{
    public class OutgoingWebookMessage : SlackMessage
    {
        [JsonProperty("timestamp")]
        public string Timestamp { get; set; }

        [JsonProperty("trigger_word")]
        public string TriggerWord { get; set; }
    }
}