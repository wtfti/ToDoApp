namespace ToDo.Tests.Api.RouteTests
{
    using System.Net.Http;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using MyTested.WebApi;
    using ToDo.Api;
    using ToDo.Api.Controllers;

    [TestClass]
    public class AccountControllerTests
    {
        [TestInitialize]
        public void Init()
        {
            MyWebApi.IsRegisteredWith(WebApiConfig.Register)
                .WithBaseAddress("http://baseurl.net");
        }

        [TestMethod]
        public void DetailsRoute()
        {
            MyWebApi
                .Routes()
                .ShouldMap("api/Account/Details")
                .WithHttpMethod(HttpMethod.Get)
                .To<AccountController>(a => a.Details());
        }

        [TestMethod]
        public void EditRoute()
        {
            MyWebApi
                .Routes()
                .ShouldMap("api/Account/Edit")
                .WithHttpMethod(HttpMethod.Put)
                .WithJsonContent(@"{""FullName"": ""name"", ""Age"": 5, ""Gender"": ""Male"" ""Image"": ""base64Image""}")
                .To<AccountController>(a => a.Edit(null));
        }

        [TestMethod]
        public void BackgroundRoute()
        {
            MyWebApi
                .Routes()
                .ShouldMap("api/Account/Background")
                .WithHttpMethod(HttpMethod.Get)
                .To<AccountController>(a => a.Background());
        }

        [TestMethod]
        public void IdentityRoute()
        {
            MyWebApi
                .Routes()
                .ShouldMap("api/Account/Identity")
                .WithHttpMethod(HttpMethod.Get)
                .To<AccountController>(a => a.Identity());
        }
    }
}
