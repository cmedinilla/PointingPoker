using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using SignalRChat.Models;
using System.Web;
namespace SignalRChat
{
    [HubName("chatHub")]
    public class ChatHub : Hub
    {
        public static Dictionary<string,bool> RoomLevelData = new Dictionary<string, bool>();
        public void Send(string name, string message)
        {
            // Call the addNewMessageToPage method to update clients.
            Clients.All.addNewMessageToPage(name, message);
        }

        public override Task OnConnected()
        {
            return base.OnConnected();
        }

        public override Task OnDisconnected()
        {
            using (var db = new UserContext())
            {
               var user= db.Users.Where(x => x.ConnectionId == Context.ConnectionId).FirstOrDefault();
                if (user!=null)
                {
                    var roomId = user.RoomId;
                    db.Users.Remove(user);
                    db.SaveChanges();
                    Clients.Group(roomId).removeClient(Context.ConnectionId);
                }
            }
            return base.OnDisconnected();
        }

        public void ClearVotes()
        {
            setRoomScoreVisiblity(false);
            using (var db = new UserContext())
            {
                var userInRoom = db.Users.Where(x => x.ConnectionId == Context.ConnectionId);
                if (userInRoom.Any())
                {
                    foreach (var item in userInRoom)
                    {
                        item.Score = "0";
                    }
                    db.SaveChanges();
                    var user = userInRoom.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
                    if (user != null)
                    {
                        var roomId = user.RoomId;
                        Clients.Group(roomId).clearVotes(Context.ConnectionId);
                    } 
                }
            }
        }
        public void ShowVotes()
        {
            setRoomScoreVisiblity(true);
            using (var db = new UserContext())
            {
                var user = db.Users.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
                if (user != null)
                {
                    var roomId = user.RoomId;
                    Clients.Group(roomId).showVotes(Context.ConnectionId);
                }
            }
        }

        public void addMyVote(string value)
        {
            using (var db = new UserContext())
            {
                var user = db.Users.Where(x => x.ConnectionId == Context.ConnectionId).FirstOrDefault();
                if (user != null)
                {
                    var roomId = user.RoomId;
                    user.Score = value;
                    db.SaveChanges();
                    Clients.Group(roomId).updateVotes(Context.ConnectionId,value);
                }
            }
        }
        public void AddToRoom(string name)
        {
            string roomId = Context.QueryString["roomId"];
            string id = Context.QueryString["id"];
            using (var db = new UserContext())
            {
                var user = new User()
                {
                    Id = id,
                    ConnectionId = Context.ConnectionId,
                    UserName = name ,
                    RoomId = roomId,
                    Score = "0"
                };

                db.Users.Add(user);
                db.SaveChanges();

                Groups.Add(Context.ConnectionId, roomId);

                Clients.Group(roomId,new string[] { Context.ConnectionId }).joinNewClient(new List<JoinModel>()
                {
                    new JoinModel()
                    {
                        Name = name,
                        RoomId = roomId,
                        Id = id,
                        ConnectionId=Context.ConnectionId,
                        Score = "0"
                    }
                }, getRoomScoreVisible(roomId));
                Task.Run(() =>
                {
                    var allOtherClients = getGroupUsers(roomId);
                    Clients.Client(Context.ConnectionId).joinNewClient(allOtherClients, getRoomScoreVisible(roomId));
                });
            }
        }
        

        private List<JoinModel> getGroupUsers(string roomId)
        {
            List<JoinModel> groupMembser = new List<JoinModel>();
            using (var db = new UserContext())
            {

                var UsersListInRoom = db.Users.Where(x=>x.RoomId==roomId);

                foreach (var item in UsersListInRoom)
                {
                    groupMembser.Add(new JoinModel()
                    {
                        Name = item.UserName,
                        RoomId = roomId,
                        Id = item.Id,
                        ConnectionId = item.ConnectionId,
                        Score = item.Score
                    });
                }
            }
            return groupMembser;
        }

        private bool getRoomScoreVisible(string roomId)
        {
            if (!RoomLevelData.ContainsKey(roomId))
            {

                RoomLevelData[roomId] = false;
                return false;
            }

           return RoomLevelData[roomId];
        }


        private void setRoomScoreVisiblity(bool visible)
        {
            RoomLevelData["roomId"] = visible;
        }
    }
}