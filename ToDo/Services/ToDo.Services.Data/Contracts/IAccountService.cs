namespace ToDo.Services.Data.Contracts
{
    using System.Collections.Generic;
    using ToDo.Data.Models;
    using ToDo.Data.Models.Account;

    public interface IAccountService
    {
        ProfileDetails ProfileDetails(string userId);

        void Edit(string userId, string fullName, int? age, GenderType gender, string image, string path);

        void AddFriend(string firstUsername, string secondUsername);

        ICollection<User> GetUsersByUsername(IEnumerable<string> users);

        User GetUserByUsername(string userId);

        string GetBackground(string userId);
    }
}
