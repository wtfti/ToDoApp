namespace ToDo.Api.Models.Friend
{
    using Data.Models;
    using Infrastructure.Ninject.Mapping;

    public class FriendResponseMondel: IMapFrom<User>
    {
        public string Username { get; set; }
    }
}