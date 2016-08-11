namespace ToDo.Services.Data.Services
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Contracts;
    using ToDo.Data.Common.Contracts;
    using ToDo.Data.Models;
    using ToDo.Data.Models.Account;

    public class AccountService : IAccountService
    {
        private readonly IRepository<ProfileDetails> profileDetailsData;
        private readonly IRepository<User> usersData;
        private readonly IFriendsService friendsService;

        public AccountService(
            IRepository<ProfileDetails> userProfileRepository,
            IRepository<User> userDataRepository,
            IFriendsService friendsService)
        {
            this.profileDetailsData = userProfileRepository;
            this.usersData = userDataRepository;
            this.friendsService = friendsService;
        }

        public IQueryable<User> AllUsers()
        {
            return this.usersData.All();
        }

        public ProfileDetails ProfileDetails(string userId)
        {
            var user = this.profileDetailsData.All().Single(a => a.Id == userId);

            return user;
        }

        public void EditAccountSettings(string userId, string fullName, int? age, GenderType gender, string image, string path)
        {
            var dbUser = this.profileDetailsData.All().Single(a => a.Id == userId);

            dbUser.FullName = fullName;
            dbUser.Age = age;
            dbUser.Gender = gender;
            if (image != null && dbUser.Background.Value != image)
            {
                using (var fs = new FileStream(path, FileMode.OpenOrCreate))
                {
                    fs.SetLength(0);
                    using (var sw = new StreamWriter(fs))
                    {
                        sw.Write(image);
                    }
                }

                dbUser.Background.Value = path;
            }

            this.profileDetailsData.SaveChanges();
        }

        public IList<User> GetUsersByFullName(IEnumerable<string> users)
        {
            IList<User> result = new List<User>();
            List<User> usersDb = this.usersData.All().ToList();

            foreach (var user in users)
            {
                var currentUserDb = usersDb.FirstOrDefault(a => a.ProfileDetails.FullName == user);
                if (currentUserDb != null)
                {
                    result.Add(currentUserDb);
                }
            }

            return result;
        }

        public User GetUserByUsername(string username)
        {
            return this.usersData.All().Single(a => a.UserName == username);
        }

        public User GetUserByFullName(string fullname)
        {
            return this.usersData.All().SingleOrDefault(user => user.ProfileDetails.FullName == fullname);
        }

        public IEnumerable<string> GetRegistratedUsers(string currentUserId)
        {
            var exceptedUsers = this.friendsService.GetFriendshipsByUserId(currentUserId).ToList();

            var users = this.usersData
                .All()
                .ToList()
                .Where(a => a.Id != currentUserId && exceptedUsers.All(e => e.Id != a.Id))
                .Select(c => c.ProfileDetails.FullName);

            return users;
        }

        public string GetBackground(string userId)
        {
            var dbUser = this.profileDetailsData.All().Single(a => a.Id == userId);
            string background = dbUser.Background.Value;
            var result = new StringBuilder();

            using (StreamReader fs = new StreamReader(background))
            {
                result.Append(fs.ReadToEnd());
            }

            return result.ToString();
        }
    }
}
