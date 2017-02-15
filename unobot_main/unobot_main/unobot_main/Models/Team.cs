using System.Collections.Generic;
using Amazon.DynamoDBv2.DataModel;

namespace unobot_main.Models
{
    [DynamoDBTable("Team")]
    public class Team
    {
        [DynamoDBHashKey]
        public string TeamId { get; set; }
        [DynamoDBRangeKey]
        public string ChannelId { get; set; }
        public Game CurrentGame { get; set; }
        public List<Game> GameHistory { get; set; }
    }
}