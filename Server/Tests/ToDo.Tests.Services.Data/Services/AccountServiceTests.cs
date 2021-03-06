﻿namespace ToDo.Tests.Services.Data.Services
{
    using System;
    using System.IO;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using ToDo.Data.Models;
    using ToDo.Data.Models.Account;
    using ToDo.Services.Data.Contracts;
    using ToDo.Services.Data.Services;

    [TestClass]
    public class AccountServiceTests
    {
        private InMemoryRepository<ProfileDetails> profileDetailsRepository;
        private InMemoryRepository<User> userRepository;
        private IFriendsService friendsService;
        private IAccountService service;
         
        [TestInitialize]
        public void Initialize()
        {
            this.profileDetailsRepository = DepedencyObjectFactory.GetProfileDetailsRepository();
            this.userRepository = new InMemoryRepository<User>();
            this.friendsService = new FriendsService(null, new InMemoryRepository<Friend>());
            this.service = new AccountService(this.profileDetailsRepository, this.userRepository, this.friendsService);
        }

        [TestMethod]
        public void ProfileDetailsShouldReturnCorrectResult()
        {
            var details = new ProfileDetails()
            {
                Id = "test",
                FullName = "test user",
                Age = 5,
                Gender = GenderType.Male,
            };
            this.profileDetailsRepository.Add(details);

            var result = this.service.ProfileDetails("test");

            Assert.IsNotNull(result);
            Assert.AreEqual("test", result.Id);
            Assert.AreEqual(5, result.Age);
            Assert.AreEqual(GenderType.Male, result.Gender);
            Assert.AreEqual("test user", result.FullName);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ProfileDetailsShouldThrowException()
        {
            var details = new ProfileDetails()
            {
                Id = "test",
                FullName = "test user",
                Age = 5,
                Gender = GenderType.Male,
            };
            this.profileDetailsRepository.Add(details);

            var result = this.service.ProfileDetails("user");
        }

        [TestMethod]
        public void EditShouldPass()
        {
            var details = new ProfileDetails()
            {
                Id = "test",
                FullName = "test user",
                Age = 5,
                Gender = GenderType.Male,
                Background = new Background()
                {
                    Id = "test",
                    Value = "base64Jpg"
                }
            };
            this.profileDetailsRepository.Add(details);

            this.service.EditAccountSettings("test", "changed user", 21, GenderType.None, null, "C:\\");

            Assert.AreEqual(1, this.profileDetailsRepository.SaveChanges());
        }

        [TestMethod]
        public void EditShouldPassOption2()
        {
            var details = new ProfileDetails()
            {
                Id = "test",
                FullName = "test user",
                Age = 5,
                Gender = GenderType.Male,
                Background = new Background()
                {
                    Id = "test",
                    Value = "base64Jpg"
                }
            };
            this.profileDetailsRepository.Add(details);

            this.service.EditAccountSettings("test", "changed user", 21, GenderType.None, "background", "E:\\test.txt");

            Assert.AreEqual(1, this.profileDetailsRepository.SaveChanges());

            string result = string.Empty;
            using (StreamReader sr = new StreamReader("E:\\test.txt"))
            {
                result = sr.ReadToEnd();
            }

            Assert.AreEqual("background", result);
        }

        [TestMethod]
        public void GetBackgroundShouldPass()
        {
            using (StreamWriter sw = new StreamWriter("E:\\test.txt"))
            {
                sw.Write("test");
            }

            var details = new ProfileDetails()
            {
                Id = "test",
                FullName = "test user",
                Age = 5,
                Gender = GenderType.Male,
                Background = new Background()
                {
                    Id = "test",
                    Value = "E:\\test.txt"
                }
            };
            this.profileDetailsRepository.Add(details);

            string result = this.service.GetBackground("test");

            Assert.AreEqual("test", result);

            File.Delete("E:\\test.txt");
        }
    }
}
