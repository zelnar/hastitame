using Newtonsoft.Json;
using System;

namespace Disco.DomainModel
{
    public class Message
    {
        public string Id { get; set; }
        [JsonProperty(PropertyName = "channel_id")]
        public string ChannelId { get; set; }
        public User Author { get; set; }
        public string Content { get; set; }
        public DateTime TimeStamp { get; set; }
        [JsonProperty(PropertyName = "edited_timestamp")]
        public DateTime? EditedTimestamp { get; set; }
    }
}
