namespace ToDo.Api.Models.Account
{
    using AutoMapper;
    using Data.Models;
    using Infrastructure.Mapping;
    public class UserResponseModel : IMapFrom<User>, IHaveCustomMappings
    {

        public string FullName { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
        }
    }
}