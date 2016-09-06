namespace ToDo.Tests.Api.ControllerTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Data.Models;
    using Data.Models.Account;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using MyTested.WebApi;
    using Server.Common.Constants;
    using Services.Data.Contracts;
    using ToDo.Api;
    using ToDo.Api.Controllers;

    [TestClass]
    public class FriendControllerTests
    {
        [TestMethod]
        public void ControllerShouldHaveAuthorizeAttribute()
        {
            var notesService = new Mock<IFriendsService>();

            MyWebApi
                .Controller<FriendController>()
                .WithResolvedDependencyFor(notesService.Object)
                .ShouldHave()
                .Attributes(attr => attr.RestrictingForAuthorizedRequests());
        }

        [TestMethod]
        public void FriendsShouldReturnOk()
        {
            AutoMapperConfig.RegisterMappings(Assembly.Load(AssemblyConstants.WebApi));

            var friendsService = new Mock<IFriendsService>();
            List<User> users = new List<User>();

            for (int i = 0; i < 10; i++)
            {
                users.Add(new User()
                {
                    ProfileDetails = new ProfileDetails()
                    {
                        FullName = "test"
                    }
                });
            }

            friendsService.Setup(a => a.GetFriendshipsByUserId(It.IsAny<string>()))
                .Returns(users.AsQueryable());

            MyWebApi
               .Controller<FriendController>()
               .WithResolvedDependencyFor(friendsService.Object)
               .AndAlso()
               .WithAuthenticatedUser(u => u.WithUsername("test"))
               .Calling(a => a.Friends())
               .ShouldReturn()
               .Ok()
               .WithResponseModelOfType<IEnumerable<string>>();
        }

        [TestMethod]
        public void PendingFriendRequestsShouldReturnOk()
        {
            var friendsService = new Mock<IFriendsService>();
            List<User> users = new List<User>();

            friendsService.Setup(a => a.PendingFriendRequests(It.IsAny<string>()))
                .Returns(new List<string>());

            MyWebApi
               .Controller<FriendController>()
               .WithResolvedDependencyFor(friendsService.Object)
               .Calling(a => a.PendingFriendRequests())
               .ShouldReturn()
               .Ok()
               .WithResponseModelOfType<IEnumerable<string>>();
        }
    }
}
