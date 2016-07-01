namespace ToDo.Tests.Api.ControllerTests
{
    using System;
    using System.Linq;
    using System.Net.Http;
    using System.Reflection;
    using Data.Models;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using MyTested.WebApi;
    using Server.Common.Constants;
    using ToDo.Api;
    using ToDo.Api.Controllers;
    using ToDo.Api.Models.Note;
    using ToDo.Api.Infrastructure.Validation;

    [TestClass]
    public class NoteControllerTests
    {
        [TestInitialize]
        public void Init()
        {
            AutoMapperConfig.RegisterMappings(Assembly.Load(AssemblyConstants.WebApi));
        }

        [TestMethod]
        public void ControllerAuthorizeAttributeShouldHave()
        {
            MyWebApi
                .Controller<NoteController>()
                .WithResolvedDependencyFor(DependencyObjectFactory.GetNotesService())
                .ShouldHave()
                .Attributes(attr => attr.RestrictingForAuthorizedRequests());
        }

        [TestMethod]
        public void AddNoteShouldReturnProperResponse()
        {
            MyWebApi
                .Controller<NoteController>()
                .WithResolvedDependencyFor(DependencyObjectFactory.GetNotesService())
                .Calling(a => a.AddNote(new NoteRequestModel()))
                .ShouldReturn()
                .Ok()
                .WithResponseModel(MessageConstants.CreateNoteMessage);
        }

        [TestMethod]
        public void AddNoteShouldHaveValidateModel()
        {
            MyWebApi
                .Controller<NoteController>()
                .WithResolvedDependencyFor(DependencyObjectFactory.GetNotesService())
                .Calling(a => a.AddNote(new NoteRequestModel()))
                .ShouldHave()
                .ActionAttributes(a => a.ContainingAttributeOfType<ValidateModelAttribute>());
        }

        [TestMethod]
        public void AddNoteWithExpirationDateShouldReturnProperResponse()
        {
            MyWebApi
                .Controller<NoteController>()
                .WithResolvedDependencyFor(DependencyObjectFactory.GetNotesService())
                .Calling(a => a.AddNoteWithExpirationDate(new NoteRequestModel()))
                .ShouldReturn()
                .Ok()
                .WithResponseModel(MessageConstants.CreateNoteMessage);
        }

        [TestMethod]
        public void AddNoteWithExpirationDateShouldHaveValidateModel()
        {
            MyWebApi
                .Controller<NoteController>()
                .WithResolvedDependencyFor(DependencyObjectFactory.GetNotesService())
                .Calling(a => a.AddNoteWithExpirationDate(new NoteRequestModel()))
                .ShouldHave()
                .ActionAttributes(a => a.ContainingAttributeOfType<ValidateModelAttribute>());
        }

        [TestMethod]
        public void GetNotesShouldReturnProperResponse()
        {
            MyWebApi
                .Controller<NoteController>()
                .WithResolvedDependencyFor(DependencyObjectFactory.GetNotesService())
                .Calling(a => a.GetNotes(1))
                .ShouldReturn()
                .Ok()
                .WithResponseModelOfType<IQueryable<NoteResponseModel>>();
        }

        [TestMethod]
        public void GetNotesWithExpirateDateShouldReturnProperResponse()
        {
            MyWebApi
                .Controller<NoteController>()
                .WithResolvedDependencyFor(DependencyObjectFactory.GetNotesService())
                .Calling(a => a.GetNotesWithExpirateDate(1))
                .ShouldReturn()
                .Ok()
                .WithResponseModelOfType<IQueryable<NoteResponseModel>>();
        }

        [TestMethod]
        public void RemoveNoteByIdWithInvalidIdShouldReturnBadRequest()
        {
            MyWebApi
                .Controller<NoteController>()
                .WithResolvedDependencyFor(DependencyObjectFactory.GetNotesService())
                .WithAuthenticatedUser()
                .Calling(q => q.RemoveNoteById(2))
                .ShouldReturn()
                .BadRequest()
                .WithErrorMessage(MessageConstants.NoteDoesNotExistsMessage);
        }

        [TestMethod]
        public void RemoveNoteByIdWithNoneExistingNoteForCurrentUserShouldReturnBadRequest()
        {
            MyWebApi
                .Controller<NoteController>()
                .WithResolvedDependencyFor(DependencyObjectFactory.GetNotesService())
                .WithAuthenticatedUser()
                .Calling(q => q.RemoveNoteById(1))
                .ShouldReturn()
                .BadRequest()
                .WithErrorMessage(MessageConstants.NoteDoesNotExistsMessage);
        }

        [TestMethod]
        public void RemoveNoteByIdShouldReturnProperResponse()
        {
            MyWebApi
                .Controller<NoteController>()
                .WithResolvedDependencyFor(DependencyObjectFactory.GetNotesService())
                .WithAuthenticatedUser(a => a.WithUsername("User"))
                .Calling(q => q.RemoveNoteById(1))
                .ShouldReturn()
                .Ok()
                .WithResponseModel(MessageConstants.RemoveNoteMessage);
        }

        [TestMethod]
        public void ChangeNoteTitleWithInvalidIdShouldReturnBadRequest()
        {
            MyWebApi
                .Controller<NoteController>()
                .WithResolvedDependencyFor(DependencyObjectFactory.GetNotesService())
                .WithAuthenticatedUser()
                .Calling(q => q.ChangeNoteTitle(2, "str"))
                .ShouldReturn()
                .BadRequest()
                .WithErrorMessage(MessageConstants.NoteDoesNotExistsMessage);
        }

        [TestMethod]
        public void ChangeNoteTitleWithNoneExistingNoteForCurrentUserShouldReturnBadRequest()
        {
            MyWebApi
                .Controller<NoteController>()
                .WithResolvedDependencyFor(DependencyObjectFactory.GetNotesService())
                .WithAuthenticatedUser()
                .Calling(q => q.ChangeNoteTitle(1, "str"))
                .ShouldReturn()
                .BadRequest()
                .WithErrorMessage(MessageConstants.NoteDoesNotExistsMessage);
        }

        [TestMethod]
        public void ChangeNoteTitleShouldReturnProperResponse()
        {
            MyWebApi
                .Controller<NoteController>()
                .WithResolvedDependencyFor(DependencyObjectFactory.GetNotesService())
                .WithAuthenticatedUser(a => a.WithUsername("User"))
                .Calling(q => q.ChangeNoteTitle(1, "str"))
                .ShouldReturn()
                .Ok()
                .WithResponseModel(MessageConstants.TitleChangeMessage);
        }

        [TestMethod]
        public void ChangeNoteContentWithInvalidIdShouldReturnBadRequest()
        {
            MyWebApi
                .Controller<NoteController>()
                .WithResolvedDependencyFor(DependencyObjectFactory.GetNotesService())
                .WithAuthenticatedUser()
                .Calling(q => q.ChangeNoteContent(2, "str"))
                .ShouldReturn()
                .BadRequest()
                .WithErrorMessage(MessageConstants.NoteDoesNotExistsMessage);
        }

        [TestMethod]
        public void ChangeNoteContentWithNoneExistingNoteForCurrentUserShouldReturnBadRequest()
        {
            MyWebApi
                .Controller<NoteController>()
                .WithResolvedDependencyFor(DependencyObjectFactory.GetNotesService())
                .WithAuthenticatedUser()
                .Calling(q => q.ChangeNoteContent(1, "str"))
                .ShouldReturn()
                .BadRequest()
                .WithErrorMessage(MessageConstants.NoteDoesNotExistsMessage);
        }

        [TestMethod]
        public void ChangeNoteContentShouldReturnProperResponse()
        {
            MyWebApi
                .Controller<NoteController>()
                .WithResolvedDependencyFor(DependencyObjectFactory.GetNotesService())
                .WithAuthenticatedUser(a => a.WithUsername("User"))
                .Calling(q => q.ChangeNoteContent(1, "str"))
                .ShouldReturn()
                .Ok()
                .WithResponseModel(MessageConstants.ContextChangeMessage);
        }

        [TestMethod]
        public void SetNoteExpireDateWithInvalidIdShouldReturnBadRequest()
        {
            MyWebApi
                .Controller<NoteController>()
                .WithResolvedDependencyFor(DependencyObjectFactory.GetNotesService())
                .WithAuthenticatedUser()
                .Calling(q => q.SetNoteExpireDate(2, "str"))
                .ShouldReturn()
                .BadRequest()
                .WithErrorMessage(MessageConstants.NoteDoesNotExistsMessage);
        }

        [TestMethod]
        public void SetNoteExpireDateWithNoneExistingNoteForCurrentUserShouldReturnBadRequest()
        {
            MyWebApi
                .Controller<NoteController>()
                .WithResolvedDependencyFor(DependencyObjectFactory.GetNotesService())
                .WithAuthenticatedUser()
                .Calling(q => q.SetNoteExpireDate(1, "str"))
                .ShouldReturn()
                .BadRequest()
                .WithErrorMessage(MessageConstants.NoteDoesNotExistsMessage);
        }

        [TestMethod]
        public void SetNoteExpireDateWithWrongDateFormatShouldReturnBadRequest()
        {
            MyWebApi
                .Controller<NoteController>()
                .WithResolvedDependencyFor(DependencyObjectFactory.GetNotesService())
                .WithAuthenticatedUser(a => a.WithUsername("User"))
                .Calling(q => q.SetNoteExpireDate(1, "01.02.2016"))
                .ShouldReturn()
                .BadRequest()
                .WithErrorMessage(MessageConstants.InvalidDateMessage);
        }

        [TestMethod]
        public void SetNoteExpireDateShouldReturnProperResponse()
        {
            MyWebApi
                .Controller<NoteController>()
                .WithResolvedDependencyFor(DependencyObjectFactory.GetNotesService())
                .WithAuthenticatedUser(a => a.WithUsername("User"))
                .Calling(q => q.SetNoteExpireDate(1, "01-06-2016"))
                .ShouldReturn()
                .Ok()
                .WithResponseModel(MessageConstants.SetDateMessage);
        }

        [TestMethod]
        public void ChangeExpireDateWithInvalidIdShouldReturnBadRequest()
        {
            MyWebApi
                .Controller<NoteController>()
                .WithResolvedDependencyFor(DependencyObjectFactory.GetNotesService())
                .WithAuthenticatedUser()
                .Calling(q => q.ChangeExpireDate(2, "str"))
                .ShouldReturn()
                .BadRequest()
                .WithErrorMessage(MessageConstants.NoteDoesNotExistsMessage);
        }

        [TestMethod]
        public void ChangeExpireDateWithNoneExistingNoteForCurrentUserShouldReturnBadRequest()
        {
            MyWebApi
                .Controller<NoteController>()
                .WithResolvedDependencyFor(DependencyObjectFactory.GetNotesService())
                .WithAuthenticatedUser()
                .Calling(q => q.ChangeExpireDate(1, "str"))
                .ShouldReturn()
                .BadRequest()
                .WithErrorMessage(MessageConstants.NoteDoesNotExistsMessage);
        }

        [TestMethod]
        public void ChangeExpireDateWithWrongDateFormatShouldReturnBadRequest()
        {
            MyWebApi
                .Controller<NoteController>()
                .WithResolvedDependencyFor(DependencyObjectFactory.GetNotesService())
                .WithAuthenticatedUser(a => a.WithUsername("User"))
                .Calling(q => q.ChangeExpireDate(1, "01.02.2016"))
                .ShouldReturn()
                .BadRequest()
                .WithErrorMessage(MessageConstants.InvalidDateMessage);
        }

        [TestMethod]
        public void ChangeExpireDateShouldReturnProperResponse()
        {
            MyWebApi
                .Controller<NoteController>()
                .WithResolvedDependencyFor(DependencyObjectFactory.GetNotesService())
                .WithAuthenticatedUser(a => a.WithUsername("User"))
                .Calling(q => q.ChangeExpireDate(1, "01-06-2016"))
                .ShouldReturn()
                .Ok()
                .WithResponseModel(MessageConstants.ChangeDateMessage);
        }
    }
}
