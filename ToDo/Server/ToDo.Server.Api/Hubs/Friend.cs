namespace ToDo.Api.Hubs
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.SignalR;
    using Models.Hubs;
    using Services.Data.Contracts;

    public class Friend : Hub
    {
        private static readonly ConcurrentDictionary<string, string> UsernameToConnectionId =
            new ConcurrentDictionary<string, string>();

        private static readonly IDictionary<string, FriendRequest> FriendRequests =
            new Dictionary<string, FriendRequest>();

        private readonly IAccountService accountService;
        
        public Friend(IAccountService accountService)
        {
            this.accountService = accountService;
        }

        public void FriendRequest(string recieverUsername, string senderFullName)
        {
            string recieverConnectionId;

            if (UsernameToConnectionId.TryGetValue(recieverUsername, out recieverConnectionId))
            {
                string key = Guid.NewGuid().ToString();
                var friendRequest = new FriendRequest()
                {
                    Id = key,
                    From = this.Context.User.Identity.GetUserName(),
                    To = recieverUsername
                };

                var friendshipExist = this.accountService.GetFriendship(recieverUsername);
                if (!FriendRequests.ContainsKey(friendRequest.From) && friendshipExist == null)
                {
                    FriendRequests.Add(friendRequest.From, friendRequest);
                    FriendRequests.Add(friendRequest.To, friendRequest);
                    this.Clients.Client(recieverConnectionId).newFriendRequest(friendRequest.Id, friendRequest.From, senderFullName);
                }
            }
        }

        public void AcceptRequest(string id)
        {
            string currentUser = this.Context.User.Identity.GetUserName();
            FriendRequest request;

            if (FriendRequests.TryGetValue(currentUser, out request) && request.Id == id)
            {
                string senderConnectionId = UsernameToConnectionId[request.From];
                string recieverConnectionId = UsernameToConnectionId[request.To];

                this.accountService.AddFriendship(request.From, request.To);
                FriendRequests.Remove(request.From);
                FriendRequests.Remove(request.To);

                this.Clients.Client(senderConnectionId).acceptedRequest(request.To);
                this.Clients.Client(recieverConnectionId).acceptedRequest(request.From);
            }
        }

        public override Task OnConnected()
        {
            string name = this.Context.User.Identity.GetUserName();

            if (name != null)
            {
                UsernameToConnectionId.TryAdd(name, this.Context.ConnectionId);
            }

            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            string name = this.Context.User.Identity.GetUserName();
            string connectionId;

            UsernameToConnectionId.TryRemove(name, out connectionId);

            return base.OnDisconnected(stopCalled);
        }
    }
}