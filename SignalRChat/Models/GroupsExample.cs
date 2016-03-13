using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SignalRChat.Models
{
    public class UserContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Room> Rooms { get; set; }
    }

    public class User
    {
        [Key]
        public string Id { get; set; }
        public string UserName { get; set; }
        public string ConnectionId { get; set; }
        public string RoomId { get; set; }
        public string Score { get; set; }
    }

    public class Room
    {
        [Key]
        public int Id { get; set; }
        public int ScoreVisible { get; set; }


    }

}

 