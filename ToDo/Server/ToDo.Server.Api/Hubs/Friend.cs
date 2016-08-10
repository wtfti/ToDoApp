namespace ToDo.Api.Hubs
{
    using System.Collections.Concurrent;
    using System.Threading.Tasks;
    using Data.Models.Account;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.SignalR;
    using Services.Data.Contracts;

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
            string recieverConnectionId;

            if (FullNameToConnectionId.TryGetValue(recieverFullName, out recieverConnectionId))
            {
                string sender = this.Context.User.Identity.GetUserName();
                string reciever = FullNameToUsername[recieverFullName];
                var existFriendship = this.friendsService.GetFriendship(sender, reciever);

                if (sender != reciever && existFriendship == null)
                {
                    this.friendsService.AddFriendRequest(sender, reciever);
                    this.Clients.Client(recieverConnectionId).newFriendRequest(UsernameToFullName[sender]);
                }
            }
            else
            {
                var user = this.accountService.GetUserByFullName(recieverFullName);

                if (user != null)
                {
                    string sender = this.Context.User.Identity.GetUserName();
                    string reciever = user.ProfileDetails.FullName;
                    var existFriendship = this.friendsService.GetFriendship(sender, reciever);

                    if (sender != reciever && existFriendship == null)
                    {
                        this.friendsService.AddFriendRequest(sender, reciever);
                    }
                }
            }
        }

        public void AcceptRequest(string senderFullName)
        {
            string currentUser = this.Context.User.Identity.GetUserName();
            string secondUser;
            FullNameToUsername.TryGetValue(senderFullName, out secondUser);

            if (currentUser != null && secondUser != null)
            {
                var request = this.friendsService.GetFriendship(currentUser, secondUser);

                if (request != null && request.Status == Status.Pending)
                {
                    this.friendsService.AcceptRequest(request);

                    string connectionId;

                    if (FullNameToConnectionId.TryGetValue(senderFullName, out connectionId))
                    {
                        this.Clients.Client(connectionId).acceptedRequest(UsernameToFullName[currentUser]);
                    }
                }
            }
            else
            {
                var user = this.accountService.GetUserByFullName(senderFullName);

                if (user != null)
                {
                    var request = this.friendsService.GetFriendship(currentUser, user.UserName);

                    if (request != null && request.Status == Status.Pending)
                    {
                        this.friendsService.AcceptRequest(request);
                    }
                }
            }
        }

        public void DeclineRequest(string senderFullName)
        {
            string firstUser = this.Context.User.Identity.GetUserName();
            string secondUser = FullNameToUsername[senderFullName];

            if (firstUser != null && secondUser != null)
            {
                var request = this.friendsService.GetFriendship(firstUser, secondUser);

                if (request != null && request.Status == Status.Pending)
                {
                    this.friendsService.DeclineRequest(request);
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