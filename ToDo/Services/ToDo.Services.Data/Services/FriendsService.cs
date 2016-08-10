namespace ToDo.Services.Data.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using Contracts;
    using ToDo.Data.Common.Contracts;
    using ToDo.Data.Models;
    using ToDo.Data.Models.Account;

    public class FriendsService : IFriendsService
    {
        private readonly IRepository<Friend> friendsData;
        private readonly IRepository<User> usersData;

        public FriendsService(IRepository<User> usersRepository, IRepository<Friend> friendsRepository)
        {
            this.friendsData = friendsRepository;
            this.usersData = usersRepository;
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
            var firstUser = this.usersData.All().Single(a => a.UserName == firstUsername);
            var secondUser = this.usersData.All().Single(a => a.UserName == secondUsername);
            var friend =
                this.friendsData.All()
                    .SingleOrDefault(a => a.UserId == firstUser.Id && a.ContactUserId == secondUser.Id) ??
                this.friendsData.All()
                    .SingleOrDefault(a => a.UserId == secondUser.Id && a.ContactUserId == firstUser.Id);

            return friend;
        }

        public IQueryable<User> GetFriendshipsByUserId(string userId)
        {
            var friends = this.friendsData
                .All()
                .Where(a => a.UserId == userId || a.ContactUserId == userId)
                .ToList();
            var allUsers = this.usersData
                .All()
                .Where(u => u.Id != userId)
                .ToList();

            var friendsEnumerable =
               from friend in friends
               from user in allUsers
               where friend.UserId == user.Id || friend.ContactUserId == user.Id
               select user;

            return friendsEnumerable.AsQueryable();
        }

        public IEnumerable<string> PendingFriendRequests(string userId)
        {
            var pendingRequests = this.friendsData
                .All()
                .Where(u => (u.UserId == userId || u.ContactUserId == userId) &&
                            u.Status == Status.Pending).ToList();
            var users = this.usersData.All().Where(u => u.Id != userId).ToList();

            var result =
               from request in pendingRequests
               from user in users
               where request.ContactUserId == user.Id || request.UserId == user.Id
               select user.ProfileDetails.FullName;

            return result;
        }

        public void AddFriendRequest(string firstUsername, string secondUsername)
        {
            var firstUserDb = this.usersData.All().Single(a => a.UserName == firstUsername);
            var secondUserDb = this.usersData.All().Single(a => a.UserName == secondUsername);

            if (firstUserDb.Id != secondUserDb.Id)
            {
                Friend friend = new Friend()
                {
                    UserId = firstUserDb.Id,
                    ContactUserId = secondUserDb.Id,
                    Status = Status.Pending
                };

                this.friendsData.Add(friend);
                this.friendsData.SaveChanges();
            }
        }
    }
}
