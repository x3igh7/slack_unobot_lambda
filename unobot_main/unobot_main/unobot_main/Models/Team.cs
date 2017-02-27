using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;

namespace unobot_main.Models
{
    [DynamoDBTable("slack_team")]
    public class Team
    {
        public Team(string teamId, string channelId)
        {
            this.TeamId = teamId;
            this.ChannelId = channelId;
            this.CurrentGame = new Game();
            this.GameHistory = new List<Game>();
        }

        [DynamoDBRangeKey]
        public string ChannelId { get; set; }

        public Game CurrentGame { get; set; }
        public List<Game> GameHistory { get; set; }

        [DynamoDBHashKey]
        public string TeamId { get; set; }

        public bool IsGameInProgress()
        {
            return this.CurrentGame != null;
        }

        public static async Task<Team> Load(DynamoDBContext context, string teamId, string channelId)
        {
            return await context.LoadAsync<Team>(teamId, channelId);
        }

        public async void Save(DynamoDBContext context)
        {
            await context.SaveAsync(this);
        }
    }
}