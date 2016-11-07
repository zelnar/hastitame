using Newtonsoft.Json;

namespace Disco.DomainModel
{
    public class GuildChannel
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [JsonProperty(PropertyName = "guild_id")]
        public string GuildId { get; set; }
        [JsonProperty(PropertyName = "is_private")]
        public bool Private { get; set; }
        [JsonProperty(PropertyName = "last_message_id")]
        public string LastMessageId { get; set; }
        [JsonProperty(PropertyName = "user_limit")]
        public int UserLimit { get; set; }

        public string Name { get; set; }
        public string Type { get; set; }
    }
}