namespace ToDo.Api.Hubs
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.SignalR;
    using Models.Hubs;

    public class Friend : Hub
    {
        private static readonly ConcurrentDictionary<string, string> UsernameToConnectionId = 
            new ConcurrentDictionary<string, string>();
        private static readonly IDictionary<string, FriendRequest> FriendRequests = 
            new Dictionary<string, FriendRequest>();
        private static readonly IDictionary<string, string> Friendships =
            new Dictionary<string, string>();

        public void FriendRequest(string recieverUsername, string senderFullName)
        {
            string recieverId;

            if (UsernameToConnectionId.TryGetValue(recieverUsername, out recieverId) &&
                !Friendships.ContainsKey(this.Context.User.Identity.GetUserName()))
            {
                string key = Guid.NewGuid().ToString();
                var friendRequest = new FriendRequest()
                {
                    Id = key,
                    From = this.Context.User.Identity.GetUserName(),
                    To = recieverUsername
                };

                
                if (!FriendRequests.ContainsKey(friendRequest.From) && !Friendships.ContainsKey(friendRequest.From))
                {
                    FriendRequests.Add(friendRequest.From, friendRequest);
                    FriendRequests.Add(friendRequest.To, friendRequest);
                    this.Clients.Client(recieverId).newFriendRequest(friendRequest.From, senderFullName);
                }
            }
        }

        public void AcceptRequest(string id)
        {
            string currentUser = this.Context.User.Identity.GetUserName();
            FriendRequest request;

            if (FriendRequests.TryGetValue(currentUser, out request) && request.To == currentUser)
            {
                string senderUsername = UsernameToConnectionId[request.From];
                string recieverUsername = UsernameToConnectionId[request.To];
                Friendships.Add(request.To, request.From);
                Friendships.Add(request.From, request.To);
                FriendRequests.Remove(request.From);
                FriendRequests.Remove(request.To);
                this.Clients.Client(senderUsername).acceptedRequest(request.To);
                this.Clients.Client(recieverUsername).acceptedRequest(request.From);
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