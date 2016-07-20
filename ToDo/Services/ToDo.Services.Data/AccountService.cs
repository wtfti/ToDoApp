namespace ToDo.Services.Data
{
    using System.Linq;
    using Contracts;
    using ToDo.Data.Common.Contracts;
    using ToDo.Data.Models;

    public class AccountService : IAccountService
    {
        private readonly IRepository<ProfileDetails> data;

        public AccountService(IRepository<ProfileDetails> userRepository)
        {
            this.data = userRepository;
        }

        public ProfileDetails ProfileDetails(string userId)
        {
            var user = this.data.All().Where(a => a.Id == userId).Single();

            return user;
        }

        public void Edit(string userId, string fullName, int? age, GenderType gender)
        {
            var dbUser = this.data.All().Where(a => a.Id == userId).Single();

            dbUser.FullName = fullName;
            dbUser.Age = age;
            dbUser.Gender = gender;

            this.data.SaveChanges();
        }
    }
}
