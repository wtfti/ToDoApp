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
        private readonly IRepository<User> users;

        public AccountService(
            IRepository<ProfileDetails> userProfileRepository,
            IRepository<User> userRepository)
        {
            this.profileDetailsData = userProfileRepository;
            this.users = userRepository;
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

        public void AddFriend(string firstUsername, string secondUsername)
        {
            var firstUser = this.users.All().Single(a => a.UserName == firstUsername);
            var secondUser = this.users.All().Single(a => a.UserName == secondUsername);

            firstUser.Friends.Add(secondUser);
            secondUser.Friends.Add(firstUser);

            this.users.SaveChanges();
        }

        public ICollection<User> GetUsersByUsername(IEnumerable<string> users)
        {
            ICollection<User> result = new List<User>();
            List<User> usersDb = this.users.All().ToList();

            foreach (var user in users)
            {
                var currentUserDb = usersDb.FirstOrDefault(a => a.UserName == user);
                result.Add(currentUserDb);
            }

            return result;
        }

        public User GetUserByUsername(string userId)
        {
            return this.users.All().Single(a => a.Id == userId);
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
