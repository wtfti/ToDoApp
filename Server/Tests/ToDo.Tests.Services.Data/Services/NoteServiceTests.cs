namespace ToDo.Tests.Services.Data.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using ToDo.Data.Common.Contracts;
    using ToDo.Data.Models;
    using ToDo.Data.Models.Account;
    using ToDo.Services.Data.Contracts;
    using ToDo.Services.Data.Services;

    [TestClass]
    public class NoteServiceTests
    {
        private static int notesCount = 100;
        private INotesService service;
        private InMemoryRepository<PrivateNote> privateNotesRepository;
        private InMemoryRepository<SharedNote> sharedNotesRepository;
        private InMemoryRepository<ProfileDetails> profileDetailsRepository;
        private InMemoryRepository<User> usersRepository;
        private IFriendsService friendsService;
        private IAccountService accountService;

        [TestInitialize]
        public void Initialize()
        {
            this.privateNotesRepository = new InMemoryRepository<PrivateNote>();
            this.sharedNotesRepository = new InMemoryRepository<SharedNote>();
            this.profileDetailsRepository = new InMemoryRepository<ProfileDetails>();
            this.usersRepository = new InMemoryRepository<User>();
            this.friendsService = new FriendsService(null, new InMemoryRepository<Friend>());
            this.accountService = new AccountService(this.profileDetailsRepository, this.usersRepository, this.friendsService);
            this.service = new NotesService(this.privateNotesRepository, this.sharedNotesRepository, this.accountService, this.friendsService);
        }

        [TestMethod]
        public void AllShouldReturnCorrectResult()
        {
            var result = this.service.All();

            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public void RemovePrivateNoteByIdShouldReturnCorrectResult()
        {
            for (int i = 0; i < 10; i++)
            {
                this.privateNotesRepository.Add(new PrivateNote()
                {
                    Id = i.ToString(),
                    Title = "title " + i,
                    Content = "content " + i
                });
            }

            var note = this.service.GetPrivateNoteById("5");

            Assert.IsNotNull(note);

            this.service.RemovePrivateNoteById(note);

            Assert.AreEqual(1, this.privateNotesRepository.SaveChanges());
            Assert.AreEqual("title 5", note.Title);
            Assert.AreEqual("content 5", note.Content);
        }

        [TestMethod]
        public void GetNotesShouldReturnCorrectResult()
        {
            string userId = "10";
            int page = 1;

            string user = "UserTest";

            var mockedAccountService = new Mock<IAccountService>();
            mockedAccountService.Setup(a => a.GetUserByUsername(It.IsAny<string>()))
                .Returns(new User()
                {
                    UserName = user,
                    Id = userId
                });

            var noteService = new NotesService(this.privateNotesRepository, null, mockedAccountService.Object, null);

            for (int i = 0; i < 20; i++)
            {
                noteService.AddPrivateNote(user, "Title " + i, string.Empty);
            }

            var result = noteService.GetNotes(userId, page);

            Assert.AreEqual(10, result.ToList().Count);
            Assert.AreEqual("Title 0", result.First().Title);
            Assert.AreEqual(userId, result.First().UserId);
            Assert.AreEqual(userId, result.Last().UserId);
        }

        [TestMethod]
        public void GetNotesPageTestingShouldReturnCorrectResult()
        {
            string user = "UserTest";

            var mockedAccountService = new Mock<IAccountService>();
            mockedAccountService.Setup(a => a.GetUserByUsername(It.IsAny<string>()))
                .Returns(new User()
                {
                    UserName = user,
                    Id = user
                });

            var noteService = new NotesService(this.privateNotesRepository, null, mockedAccountService.Object, null);
            for (int i = 0; i < 20; i++)
            {
                noteService.AddPrivateNote(user, string.Empty, string.Empty);
            }

            var notes = this.privateNotesRepository.All();

            Assert.IsNotNull(notes);
            Assert.AreEqual(notes.Last().UserId, user);
            Assert.AreEqual(20, notes.Count());
            Assert.AreEqual(20, this.privateNotesRepository.SaveChanges());
        }

        [TestMethod]
        public void GetNotesWithExpiredDateShouldReturnCorrectResult()
        {
            string userId = "8";
            int page = 1;

            var mockedAccountService = new Mock<IAccountService>();
            mockedAccountService.Setup(a => a.GetUserByUsername(It.IsAny<string>()))
                .Returns(new User()
                {
                    Id = userId
                });

            var noteService = new NotesService(this.privateNotesRepository, null, mockedAccountService.Object, null);
            for (int i = 0; i < 20; i++)
            {
                this.privateNotesRepository.Add(new PrivateNote()
                {
                    IsExpired = i % 2 == 0,
                    Title = "Title " + i,
                    UserId = userId
                });
            }

            var result = noteService.GetPrivateNotesWithExpirationDate(userId, page).ToList();

            for (int i = 0; i < 10; i++)
            {
                Assert.IsTrue(result[i].IsExpired);
            }

            Assert.AreEqual(10, result.Count);
            Assert.AreEqual("Title 0", result.First().Title);
            Assert.AreEqual(userId, result.First().UserId);
        }

        [TestMethod]
        public void GetNotesWithExpiredDatePageTestingShouldReturnCorrectResult()
        {
            string user = "UserTest";

            var mockedAccountService = new Mock<IAccountService>();
            mockedAccountService.Setup(a => a.GetUserByUsername(It.IsAny<string>()))
                .Returns(new User()
                {
                    Id = user
                });

            var noteService = new NotesService(this.privateNotesRepository, null, mockedAccountService.Object, null);
            for (int i = 0; i < 20; i++)
            {
                this.privateNotesRepository.Add(new PrivateNote()
                {
                    IsExpired = i != 19,
                    Title = "Title " + i,
                    UserId = user
                });
            }

            var firstPage = noteService.GetPrivateNotesWithExpirationDate(user, 1).ToList();
            var secondPage = noteService.GetPrivateNotesWithExpirationDate(user, 2).ToList();

            Assert.IsNotNull(firstPage);
            Assert.IsNotNull(firstPage);

            for (int i = 0; i < 10; i++)
            {
                Assert.IsTrue(firstPage[i].IsExpired);
            }

            for (int i = 0; i < 9; i++)
            {
                Assert.IsTrue(secondPage[i].IsExpired);
            }

            Assert.AreEqual(firstPage.Last().UserId, user);
            Assert.AreEqual(secondPage.Last().UserId, user);
        }

        [TestMethod]
        public void AddNoteShouldReturnCorrectResult()
        {
            string user = "username";
            string title = "username title";
            string content = "some test content";
            int count = this.service.All().Count();
            this.usersRepository.Add(new User()
            {
                UserName = "username",
                Id = "123456"
            });

            this.service.AddPrivateNote(user, title, content);

            Assert.AreEqual(count + 1, this.service.All().Count());
            Assert.AreEqual("123456", this.privateNotesRepository.All().Last().UserId);
            Assert.AreEqual(content, this.privateNotesRepository.All().Last().Content);
            Assert.AreEqual(1, this.privateNotesRepository.NumberOfSaves);
        }

        [TestMethod]
        public void SetExpiredShouldReturnCorrectResult()
        {
            var note = new PrivateNote()
            {
                IsExpired = false
            };
            this.service.SetExpiredPrivateNote(note);

            Assert.AreEqual(1, this.privateNotesRepository.SaveChanges());
            Assert.IsTrue(note.IsExpired);
        }

        [TestMethod]
        public void SetCompleteShouldReturnCorrectResult()
        {
            var note = new PrivateNote()
            {
                IsComplete = false
            };
            this.service.SetCompletePrivateNote(note);

            Assert.AreEqual(1, this.privateNotesRepository.SaveChanges());
            Assert.IsTrue(note.IsComplete);
        }

        [TestMethod]
        public void GetNotesWithExpiredDateShouldReturnEmptyCollection()
        {
            var result = this.service.GetPrivateNotesWithExpirationDate("101", 1);

            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public void GetCompletedNotesShouldReturnEmptyCollection()
        {
            for (int i = 1; i <= 10; i++)
            {
                this.privateNotesRepository.Add(new PrivateNote()
                {
                    IsComplete = true,
                    UserId = "complete"
                });
            }

            var result = this.service.GetCompletedPrivateNotes("1", 1);

            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public void GetCompleteNotesShouldPass()
        {
            for (int i = 1; i <= 10; i++)
            {
                this.privateNotesRepository.Add(new PrivateNote()
                {
                    IsComplete = true,
                    UserId = "complete"
                });
            }

            var result = this.service.GetCompletedPrivateNotes("complete", 1);

            Assert.AreEqual(10, result.Count());
            Assert.IsTrue(result.Any(a => a.IsComplete));
        }

        [TestMethod]
        public void GetNotesFromTodayShouldReturnEmptyCollection()
        {
            var result = this.service.GetNotesFromToday("1", 1);

            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public void GetNotesFromTodayShouldPass()
        {
            this.privateNotesRepository.Add(new PrivateNote()
            {
                UserId = "today",
                CreatedOn = DateTime.Now
            });

            this.privateNotesRepository.Add(new PrivateNote()
            {
                UserId = "today",
                CreatedOn = DateTime.Now,
                IsExpired = true
            });

            this.privateNotesRepository.Add(new PrivateNote()
            {
                UserId = "today",
                CreatedOn = DateTime.Now,
                IsComplete = true
            });

            this.privateNotesRepository.Add(new PrivateNote()
            {
                UserId = "today",
                CreatedOn = DateTime.Now,
                IsComplete = true,
                IsExpired = true
            });

            var result = this.service.GetNotesFromToday("today", 1);

            Assert.AreEqual(1, result.Count());
            Assert.IsFalse(result.First().IsComplete);
            Assert.IsFalse(result.First().IsExpired);
            Assert.AreEqual("today", result.First().UserId);
        }

        [TestMethod]
        public void GetNotesFromTodayShouldPassOption2()
        {
            var result = this.service.GetNotesFromToday("today", 2, notesCount);

            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public void GetNotesFromTodayShouldPassOption3()
        {
            this.privateNotesRepository.Add(new PrivateNote()
            {
                UserId = "today",
                CreatedOn = DateTime.Now
            });

            this.privateNotesRepository.Add(new PrivateNote()
            {
                UserId = "today",
                CreatedOn = DateTime.Now.AddDays(1),
                IsExpired = true
            });

            this.privateNotesRepository.Add(new PrivateNote()
            {
                UserId = "today",
                CreatedOn = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day - 1),
                IsComplete = true
            });

            var result = this.service.GetNotesFromToday("today", 1);

            Assert.AreEqual(1, result.Count());
            Assert.IsFalse(result.First().IsComplete);
            Assert.IsFalse(result.First().IsExpired);
            Assert.AreEqual("today", result.First().UserId);
        }

        [TestMethod]
        public void ChangeNoteShouldPass()
        {
            var dbNote = new PrivateNote()
            {
                Title = "test",
                Content = "content",
                ExpiredOn = new DateTime(2016, 1, 1)
            };

            string newTitle = "test";
            string newContent = "content";
            DateTime? newExpirationDate = new DateTime(2016, 1, 1);

            this.service.ChangePrivateNote(dbNote, newTitle, newContent, newExpirationDate);

            Assert.AreEqual(1, this.privateNotesRepository.SaveChanges());
        }

        [TestMethod]
        public void ChangeNoteShouldPassOption2()
        {
            var dbNote = new PrivateNote()
            {
                Title = "test",
                Content = "content",
                ExpiredOn = new DateTime(2016, 1, 1)
            };

            string newTitle = "test------";
            string newContent = "content";
            DateTime? newExpirationDate = new DateTime(2016, 1, 1);

            this.service.ChangePrivateNote(dbNote, newTitle, newContent, newExpirationDate);

            Assert.AreEqual(1, this.privateNotesRepository.SaveChanges());
        }

        [TestMethod]
        public void ChangeNoteShouldPassOption3()
        {
            var dbNote = new PrivateNote()
            {
                Title = "test",
                Content = "content",
                ExpiredOn = new DateTime(2016, 1, 1)
            };

            string newTitle = "test";
            string newContent = "content------";
            DateTime? newExpirationDate = new DateTime(2016, 1, 1);

            this.service.ChangePrivateNote(dbNote, newTitle, newContent, newExpirationDate);

            Assert.AreEqual(1, this.privateNotesRepository.SaveChanges());
        }

        [TestMethod]
        public void ChangeNoteShouldPassOption4()
        {
            var dbNote = new PrivateNote()
            {
                Title = "test",
                Content = "content",
                ExpiredOn = new DateTime(2016, 1, 1)
            };

            string newTitle = "test";
            string newContent = "content";
            DateTime? newExpirationDate = new DateTime(2016, 1, 2);

            this.service.ChangePrivateNote(dbNote, newTitle, newContent, newExpirationDate);

            Assert.AreEqual(1, this.privateNotesRepository.SaveChanges());
        }

        [TestMethod]
        public void GetNoteByIdShouldPass()
        {
            var dbNote = new PrivateNote()
            {
                Id = "0",
                Title = "Title " + 5,
                Content = "Content " + 5,
                CreatedOn = DateTime.Now.AddDays(5),
                ExpiredOn = DateTime.Now.AddDays(5 + 5),
                UserId = 5.ToString()
            };

            this.privateNotesRepository.Add(dbNote);
            var result = this.service.GetPrivateNoteById("0");

            Assert.AreEqual(result.Id, dbNote.Id);
            Assert.AreEqual(result.Title, dbNote.Title);
            Assert.AreEqual(result.Content, dbNote.Content);
            Assert.AreEqual(result.CreatedOn.Date, dbNote.CreatedOn.Date);
            Assert.AreEqual(result.ExpiredOn.Value.Date, dbNote.ExpiredOn.Value.Date);
            Assert.AreEqual(result.UserId, dbNote.UserId);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetNoteByIdShouldReturnNull()
        {
            this.service.GetPrivateNoteById("0");
            this.service.GetPrivateNoteById("356");
        }
    }
}
