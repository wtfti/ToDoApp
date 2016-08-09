namespace ToDo.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Linq;
    using ToDo.Data.Models;
    using ToDo.Data.Models.Account;

    public interface IAccountService
    {
        ProfileDetails ProfileDetails(string userId);

        void EditAccountSettings(string userId, string fullName, int? age, GenderType gender, string image, string path);

        ICollection<User> GetUsersByUsername(IEnumerable<string> users);

        User GetUserByUsername(string userId);

        IQueryable<User> GetRegistratedUsers(string currentUser);

        void AcceptRequest(Friend request);

        void DeclineRequest(Friend request);

        Friend GetFriendship(string firstUsername, string secondUsername);

        void AddFriendRequest(string firstUser, string secondUser);

        string GetBackground(string userId);
    }
}
