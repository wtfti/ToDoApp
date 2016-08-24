namespace ToDo.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Linq;
    using ToDo.Data.Models;
    using ToDo.Data.Models.Account;

    public interface IAccountService
    {
        ProfileDetails ProfileDetails(string userId);

        IQueryable<User> GetIdentity(string id);

        void EditAccountSettings(string userId, string fullName, int? age, GenderType gender, string image, string path);

        IList<User> GetUsersByFullName(IEnumerable<string> users);

        User GetUserByUsername(string userId);

        User GetUserByFullName(string fullname);

        IEnumerable<string> GetRegistratedUsers(string currentUser);

        string GetBackground(string userId);
    }
}
