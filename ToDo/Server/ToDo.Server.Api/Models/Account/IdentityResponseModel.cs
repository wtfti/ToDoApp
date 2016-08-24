namespace ToDo.Api.Models.Account
{
    using Data.Models;
    using Infrastructure.Ninject.Mapping;

    public class IdentityResponseModel : IMapFrom<User>
    {
        public string Username { get; set; }

        public string FullName { get; set; }
    }
}