namespace ToDo.Api.Controllers
{
    using System.Linq;
    using System.Web.Http;
    using AutoMapper.QueryableExtensions;
    using Microsoft.AspNet.Identity;
    using Models.Friend;
    using Services.Data.Contracts;

    [Authorize]
    public class FriendController : BaseController
    {
        private readonly IFriendsService service;

        public FriendController(IFriendsService friendsService)
        {
            this.service = friendsService;
        }

        [HttpGet]
        public IHttpActionResult Friends()
        {
            var result = this.service
                .GetFriendshipsByUserId(this.User.Identity.GetUserId())
                .ProjectTo<FriendResponseMondel>();

            return this.Ok(result);
        }

        [HttpGet]
        public IHttpActionResult PendingFriendRequests()
        {
            var result = this.service
                .PendingFriendRequests(this.User.Identity.GetUserId());

            return this.Ok(result);
        }
    }
}