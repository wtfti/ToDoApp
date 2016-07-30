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
        private static ConcurrentDictionary<string, string> UsernameToConnectionId = 
            new ConcurrentDictionary<string, string>();
        private static IDictionary<string, FriendRequest> FriendRequests = 
            new Dictionary<string, FriendRequest>();

        public void FriendRequest(string recieverUsername, string senderFullName)
        {
            string recieverId;

            if (UsernameToConnectionId.TryGetValue(recieverUsername, out recieverId))
            {
                string key = Guid.NewGuid().ToString();
                var friendRequest = new FriendRequest()
                {
                    From = this.Context.User.Identity.GetUserName(),
                    To = recieverUsername
                };

                if (!FriendRequests.ContainsKey(key))
                {
                    FriendRequests.Add(key, friendRequest);
                    this.Clients.Client(recieverId).newFriendRequest(key, senderFullName);
                }
            }
        }

        public void AcceptRequest(string id)
        {
            string currentUser = Context.User.Identity.GetUserName();
            FriendRequest request;

            if (FriendRequests.TryGetValue(id, out request) && request.To == currentUser)
            {
                var senderId = UsernameToConnectionId[request.From];
                // TODO Make friendship between these users lol
                this.Clients.Client(senderId).acceptedRequest();
            }
        }

        public override Task OnConnected()
        {
            string name = Context.User.Identity.GetUserName();

            if (name != null)
            {
                UsernameToConnectionId.TryAdd(name, this.Context.ConnectionId);
            }

            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            string name = Context.User.Identity.GetUserName();
            string connectionId;

            UsernameToConnectionId.TryRemove(name, out connectionId);

            return base.OnDisconnected(stopCalled);
        }
    }
}