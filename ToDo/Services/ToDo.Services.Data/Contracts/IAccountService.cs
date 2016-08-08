namespace ToDo.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Linq;
    using ToDo.Data.Models;
    using ToDo.Data.Models.Account;

    public interface IAccountService
    {
        ProfileDetails ProfileDetails(string userId);

        void Edit(string userId, string fullName, int? age, GenderType gender, string image, string path);

        ICollection<User> GetUsersByUsername(IEnumerable<string> users);

        User GetUserByUsername(string userId);

        IQueryable<User> GetRegistratedUsers();

        void AddFriendship(string firstUsername, string secondUsername);

        Friend GetFriendship(string username);

        string GetBackground(string userId);
    }
}
