namespace ToDo.Services.Data
{
    using System.IO;
    using System.Linq;
    using System.Text;
    using Contracts;
    using ToDo.Data.Common.Contracts;
    using ToDo.Data.Models.Account;

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

        public async void Edit(string userId, string fullName, int? age, GenderType gender, string image, string path)
        {
            var dbUser = this.data.All().Where(a => a.Id == userId).Single();

            dbUser.FullName = fullName;
            dbUser.Age = age;
            dbUser.Gender = gender;
            if (dbUser.Background.Value != image)
            {
                using (var fs = new FileStream(path, FileMode.Truncate))
                {
                    using (var sw = new StreamWriter(fs))
                    {
                        await sw.WriteAsync(image);
                    }
                }

                dbUser.Background.Value = path;
            }
            

            this.data.SaveChanges();
        }

        public string GetBackground(string userId)
        {
            var dbUser = this.data.All().Where(a => a.Id == userId).Single();
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
