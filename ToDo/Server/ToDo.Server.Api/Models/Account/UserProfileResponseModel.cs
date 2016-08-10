namespace ToDo.Api.Models.Account
{
    using Data.Models.Account;
    using Infrastructure.Ninject.Mapping;

    public class UserProfileResponseModel : IMapFrom<ProfileDetails>
    {
        public string FullName { get; set; }

        public int? Age { get; set; }

        public string Gender { get; set; }

        public string Email { get; set; }
    }
}