using Sendbird.Entities;
using System;
using System.Collections.Generic;

namespace SendBird.Api.Models
{
    public class ChannelEvent<T> where T: ChannelBase
    {
        public ChannelEvent()
        {
            Members = new List<User>();
        }

        public string Category { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<User> Members { get; set; }
        public User Inviter { get; set; }
        public T Channel { get; set; }

        public string AppId;
    }
}
