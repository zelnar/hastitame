using Disco.DomainModel;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Disco
{
    public class Discord
    {
        public static string GuildId;
        public static string ClientId { get; set; }
        public static string BotToken { get; set; }

        /// <summary>
        /// Configures Discord
        /// </summary>
        /// <param name="guildId">Discord server (guild) Id</param>
        /// <param name="clientId">App client Id</param>
        /// <param name="botToken">Bot auth token</param>
        public Discord(string guildId, string clientId, string botToken)
        {
            GuildId = guildId;
            ClientId = clientId;
            BotToken = botToken;
        }

        /// <summary>
        /// Gets all the discord channels on the server
        /// </summary>
        /// <returns>List of GuildChannel</returns>
        public async Task<List<GuildChannel>> ListChannelsAsync()
        {
            using (HttpService service = new HttpService())
            {
                return await service.GetAsync<List<GuildChannel>>($"guilds/{GuildId}/channels");
            }
        }

        /// <summary>
        /// Gets channel via friendly name
        /// </summary>
        /// <param name="channelName"></param>
        /// <returns></returns>
        public async Task<GuildChannel> GetChannelByName(string channelName)
        {
            var channels = await ListChannelsAsync();
            var theChannel = channels.SingleOrDefault(c => c.Name == channelName);

            using (HttpService service = new HttpService())
            {
                return await service.GetAsync<GuildChannel>($"channels/{theChannel.Id}");
            }
        }

        /// <summary>
        /// Gets channel via Id
        /// </summary>
        /// <param name="channelId"></param>
        /// <returns></returns>
        public async Task<GuildChannel> GetChannelById(string channelId)
        {
            var channels = await ListChannelsAsync();
            var theChannel = channels.SingleOrDefault(c => c.Id == channelId);

            using (HttpService service = new HttpService())
            {
                return await service.GetAsync<GuildChannel>($"channels/{theChannel.Id}");
            }
        }

        /// <summary>
        /// Gets the messages for a channel via channel Id
        /// </summary>
        /// <param name="channelId">The Id of the Channel</param>
        /// <returns>List of messages in channel</returns>
        public async Task<List<Message>> GetMessagesForChannelById(string channelId)
        {
            using (HttpService service = new HttpService())
            {
                return await service.GetAsync<List<Message>>($"channels/{channelId}/messages");
            }
        }

        /// <summary>
        /// Gets the messages for a channel by name
        /// </summary>
        /// <param name="channelName">Friendly channel name</param>
        /// <returns>List of messages in channel</returns>
        public async Task<List<Message>> GetMessagesForChannelByName(string channelName)
        {
            var channels = await ListChannelsAsync();

            var theChannel = channels.SingleOrDefault(c => c.Name == channelName);

            using (HttpService service = new HttpService())
            {
                return await service.GetAsync<List<Message>>($"channels/{theChannel.Id}/messages");
            }
        }


        /// <summary>
        /// Posts a message to a discord channel
        /// </summary>
        /// <param name="channelId"></param>
        /// <param name="message"></param>
        public void PostMessage(string channelId, string message)
        {
            var msg = new { content = message };

            using (HttpService service = new HttpService())
            {
                service.PostAsync($"channels/{channelId}/messages", msg);
            }
        }

        /// <summary>
        /// Generates the URL needed to authorize your bot for the first time.
        /// </summary>
        /// <returns></returns>
        public string GenerateAddBotUrl()
        {
            return HttpService.BaseUrl + $"oauth2/authorize?client_id={ClientId}&scope=bot&permissions=0";
        }
    }
}