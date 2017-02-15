using Newtonsoft.Json;

namespace unobot_main.SlackApi
{
    public class ResponsePayload
    {
        [JsonProperty("text")]
        public string Text { get; set; }
    }
}
