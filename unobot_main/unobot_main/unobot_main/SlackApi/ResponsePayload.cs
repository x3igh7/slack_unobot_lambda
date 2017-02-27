using Newtonsoft.Json;

namespace unobot_main.SlackApi
{
    public class ResponsePayload
    {
        [JsonProperty("response_type")]
        public string ResponseType { get; set; }

        [JsonProperty("channel")]
        public string Channel { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }
    }
}
