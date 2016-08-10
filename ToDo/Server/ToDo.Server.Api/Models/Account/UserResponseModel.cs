namespace ToDo.Api.Models.Account
{
    using Data.Models;
    using Infrastructure.Ninject.Mapping;

    public class UserResponseModel : IMapFrom<User>
    {
        public string FullName { get; set; }
    }
}