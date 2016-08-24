namespace ToDo.Api.Models.Account
{
    using Data.Models.Account;

    public class UserProfileRequestModel
    {
        public string FullName { get; set; }

        public int? Age { get; set; }

        public GenderType Gender { get; set; }

        public string Image { get; set; }
    }
}