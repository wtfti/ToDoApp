namespace ToDo.Tests.Api.ControllerTests
{
    using Data.Models.Account;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using MyTested.WebApi;
    using Server.Common.Constants;
    using ToDo.Api.Controllers;
    using ToDo.Api.Infrastructure.Validation;
    using ToDo.Api.Models.Account;
    using ToDo.Api.Models.Note;

    [TestClass]
    public class AccountControllerTests
    {
        [TestMethod]
        public void DetailsShouldReturnOk()
        {
            MyWebApi
                .Controller<AccountController>()
                .WithResolvedDependencyFor(DependencyObjectFactory.MockAccountService())
                .Calling(a => a.Details())
                .ShouldReturn()
                .Ok()
                .WithResponseModelOfType<UserProfileResponseModel>();
        }

        [TestMethod]
        public void EditShouldReturnOk()
        {
            var model = new UserProfileRequestModel()
            {
                FullName = "Name",
                Age = 5,
                Gender = GenderType.None
            };

            MyWebApi
                .Controller<AccountController>()
                .WithResolvedDependencyFor(DependencyObjectFactory.MockAccountService())
                .Calling(a => a.Edit(model))
                .ShouldHave()
                .ActionAttributes(attr => attr.ContainingAttributeOfType<ValidateModelAttribute>())
                .AndAlso()
                .ShouldReturn()
                .Ok();
        }

        [TestMethod]
        public void EditShouldReturnOkOption2()
        {
            var model = new UserProfileRequestModel()
            {
                FullName = "Name",
                Gender = GenderType.None
            };

            MyWebApi
                .Controller<AccountController>()
                .WithResolvedDependencyFor(DependencyObjectFactory.MockAccountService())
                .Calling(a => a.Edit(model))
                .ShouldHave()
                .ActionAttributes(attr => attr.ContainingAttributeOfType<ValidateModelAttribute>())
                .AndAlso()
                .ShouldReturn()
                .Ok();
        }

        [TestMethod]
        public void EditShouldReturnOkOption3()
        {
            var model = new UserProfileRequestModel()
            {
                FullName = "Name",
                Gender = GenderType.None,
                Image = "base64jpg"
            };

            MyWebApi
                .Controller<AccountController>()
                .WithResolvedDependencyFor(DependencyObjectFactory.MockAccountService())
                .Calling(a => a.Edit(model))
                .ShouldHave()
                .ActionAttributes(attr => attr.ContainingAttributeOfType<ValidateModelAttribute>())
                .AndAlso()
                .ShouldReturn()
                .Ok();
        }

        [TestMethod]
        public void EditShouldReturnInvalidModel()
        {
            var model = new UserProfileRequestModel()
            {
                Gender = GenderType.None
            };

            MyWebApi
                .Controller<AccountController>()
                .WithResolvedDependencyFor(DependencyObjectFactory.MockAccountService())
                .Calling(a => a.Edit(model))
                .ShouldHave()
                .InvalidModelState();
        }

        [TestMethod]
        public void EditShouldReturnInvalidModelOption2()
        {
            var model = new UserProfileRequestModel()
            {
                FullName = "Name"
            };

            MyWebApi
                .Controller<AccountController>()
                .WithResolvedDependencyFor(DependencyObjectFactory.MockAccountService())
                .Calling(a => a.Edit(model))
                .ShouldHave()
                .InvalidModelState();
        }

        [TestMethod]
        public void EditShouldReturnInvalidModelOption3()
        {
            var model = new UserProfileRequestModel()
            {
                Age = 5
            };

            MyWebApi
                .Controller<AccountController>()
                .WithResolvedDependencyFor(DependencyObjectFactory.MockAccountService())
                .Calling(a => a.Edit(model))
                .ShouldHave()
                .InvalidModelState();
        }

        [TestMethod]
        public void BackgroundShouldReturnOk()
        {
            MyWebApi
                .Controller<AccountController>()
                .WithResolvedDependencyFor(DependencyObjectFactory.MockAccountService())
                .Calling(a => a.Background())
                .ShouldReturn()
                .Ok();
        }
    }
}
