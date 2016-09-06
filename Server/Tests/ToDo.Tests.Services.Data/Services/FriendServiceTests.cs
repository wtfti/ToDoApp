namespace ToDo.Tests.Services.Data.Services
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using ToDo.Data.Models;
    using ToDo.Data.Models.Account;
    using ToDo.Services.Data.Services;

    [TestClass]
    public class FriendServiceTests
    {
        [TestMethod]
        public void AcceptRequestShouldPass()
        {
            var friend = new Friend();
            friend.Status = Status.Pending;
            var friendRepository = new InMemoryRepository<Friend>();
            friendRepository.Add(friend);

            var service = new FriendsService(null, friendRepository);

            service.AcceptRequest(friend);
            
            Assert.AreEqual(Status.Accepted, friendRepository.GetById(0).Status);
        }

        [TestMethod]
        public void DeclineRequestShouldPass()
        {
            var friend = new Friend();
            friend.Status = Status.Pending;
            var friendRepository = new InMemoryRepository<Friend>();
            friendRepository.Add(friend);

            var service = new FriendsService(null, friendRepository);

            service.DeclineRequest(friend);

            Assert.AreEqual(Status.Declined, friendRepository.GetById(0).Status);
        }

        [TestMethod]
        public void GetFriendshipWithoutFriendshipShouldPass()
        {
            var friendRepository = new InMemoryRepository<Friend>();
            var userRepository = new InMemoryRepository<User>();
            var service = new FriendsService(userRepository, friendRepository);

            userRepository.Add(new User()
            {
                UserName = "testUser"
            });

            userRepository.Add(new User()
            {
                UserName = "test"
            });


        }
    }
}
