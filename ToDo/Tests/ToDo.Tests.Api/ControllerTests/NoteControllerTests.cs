namespace ToDo.Tests.Api.ControllerTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Data.Models;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using MyTested.WebApi;
    using Server.Common.Constants;
    using Services.Data.Contracts;
    using ToDo.Api;
    using ToDo.Api.Controllers;
    using ToDo.Api.Infrastructure.Validation;
    using ToDo.Api.Models.Note;

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
                .WithResponseModel(MessageConstants.CreateNote);
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
                .WithAuthenticatedUser(a => a.WithIdentifier("Wrong user"))
                .Calling(q => q.RemoveNoteById(2))
                .ShouldReturn()
                .BadRequest()
                .WithErrorMessage(MessageConstants.NoteDoesNotExist);
        }

        [TestMethod]
        public void RemoveNoteByIdWithNoneExistingNoteForCurrentUserShouldReturnBadRequest()
        {
            MyWebApi
                .Controller<NoteController>()
                .WithResolvedDependencyFor(DependencyObjectFactory.GetNotesService())
                .WithAuthenticatedUser(a => a.WithIdentifier("Wrong user"))
                .Calling(q => q.RemoveNoteById(1))
                .ShouldReturn()
                .BadRequest()
                .WithErrorMessage(MessageConstants.NoteDoesNotExist);
        }

        [TestMethod]
        public void RemoveNoteByIdShouldReturnProperResponse()
        {
            MyWebApi
                .Controller<NoteController>()
                .WithResolvedDependencyFor(DependencyObjectFactory.GetNotesService())
                .WithAuthenticatedUser(a => a.WithIdentifier("User"))
                .Calling(q => q.RemoveNoteById(1))
                .ShouldReturn()
                .Ok()
                .WithResponseModel(MessageConstants.RemoveNote);
        }

        [TestMethod]
        public void ChangeNoteTitleWithInvalidIdShouldReturnBadRequest()
        {
            MyWebApi
                .Controller<NoteController>()
                .WithResolvedDependencyFor(DependencyObjectFactory.GetNotesService())
                .WithAuthenticatedUser(a => a.WithIdentifier("Wrong user"))
                .Calling(q => q.ChangeNoteTitle(2, "str"))
                .ShouldReturn()
                .BadRequest()
                .WithErrorMessage(MessageConstants.NoteDoesNotExist);
        }

        [TestMethod]
        public void ChangeNoteTitleWithNoneExistingNoteForCurrentUserShouldReturnBadRequest()
        {
            MyWebApi
                .Controller<NoteController>()
                .WithResolvedDependencyFor(DependencyObjectFactory.GetNotesService())
                .WithAuthenticatedUser(a => a.WithIdentifier("Wrong user"))
                .Calling(q => q.ChangeNoteTitle(1, "str"))
                .ShouldReturn()
                .BadRequest()
                .WithErrorMessage(MessageConstants.NoteDoesNotExist);
        }

        [TestMethod]
        public void ChangeNoteTitleShouldReturnProperResponse()
        {
            MyWebApi
                .Controller<NoteController>()
                .WithResolvedDependencyFor(DependencyObjectFactory.GetNotesService())
                .WithAuthenticatedUser(a => a.WithIdentifier("User"))
                .Calling(q => q.ChangeNoteTitle(1, "str"))
                .ShouldReturn()
                .Ok()
                .WithResponseModel(MessageConstants.TitleChange);
        }

        [TestMethod]
        public void ChangeNoteContentWithInvalidIdShouldReturnBadRequest()
        {
            MyWebApi
                .Controller<NoteController>()
                .WithResolvedDependencyFor(DependencyObjectFactory.GetNotesService())
                .WithAuthenticatedUser(a => a.WithIdentifier("Wrong user"))
                .Calling(q => q.ChangeNoteContent(2, "str"))
                .ShouldReturn()
                .BadRequest()
                .WithErrorMessage(MessageConstants.NoteDoesNotExist);
        }

        [TestMethod]
        public void ChangeNoteContentWithNoneExistingNoteForCurrentUserShouldReturnBadRequest()
        {
            MyWebApi
                .Controller<NoteController>()
                .WithResolvedDependencyFor(DependencyObjectFactory.GetNotesService())
                .WithAuthenticatedUser(a => a.WithIdentifier("Wrong user"))
                .Calling(q => q.ChangeNoteContent(1, "str"))
                .ShouldReturn()
                .BadRequest()
                .WithErrorMessage(MessageConstants.NoteDoesNotExist);
        }

        [TestMethod]
        public void ChangeNoteContentShouldReturnProperResponse()
        {
            MyWebApi
                .Controller<NoteController>()
                .WithResolvedDependencyFor(DependencyObjectFactory.GetNotesService())
                .WithAuthenticatedUser(a => a.WithIdentifier("User"))
                .Calling(q => q.ChangeNoteContent(1, "str"))
                .ShouldReturn()
                .Ok()
                .WithResponseModel(MessageConstants.ContextChange);
        }

        [TestMethod]
        public void SetNoteExpireDateWithInvalidIdShouldReturnBadRequest()
        {
            MyWebApi
                .Controller<NoteController>()
                .WithResolvedDependencyFor(DependencyObjectFactory.GetNotesService())
                .WithAuthenticatedUser(a => a.WithIdentifier("Wrong user"))
                .Calling(q => q.SetNoteExpireDate(2, "str"))
                .ShouldReturn()
                .BadRequest()
                .WithErrorMessage(MessageConstants.NoteDoesNotExist);
        }

        [TestMethod]
        public void SetNoteExpireDateWithNoneExistingNoteForCurrentUserShouldReturnBadRequest()
        {
            MyWebApi
                .Controller<NoteController>()
                .WithResolvedDependencyFor(DependencyObjectFactory.GetNotesService())
                .WithAuthenticatedUser(a => a.WithIdentifier("Wrong user"))
                .Calling(q => q.SetNoteExpireDate(1, "str"))
                .ShouldReturn()
                .BadRequest()
                .WithErrorMessage(MessageConstants.NoteDoesNotExist);
        }

        [TestMethod]
        public void SetNoteExpireDateWithWrongDateFormatShouldReturnBadRequest()
        {
            MyWebApi
                .Controller<NoteController>()
                .WithResolvedDependencyFor(DependencyObjectFactory.GetNotesService())
                .WithAuthenticatedUser(a => a.WithIdentifier("User"))
                .Calling(q => q.SetNoteExpireDate(1, "01.02.2016"))
                .ShouldReturn()
                .BadRequest()
                .WithErrorMessage(MessageConstants.InvalidDate);
        }

        [TestMethod]
        public void SetNoteExpireDateShouldReturnProperResponse()
        {
            MyWebApi
                .Controller<NoteController>()
                .WithResolvedDependencyFor(DependencyObjectFactory.GetNotesService())
                .WithAuthenticatedUser(a => a.WithIdentifier("User"))
                .Calling(q => q.SetNoteExpireDate(1, "01/06/2016 14:12"))
                .ShouldReturn()
                .Ok()
                .WithResponseModel(MessageConstants.SetDate);
        }

        [TestMethod]
        public void ChangeExpireDateWithInvalidIdShouldReturnBadRequest()
        {
            MyWebApi
                .Controller<NoteController>()
                .WithResolvedDependencyFor(DependencyObjectFactory.GetNotesService())
                .WithAuthenticatedUser(a => a.WithIdentifier("Wrong user"))
                .Calling(q => q.ChangeExpireDate(2, "str"))
                .ShouldReturn()
                .BadRequest()
                .WithErrorMessage(MessageConstants.NoteDoesNotExist);
        }

        [TestMethod]
        public void ChangeExpireDateWithNoneExistingNoteForCurrentUserShouldReturnBadRequest()
        {
            MyWebApi
                .Controller<NoteController>()
                .WithResolvedDependencyFor(DependencyObjectFactory.GetNotesService())
                .WithAuthenticatedUser(a => a.WithIdentifier("Wrong user"))
                .Calling(q => q.ChangeExpireDate(1, "str"))
                .ShouldReturn()
                .BadRequest()
                .WithErrorMessage(MessageConstants.NoteDoesNotExist);
        }

        [TestMethod]
        public void ChangeExpireDateWithWrongDateFormatShouldReturnBadRequest()
        {
            MyWebApi
                .Controller<NoteController>()
                .WithResolvedDependencyFor(DependencyObjectFactory.GetNotesService())
                .WithAuthenticatedUser(a => a.WithIdentifier("User"))
                .Calling(q => q.ChangeExpireDate(1, "01.02.2016"))
                .ShouldReturn()
                .BadRequest()
                .WithErrorMessage(MessageConstants.InvalidDate);
        }

        [TestMethod]
        public void ChangeExpireDateShouldReturnProperResponse()
        {
            MyWebApi
                .Controller<NoteController>()
                .WithResolvedDependencyFor(DependencyObjectFactory.GetNotesService())
                .WithAuthenticatedUser(a => a.WithIdentifier("User"))
                .Calling(q => q.ChangeExpireDate(1, "01/06/2016 16:26"))
                .ShouldReturn()
                .Ok()
                .WithResponseModel(MessageConstants.ChangeDate);
        }

        [TestMethod]
        public void SetCompleteShouldReturnNotFound()
        {
            MyWebApi
                .Controller<NoteController>()
                .WithResolvedDependencyFor(DependencyObjectFactory.GetNotesService())
                .WithAuthenticatedUser(a => a.WithIdentifier("User"))
                .Calling(q => q.SetComplete(101))
                .ShouldReturn()
                .BadRequest()
                .WithErrorMessage(MessageConstants.NoteDoesNotExist);
        }

        [TestMethod]
        public void SetCompleteWithInvalidUserShouldReturnNotFound()
        {
            MyWebApi
                .Controller<NoteController>()
                .WithResolvedDependencyFor(DependencyObjectFactory.GetNotesService())
                .WithAuthenticatedUser(a => a.WithIdentifier("User1"))
                .Calling(q => q.SetComplete(1))
                .ShouldReturn()
                .BadRequest()
                .WithErrorMessage(MessageConstants.NoteDoesNotExist);
        }

        [TestMethod]
        public void SetCompleteShouldReturnOk()
        {
            var notesService = new Mock<INotesService>();
            notesService.Setup(a => a.GetNoteById(It.IsAny<int>()))
                .Returns(new Note()
                {
                    Id = 1,
                    Title = "TestTitle",
                    Content = "TestContent",
                    CreatedOn = DateTime.Now,
                    ExpiredOn = DateTime.Now.AddDays(1),
                    UserId = "User"
                });

            notesService.Setup(a => a.SetComplete(It.IsAny<Note>()));
            MyWebApi
                .Controller<NoteController>()
                .WithResolvedDependencyFor(notesService.Object)
                .WithAuthenticatedUser(a => a.WithIdentifier("User"))
                .Calling(q => q.SetComplete(1))
                .ShouldReturn()
                .Ok();
        }

        [TestMethod]
        public void GetCompleteNotesShouldPass()
        {
            MyWebApi
                .Controller<NoteController>()
                .WithResolvedDependencyFor(DependencyObjectFactory.GetNotesService())
                .WithAuthenticatedUser(a => a.WithIdentifier("User"))
                .Calling(q => q.GetCompletedNotes(1))
                .ShouldReturn()
                .Ok()
                .WithResponseModelOfType<IQueryable<NoteResponseModel>>()
                .Passing(a =>
                {
                    Assert.AreEqual(50, a.Count());
                });
        }
    }
}
