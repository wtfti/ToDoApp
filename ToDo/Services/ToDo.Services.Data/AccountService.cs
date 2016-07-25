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
        private readonly IRepository<ProfileDetails> profileDetailsData;

        public AccountService(IRepository<ProfileDetails> userProfileRepository)
        {
            this.profileDetailsData = userProfileRepository;
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
