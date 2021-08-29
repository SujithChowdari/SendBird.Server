
using Sendbird.Enums;

namespace SendBird.Api.Models
{
    public class CustomMessage
    {
        public MessageType MessageType { get; set; }
        public string Message { get; set; }
        public string Data { get; set; }
    }
}
