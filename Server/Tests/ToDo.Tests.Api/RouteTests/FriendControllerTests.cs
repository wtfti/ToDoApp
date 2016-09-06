namespace ToDo.Tests.Api.RouteTests
{
    using System.Net.Http;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using MyTested.WebApi;
    using ToDo.Api;
    using ToDo.Api.Controllers;

    [TestClass]
    public class FriendControllerTests
    {
        [TestInitialize]
        public void Init()
        {
            MyWebApi.IsRegisteredWith(WebApiConfig.Register)
                .WithBaseAddress("http://baseurl.net");
        }

        [TestMethod]
        public void FriendsRoute()
        {
            MyWebApi
                .Routes()
                .ShouldMap("api/Friend/Friends")
                .WithHttpMethod(HttpMethod.Get)
                .To<FriendController>(a => a.Friends());
        }

        [TestMethod]
        public void PendingFriendRequestsRoute()
        {
            MyWebApi
                .Routes()
                .ShouldMap("api/Friend/PendingFriendRequests")
                .WithHttpMethod(HttpMethod.Get)
                .To<FriendController>(a => a.PendingFriendRequests());
        }
    }
}
