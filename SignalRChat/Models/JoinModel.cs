using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRChat.Models
{
    public class JoinModel
    {
        public string RoomId { get; set; }
        public string Name { get; set; }
        public string Id { get; set; }
        public string ConnectionId { get; set; }
        public string Score { get; set; }
    }
}