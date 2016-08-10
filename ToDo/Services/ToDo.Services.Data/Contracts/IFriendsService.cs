namespace ToDo.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Linq;
    using ToDo.Data.Models;
    using ToDo.Data.Models.Account;

    public interface IFriendsService
    {
        void AcceptRequest(Friend request);

        void DeclineRequest(Friend request);

        Friend GetFriendship(string firstUsername, string secondUsername);

        IQueryable<User> GetFriendshipsByUserId(string userId);

        IEnumerable<string> PendingFriendRequests(string userId);

        void AddFriendRequest(string firstUser, string secondUser);
    }
}
