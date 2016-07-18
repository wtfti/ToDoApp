namespace ToDo.Tests.Api.ControllerTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Web.Http;
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
        public void AddNoteShouldReturnBadRequestInvalidDate()
        {
            var notesService = new Mock<INotesService>();
            notesService.Setup(a => a.AddNote(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime?>()));
            var note = new NoteRequestModel();
            note.Title = "test";
            note.Content = "test";
            note.ExpiredOn = new DateTime(2016, 1, 1);

            MyWebApi
                .Controller<NoteController>()
                .WithResolvedDependencies(notesService.Object)
                .Calling(a => a.AddNote(note))
                .ShouldReturn()
                .BadRequest();
        }

        [TestMethod]
        public void AddNoteShouldReturnBadRequestInvalidDateOption2()
        {
            var notesService = new Mock<INotesService>();
            notesService.Setup(a => a.AddNote(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime?>()));
            var note = new NoteRequestModel();
            note.Title = "test";
            note.Content = "test";
            note.ExpiredOn = DateTime.Now;

            MyWebApi
                .Controller<NoteController>()
                .WithResolvedDependencies(notesService.Object)
                .Calling(a => a.AddNote(note))
                .ShouldReturn()
                .BadRequest();
        }

        [TestMethod]
        public void AddNoteShouldHaveHttpPost()
        {
            MyWebApi
                .Controller<NoteController>()
                .WithResolvedDependencyFor(DependencyObjectFactory.GetNotesService())
                .Calling(a => a.AddNote(new NoteRequestModel()))
                .ShouldHave()
                .ActionAttributes(a => a.ContainingAttributeOfType<HttpPostAttribute>());
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
        public void GetNotesShouldHaveHttpPost()
        {
            MyWebApi
                .Controller<NoteController>()
                .WithResolvedDependencyFor(DependencyObjectFactory.GetNotesService())
                .Calling(a => a.GetNotes(2))
                .ShouldHave()
                .ActionAttributes(a => a.ContainingAttributeOfType<HttpGetAttribute>());
        }

        [TestMethod]
        public void GetNotesShouldReturnInvalidPage()
        {
            MyWebApi
                .Controller<NoteController>()
                .WithResolvedDependencyFor(DependencyObjectFactory.GetNotesService())
                .Calling(a => a.GetNotes(-1))
                .ShouldReturn()
                .BadRequest();

            MyWebApi
               .Controller<NoteController>()
               .WithResolvedDependencyFor(DependencyObjectFactory.GetNotesService())
               .Calling(a => a.GetNotes(0))
               .ShouldReturn()
               .BadRequest();
        }

        [TestMethod]
        public void GetNotesWithExpirationDateShouldReturnProperResponse()
        {
            MyWebApi
                .Controller<NoteController>()
                .WithResolvedDependencyFor(DependencyObjectFactory.GetNotesService())
                .Calling(a => a.GetNotesWithExpirationDate(1))
                .ShouldReturn()
                .Ok()
                .WithResponseModelOfType<IQueryable<NoteResponseModel>>();
        }

        [TestMethod]
        public void GetNotesWithExpirationDateShouldReturnInvalidPage()
        {
            MyWebApi
                .Controller<NoteController>()
                .WithResolvedDependencyFor(DependencyObjectFactory.GetNotesService())
                .Calling(a => a.GetNotesWithExpirationDate(-1))
                .ShouldReturn()
                .BadRequest();

            MyWebApi
               .Controller<NoteController>()
               .WithResolvedDependencyFor(DependencyObjectFactory.GetNotesService())
               .Calling(a => a.GetNotesWithExpirationDate(0))
               .ShouldReturn()
               .BadRequest();
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
        public void RemoveNoteByIdWithNoneExistingNoteForCurrentUserShouldReturnBadRequestOption2()
        {
            MyWebApi
                .Controller<NoteController>()
                .WithResolvedDependencyFor(DependencyObjectFactory.GetNotesService())
                .WithAuthenticatedUser(a => a.WithIdentifier("Wrong user"))
                .Calling(q => q.RemoveNoteById(0))
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
        public void SetCompleteWithInvalidUserShouldReturnNoteDoesNotExist()
        {
            MyWebApi
                .Controller<NoteController>()
                .WithResolvedDependencyFor(DependencyObjectFactory.GetNotesService())
                .WithAuthenticatedUser(a => a.WithIdentifier("User1"))
                .Calling(q => q.SetComplete(-1))
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

        [TestMethod]
        public void GetCompleteNotesShouldReturnBadRequest()
        {
            MyWebApi
                .Controller<NoteController>()
                .WithResolvedDependencyFor(DependencyObjectFactory.GetNotesService())
                .Calling(a => a.GetCompletedNotes(-1))
                .ShouldReturn()
                .BadRequest();

            MyWebApi
               .Controller<NoteController>()
               .WithResolvedDependencyFor(DependencyObjectFactory.GetNotesService())
               .Calling(a => a.GetCompletedNotes(0))
               .ShouldReturn()
               .BadRequest();
        }

        [TestMethod]
        public void GetCompletedNotesShouldReturnEmptyCollection()
        {
            var notes = new List<Note>();

            var notesService = new Mock<INotesService>();
            notesService.Setup(a => a.GetCompletedNotes(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(notes.AsQueryable());

            MyWebApi
                .Controller<NoteController>()
                .WithResolvedDependencyFor(notesService.Object)
                .WithAuthenticatedUser(a => a.WithIdentifier("User"))
                .Calling(q => q.GetCompletedNotes(1))
                .ShouldReturn()
                .Ok()
                .WithResponseModelOfType<IQueryable<NoteResponseModel>>()
                .Passing(a =>
                {
                    Assert.AreEqual(0, a.Count());
                });
        }

        [TestMethod]
        public void GetNotesFromTodayShouldPass()
        {
            var notes = new List<Note>();
            for (int i = 0; i < 100; i++)
            {
                notes.Add(new Note()
                {
                    UserId = "User",
                    CreatedOn = DateTime.Now
                });
            }

            var notesService = new Mock<INotesService>();
            notesService.Setup(a => a.GetNotesFromToday(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(notes.AsQueryable());

            MyWebApi
                .Controller<NoteController>()
                .WithResolvedDependencyFor(notesService.Object)
                .WithAuthenticatedUser(a => a.WithIdentifier("User"))
                .Calling(q => q.GetNotesFromToday(1))
                .ShouldReturn()
                .Ok()
                .WithResponseModelOfType<IQueryable<NoteResponseModel>>()
                .Passing(a =>
                {
                    Assert.AreEqual(100, a.Count());
                    Assert.IsFalse(a.Any(x => x.IsComplete));
                    Assert.IsFalse(a.Any(x => x.IsExpired));
                    Assert.IsTrue(a.Any(x => x.CreatedOn.Date == DateTime.Now.Date));
                });
        }

        [TestMethod]
        public void GetNotesFromTodayShouldReturnEmptyColletion()
        {
            var notes = new List<Note>();

            var notesService = new Mock<INotesService>();
            notesService.Setup(a => a.GetNotesFromToday(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(notes.AsQueryable());

            MyWebApi
                .Controller<NoteController>()
                .WithResolvedDependencyFor(notesService.Object)
                .WithAuthenticatedUser(a => a.WithIdentifier("User"))
                .Calling(q => q.GetNotesFromToday(1))
                .ShouldReturn()
                .Ok()
                .WithResponseModelOfType<IQueryable<NoteResponseModel>>()
                .Passing(a =>
                {
                    Assert.AreEqual(0, a.Count());
                });
        }

        [TestMethod]
        public void GetNotesFromTodayShouldPassOption2()
        {
            var notes = new List<Note>()
            {
                new Note()
                {
                    Id = 1000,
                    UserId = "User",
                    CreatedOn = DateTime.Now
                },
                new Note()
                {
                    UserId = "User",
                    CreatedOn = DateTime.Now
                },
                new Note()
                {
                    UserId = "User",
                    CreatedOn = DateTime.Now
                }
            };
            var notesService = new Mock<INotesService>();
            notesService.Setup(a => a.GetNotesFromToday(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(notes.AsQueryable());

            MyWebApi
                .Controller<NoteController>()
                .WithResolvedDependencyFor(notesService.Object)
                .WithAuthenticatedUser(a => a.WithIdentifier("User"))
                .Calling(q => q.GetNotesFromToday(1))
                .ShouldReturn()
                .Ok()
                .WithResponseModelOfType<IQueryable<NoteResponseModel>>()
                .Passing(a =>
                {
                    Assert.AreEqual(notes.Count, a.Count());
                    Assert.AreEqual(1000, a.First().Id);
                });
        }

        [TestMethod]
        public void ChangeNoteShouldPass()
        {
            var dbNote = new Note()
            {
                UserId = "User"
            };

            var note = new NoteRequestModel();
            note.Title = "test";
            note.Content = "test";
            note.ExpiredOn = DateTime.Now.AddDays(1);
            note.Id = 5;

            var mock = new Mock<INotesService>();
            mock.Setup(a => a.GetNoteById(It.IsAny<int>())).Returns(dbNote);

            MyWebApi
                .Controller<NoteController>()
                .WithResolvedDependencyFor(mock.Object)
                .WithAuthenticatedUser(a => a.WithIdentifier("User"))
                .Calling(q => q.ChangeNote(note))
                .ShouldReturn()
                .Ok();
        }

        [TestMethod]
        public void ChangeNoteShouldReturnBadRequest()
        {
            var note = new NoteRequestModel();
            note.Title = "test";
            note.Content = "test";
            note.ExpiredOn = DateTime.Now.AddDays(1);

            MyWebApi
                .Controller<NoteController>()
                .WithResolvedDependencyFor(DependencyObjectFactory.GetNotesService())
                .WithAuthenticatedUser(a => a.WithIdentifier("User"))
                .Calling(q => q.ChangeNote(note))
                .ShouldReturn()
                .BadRequest()
                .WithErrorMessage(MessageConstants.NoteDoesNotExist);
        }

        [TestMethod]
        public void ChangeNoteShouldReturnBadRequestOption2()
        {
            var note = new NoteRequestModel();
            note.Title = "test";
            note.Content = "test";
            note.ExpiredOn = new DateTime?(new DateTime(2016, 1, 1));
            note.Id = 1;

            MyWebApi
                .Controller<NoteController>()
                .WithResolvedDependencyFor(DependencyObjectFactory.GetNotesService())
                .WithAuthenticatedUser(a => a.WithIdentifier("User"))
                .Calling(q => q.ChangeNote(note))
                .ShouldReturn()
                .BadRequest()
                .WithErrorMessage(MessageConstants.InvalidDate);
        }

        [TestMethod]
        public void ChangeNoteShouldReturnBadRequestOption3()
        {
            var note = new NoteRequestModel();
            note.Title = "test";
            note.Content = "test";
            note.ExpiredOn = DateTime.Now.AddDays(1);
            note.Id = -1;

            MyWebApi
                .Controller<NoteController>()
                .WithResolvedDependencyFor(DependencyObjectFactory.GetNotesService())
                .WithAuthenticatedUser(a => a.WithIdentifier("User"))
                .Calling(q => q.ChangeNote(note))
                .ShouldReturn()
                .BadRequest()
                .WithErrorMessage(MessageConstants.NoteDoesNotExist);
        }
    }
}
