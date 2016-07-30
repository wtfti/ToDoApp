namespace ToDo.Tests.Services.Data.Services
{
    using System;
    using System.IO;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using ToDo.Data.Models;
    using ToDo.Data.Models.Account;
    using ToDo.Services.Data;
    using ToDo.Services.Data.Contracts;

    [TestClass]
    public class AccountServiceTests
    {
        private InMemoryRepository<ProfileDetails> profileDetailsRepository;
        private InMemoryRepository<User> userRepository;
        private IAccountService service;
         
        [TestInitialize]
        public void Initialize()
        {
            this.profileDetailsRepository = DepedencyObjectFactory.GetProfileDetailsRepository();
            this.userRepository = new InMemoryRepository<User>();
            this.service = new AccountService(this.profileDetailsRepository, this.userRepository);
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

            this.service.Edit("test", "changed user", 21, GenderType.None, null, "C:\\");

            Assert.AreEqual(1, this.profileDetailsRepository.SaveChanges());
        }

        // Test will create new file test.txt in C:\
        // In last unit test for GetBackground method will be delete it.
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

            this.service.Edit("test", "changed user", 21, GenderType.None, "background", "E:\\test.txt");

            Assert.AreEqual(1, this.profileDetailsRepository.SaveChanges());

            string result = "";
            using (StreamReader sr = new StreamReader("E:\\test.txt"))
            {
                result = sr.ReadToEnd();
            }

            Assert.AreEqual("background", result);
        }

        // Test will throw exception if E:\test.txt is not exist
        // Run test EditShouldPassOption2 to create it or create it manual
        [TestMethod]
        public void GetBackgroundShouldPass()
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
                    Value = "E:\\test.txt"
                }
            };
            this.profileDetailsRepository.Add(details);

            string result = this.service.GetBackground("test");

            Assert.AreEqual("background", result);

            File.Delete("E:\\test.txt");
        }
    }
}
