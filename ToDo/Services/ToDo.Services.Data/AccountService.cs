namespace ToDo.Services.Data
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
        private readonly IRepository<Friend> friendsData;

        public AccountService(
            IRepository<ProfileDetails> userProfileRepository,
            IRepository<User> userDataRepository,
            IRepository<Friend> friendRepository)
        {
            this.profileDetailsData = userProfileRepository;
            this.usersData = userDataRepository;
            this.friendsData = friendRepository;
        }

        public ProfileDetails ProfileDetails(string userId)
        {
            var user = this.profileDetailsData.All().Where(a => a.Id == userId).Single();

            return user;
        }

        public void Edit(string userId, string fullName, int? age, GenderType gender, string image, string path)
        {
            var dbUser = this.profileDetailsData.All().Where(a => a.Id == userId).Single();

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

        public ICollection<User> GetUsersByUsername(IEnumerable<string> users)
        {
            ICollection<User> result = new List<User>();
            List<User> usersDb = this.usersData.All().ToList();

            foreach (var user in users)
            {
                var currentUserDb = usersDb.FirstOrDefault(a => a.UserName == user);
                result.Add(currentUserDb);
            }

            return result;
        }

        public User GetUserByUsername(string userId)
        {
            return this.usersData.All().Single(a => a.Id == userId);
        }

        public IQueryable<User> GetRegistratedUsers()
        {
            var users = this.usersData.All();

            return users;
        }

        public void AddFriendship(string firstUsername, string secondUsername)
        {
            var firstUser = this.GetUserByUsername(firstUsername);
            var secondUser = this.GetUserByUsername(secondUsername);

            Friend firstUserToSecondUserFriendship = new Friend()
            {
                FirstUserId = firstUser.Id,
                SecondUserId = secondUser.Id
            };
            
            Friend secondUserToFirstUserFriendship = new Friend()
            {
                FirstUserId = secondUser.Id,
                SecondUserId = firstUser.Id
            };

            this.friendsData.Add(firstUserToSecondUserFriendship);
            this.friendsData.Add(secondUserToFirstUserFriendship);
            this.friendsData.SaveChanges();
        }

        public Friend GetFriendship(string username)
        {
            var user = this.GetUserByUsername(username);
            var friend = this.friendsData.All().SingleOrDefault(a => a.FirstUserId == user.Id);

            return friend;
        }

        public string GetBackground(string userId)
        {
            var dbUser = this.profileDetailsData.All().Where(a => a.Id == userId).Single();
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
