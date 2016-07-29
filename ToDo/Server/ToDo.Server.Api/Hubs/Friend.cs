namespace ToDo.Api.Hubs
{
    using System.Collections.Concurrent;
    using System.Threading.Tasks;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.SignalR;

    public class Friend : Hub
    {
        private static ConcurrentDictionary<string, string> users = 
            new ConcurrentDictionary<string, string>();

        public void FriendRequest(string recieverFullName)
        {
            string recieverId;

            if (users.TryGetValue(recieverFullName, out recieverId))
            {
                this.Clients.Client(recieverId).newFriendRequest(recieverFullName);
            }
        }

        public void AcceptRequest(string senderFullName)
        {
            string recieverId;

            if (users.TryGetValue(senderFullName, out recieverId))
            {
                this.Clients.Client(recieverId).acceptedRequest();
            }
        }

        public override Task OnConnected()
        {
            string name = Context.User.Identity.GetUserName();

            if (name != null)
            {
                users.TryAdd(name, this.Context.ConnectionId);
            }

            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            string name = Context.User.Identity.GetUserName();
            string connectionId;

            users.TryRemove(name, out connectionId);

            return base.OnDisconnected(stopCalled);
        }

        public override Task OnReconnected()
        {
            string name = this.Context.User.Identity.Name;

            if (!users[name].Contains(this.Context.ConnectionId))
            {
                users.TryAdd(name, this.Context.ConnectionId);
            }

            return base.OnReconnected();
        }
    }
}