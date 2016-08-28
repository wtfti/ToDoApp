namespace ToDo.Api.Hubs
{
    using System.Collections.Concurrent;
    using System.Threading.Tasks;
    using Data.Models.Account;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.SignalR;
    using Services.Data.Contracts;

    [Authorize]
    public class Friend : Hub
    {
        private static readonly ConcurrentDictionary<string, string> FullNameToConnectionId =
            new ConcurrentDictionary<string, string>();
        private static readonly ConcurrentDictionary<string, string> FullNameToUsername =
            new ConcurrentDictionary<string, string>();
        private static readonly ConcurrentDictionary<string, string> UsernameToFullName =
            new ConcurrentDictionary<string, string>();

        private readonly IAccountService accountService;
        private readonly IFriendsService friendsService;

        public Friend(IAccountService accountService, IFriendsService friendsService)
        {
            this.accountService = accountService;
            this.friendsService = friendsService;
        }

        public void FriendRequest(string recieverFullName)
        {
            string senderUsername = this.Context.User.Identity.GetUserName();
            string recieverConnectionId;

            // check if user is connected to server(online)
            if (FullNameToConnectionId.TryGetValue(recieverFullName, out recieverConnectionId))
            {
                string reciever = FullNameToUsername[recieverFullName];
                var existFriendship = this.friendsService.GetFriendship(senderUsername, reciever);

                if (senderUsername != reciever && existFriendship == null)
                {
                    this.friendsService.AddFriendRequest(senderUsername, reciever);
                    this.Clients.Client(recieverConnectionId).newFriendRequest(UsernameToFullName[senderUsername]);
                }
            }
            else
            {
                var recieverUser = this.accountService.GetUserByFullName(recieverFullName);

                if (recieverUser != null)
                {
                    string recieverUsername = recieverUser.UserName;
                    var existFriendship = this.friendsService.GetFriendship(senderUsername, recieverUsername);

                    if (senderUsername != recieverUsername && existFriendship == null)
                    {
                        this.friendsService.AddFriendRequest(senderUsername, recieverUsername);
                    }
                }
            }
        }

        public void AcceptRequest(string senderFullName)
        {
            string currentUsername = this.Context.User.Identity.GetUserName();
            string senderUsername;
            FullNameToUsername.TryGetValue(senderFullName, out senderUsername);

            // check if user is connected to server(online)
            if (currentUsername != null && senderUsername != null)
            {
                var request = this.friendsService.GetFriendship(currentUsername, senderUsername);

                if (request != null && request.Status == Status.Pending)
                {
                    this.friendsService.AcceptRequest(request);

                    string senderConnectionId;
                    string currentUserConnectionId;

                    if (FullNameToConnectionId.TryGetValue(senderFullName, out senderConnectionId))
                    {
                        this.Clients.Client(senderConnectionId).acceptedRequest(UsernameToFullName[currentUsername]);
                    }

                    if (FullNameToConnectionId.TryGetValue(UsernameToFullName[currentUsername], out currentUserConnectionId))
                    {
                        this.Clients.Client(currentUserConnectionId).acceptedRequest(senderFullName);
                    }
                }
            }
            else
            {
                var user = this.accountService.GetUserByFullName(senderFullName);

                if (user != null && currentUsername != null)
                {
                    var request = this.friendsService.GetFriendship(currentUsername, user.UserName);

                    if (request != null && request.Status == Status.Pending)
                    {
                        string currentUserConnectionId;

                        this.friendsService.AcceptRequest(request);

                        if (FullNameToConnectionId.TryGetValue(UsernameToFullName[currentUsername], out currentUserConnectionId))
                        {
                            this.Clients.Client(currentUserConnectionId).acceptedRequest(senderFullName);
                        }
                    }
                }
            }
        }

        public void DeclineRequest(string senderFullName)
        {
            string currentUsername = this.Context.User.Identity.GetUserName();
            string senderUsername;

            // check if user is connected to server(online)
            if (currentUsername != null && UsernameToFullName.TryGetValue(senderFullName, out senderUsername)) 
            {
                var request = this.friendsService.GetFriendship(currentUsername, senderUsername);

                if (request != null && request.Status == Status.Pending)
                {
                    string senderConnectionId = FullNameToConnectionId[UsernameToFullName[senderUsername]];
                    this.friendsService.DeclineRequest(request);
                    this.Clients.Client(senderConnectionId)
                        .declinedRequest(UsernameToFullName[currentUsername]);
                }
            }
            else
            {
                var senderUser = this.accountService.GetUserByFullName(senderFullName);

                if (senderUser != null)
                {
                    senderUsername = senderUser.UserName;
                    var request = this.friendsService.GetFriendship(currentUsername, senderUsername);

                    if (request != null && request.Status == Status.Pending)
                    {
                        this.friendsService.DeclineRequest(request);
                    }
                }
            }
        }

        public override Task OnConnected()
        {
            string username = this.Context.User.Identity.GetUserName();

            if (username != null)
            {
                string fullname = this.accountService.GetUserByUsername(username).ProfileDetails.FullName;
                FullNameToConnectionId.TryAdd(fullname, this.Context.ConnectionId);
                FullNameToUsername.TryAdd(fullname, username);
                UsernameToFullName.TryAdd(username, fullname);
            }

            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            string username = this.Context.User.Identity.GetUserName();
            string fullname;
            UsernameToFullName.TryGetValue(username, out fullname);
            string removed;

            if (fullname == null)
            {
                var user = this.accountService.GetUserByUsername(username);
                fullname = user.ProfileDetails.FullName;
            }

            FullNameToConnectionId.TryRemove(fullname, out removed);
            FullNameToUsername.TryRemove(fullname, out removed);
            UsernameToFullName.TryRemove(username, out removed);

            return base.OnDisconnected(stopCalled);
        }
    }
}