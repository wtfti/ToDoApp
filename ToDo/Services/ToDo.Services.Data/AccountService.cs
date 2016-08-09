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

        public void EditAccountSettings(string userId, string fullName, int? age, GenderType gender, string image, string path)
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

        public User GetUserByUsername(string username)
        {
            return this.usersData.All().Single(a => a.UserName == username);
        }

        public IQueryable<User> GetRegistratedUsers(string currentUserId)
        {
            var friends = this.friendsData.All().Where(a => a.FirstUserId == currentUserId || a.SecondUserId == currentUserId);
            var allUsers = this.usersData.All();
            var exceptUserCollection =
                from friend in friends.AsEnumerable()
                join user in allUsers.AsEnumerable()
                on friend.FirstUserId equals user.Id
                select user;

            var exceptUserCollection2 =
                from friend in friends.AsEnumerable()
                join user in allUsers.AsEnumerable()
                on friend.SecondUserId equals user.Id
                select user;

            var users = this.usersData.All().Where(a => a.Id != currentUserId).Except(exceptUserCollection).Except(exceptUserCollection2);

            return users;
        }

        public void AcceptRequest(Friend request)
        {
            request.Status = Status.Accepted;

            this.friendsData.SaveChanges();
        }

        public void DeclineRequest(Friend request)
        {
            request.Status = Status.Declined;

            this.friendsData.SaveChanges();
        }

        public Friend GetFriendship(string firstUsername, string secondUsername)
        {
            var firstUser = this.GetUserByUsername(firstUsername);
            var secondUser = this.GetUserByUsername(secondUsername);
            var friend =
                this.friendsData.All()
                    .SingleOrDefault(a => a.FirstUserId == firstUser.Id && a.SecondUserId == secondUser.Id) ??
                this.friendsData.All()
                    .SingleOrDefault(a => a.FirstUserId == secondUser.Id && a.SecondUserId == firstUser.Id);

            return friend;
        }

        public void AddFriendRequest(string firstUsername, string secondUsername)
        {
            var firstUserDb = this.GetUserByUsername(firstUsername);
            var secondUserDb = this.GetUserByUsername(secondUsername);

            Friend friend = new Friend()
            {
                FirstUserId = firstUserDb.Id,
                SecondUserId = secondUserDb.Id,
                Status = Status.Pending
            };

            this.friendsData.Add(friend);
            this.friendsData.SaveChanges();
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
