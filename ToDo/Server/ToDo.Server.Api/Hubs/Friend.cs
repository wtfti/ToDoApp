namespace ToDo.Api.Hubs
{
    using System.Collections.Concurrent;
    using System.Threading.Tasks;
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

        public Friend(IAccountService accountService)
        {
            this.accountService = accountService;
        }

        public void FriendRequest(string recieverFullName)
        {
            string recieverConnectionId;

            if (FullNameToConnectionId.TryGetValue(recieverFullName, out recieverConnectionId))
            {
                var friendshipExist = this.accountService.GetFriendship(this.Context.User.Identity.GetUserName(), FullNameToUsername[recieverFullName]);
                if (friendshipExist == null)
                {
                    string sender = this.Context.User.Identity.GetUserName();
                    string reciever = FullNameToUsername[recieverFullName];

                    this.accountService.AddFriendRequest(sender, reciever);
                    this.Clients.Client(recieverConnectionId).newFriendRequest(UsernameToFullName[sender]);
                }
            }
        }

        public void AcceptRequest(string senderFullName)
        {
            string currentUser = this.Context.User.Identity.GetUserName();
            string secondUser = FullNameToUsername[senderFullName];

            if (currentUser != null && secondUser != null)
            {
                var request = this.accountService.GetFriendship(currentUser, secondUser);

                if (request != null)
                {
                    this.accountService.AcceptRequest(request);

                    string connectionId;

                    if (FullNameToConnectionId.TryGetValue(senderFullName, out connectionId))
                    {
                        this.Clients.Client(connectionId).acceptedRequest(UsernameToFullName[currentUser]);
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
                var request = this.accountService.GetFriendship(firstUser, secondUser);

                if (request != null)
                {
                    this.accountService.DeclineRequest(request);
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
            string name = this.Context.User.Identity.GetUserName();
            string connectionId;

            FullNameToConnectionId.TryRemove(name, out connectionId);

            return base.OnDisconnected(stopCalled);
        }
    }
}