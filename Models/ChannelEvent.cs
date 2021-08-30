using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Sendbird.Entities;
using System;
using System.Collections.Generic;

namespace SendBird.Api.Models
{
    [JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class ChannelEvent<T> 
    {
        public ChannelEvent()
        {
            Members = new List<User>();
        }

        public string Category { get; set; }
        public string CreatedAt { get; set; }
        public string InvitedAt { get; set; }
        public List<User> Members { get; set; }
        public User Inviter { get; set; }
        public List<User> Invitees { get; set; }
        public T Channel { get; set; }
        public string AppId;
    }
}
