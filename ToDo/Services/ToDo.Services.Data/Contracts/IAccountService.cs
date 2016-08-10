namespace ToDo.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Linq;
    using ToDo.Data.Models;
    using ToDo.Data.Models.Account;

    public interface IAccountService
    {
        IQueryable<User> AllUsers();

        ProfileDetails ProfileDetails(string userId);

        void EditAccountSettings(string userId, string fullName, int? age, GenderType gender, string image, string path);

        ICollection<User> GetUsersByUsername(IEnumerable<string> users);

        User GetUserByUsername(string userId);

        User GetUserByFullName(string fullname);

        IEnumerable<string> GetRegistratedUsers(string currentUser);

        string GetBackground(string userId);
    }
}
