namespace ToDo.Tests.Services.Data.Services
{
    using System;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            this.privateNotesRepository = DepedencyObjectFactory.GetNoteRepository(notesCount);
            this.sharedNotesRepository = new InMemoryRepository<SharedNote>();
            this.profileDetailsRepository = new InMemoryRepository<ProfileDetails>();
            this.usersRepository = new InMemoryRepository<User>();
            this.friendsService = new FriendsService(null , new InMemoryRepository<Friend>());
            this.accountService = new AccountService(this.profileDetailsRepository, this.usersRepository, this.friendsService);
            this.service = new NotesService(this.privateNotesRepository, this.sharedNotesRepository, this.accountService);
        }

        [TestMethod]
        public void AllShouldReturnCorrectResult()
        {
            var result = this.service.All();

            Assert.AreEqual(notesCount * 2, result.Count());
            Assert.IsNotNull(result.ToList()[1]);
        }

        [TestMethod]
        public void RemoveByIdShouldReturnCorrectResult()
        {
            var note = new PrivateNote()
            {
                Id = "1",
                Title = "Title " + 1,
                Content = "Content " + 1,
                CreatedOn = DateTime.Now.AddDays(1),
                ExpiredOn = DateTime.Now.AddDays(1 + 1),
                UserId = 1.ToString()
            };

            int count = this.service.All().Count();

            this.service.RemovePrivateNoteById(note);

            Assert.AreEqual(count - 1, this.service.All().ToList().Count);
            Assert.AreEqual(2, this.service.All().ToList()[0].Id);
            Assert.AreEqual(1, this.privateNotesRepository.SaveChanges());
        }

        [TestMethod]
        public void GetNotesShouldReturnCorrectResult()
        {
            string userId = "10";
            int page = 1;

            var result = this.service.GetNotes(userId, page);

            Assert.AreEqual(1, result.ToList().Count);
            Assert.AreEqual("Title 10", result.First().Title);
            Assert.AreEqual(userId, result.First().UserId);
        }

        [TestMethod]
        public void GetNotesPageTestingShouldReturnCorrectResult()
        {
            string user = "UserTest";

            for (int i = 0; i < 20; i++)
            {
                this.service.AddPrivateNote(user, string.Empty, string.Empty);
            }

            var firstPageResult = this.service.GetNotes(user, 1);
            var secondPageResult = this.service.GetNotes(user, 2);

            Assert.IsNotNull(firstPageResult);
            Assert.IsNotNull(secondPageResult);
            Assert.AreEqual(firstPageResult.Last().UserId, user);
            Assert.AreEqual(secondPageResult.Last().UserId, user);
            Assert.AreEqual(10, firstPageResult.Count());
            Assert.AreEqual(10, secondPageResult.Count());
        }

        [TestMethod]
        public void GetNotesWithExpiredDateShouldReturnCorrectResult()
        {
            string userId = "8";
            int page = 1;

            var result = this.service.GetNotes(userId, page);

            Assert.AreEqual(1, result.ToList().Count);
            Assert.AreEqual("Title 8", result.First().Title);
            Assert.IsNotNull(result.First().ExpiredOn);
            Assert.AreEqual(userId, result.First().UserId);
        }

        [TestMethod]
        public void GetNotesWithExpiredDatePageTestingShouldReturnCorrectResult()
        {
            string user = "UserTest";
            DateTime date = new DateTime(2010, 10, 10);

            for (int i = 0; i < 20; i++)
            {
                this.service.AddPrivateNote(user, string.Empty, string.Empty, date);
            }

            var firstPageResult = this.service.GetNotes(user, 1);
            var secondPageResult = this.service.GetNotes(user, 2);

            Assert.IsNotNull(firstPageResult);
            Assert.IsNotNull(secondPageResult);
            Assert.AreEqual(firstPageResult.Last().UserId, user);
            Assert.AreEqual(secondPageResult.Last().UserId, user);
            Assert.AreEqual(10, firstPageResult.Count());
            Assert.AreEqual(10, secondPageResult.Count());
            Assert.AreEqual(date, firstPageResult.Last().ExpiredOn);
            Assert.AreEqual(date, secondPageResult.First().ExpiredOn);
        }

        [TestMethod]
        public void AddNoteShouldReturnCorrectResult()
        {
            string user = "username";
            string title = "username title";
            string content = "some test content";
            int count = this.service.All().Count();

            this.service.AddPrivateNote(user, title, content);

            Assert.AreEqual(count + 1, this.service.All().Count());
            Assert.AreEqual(user, this.privateNotesRepository.All().Last().UserId);
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
            this.service.SetExpired(note);

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
            this.service.SetComplete(note);

            Assert.AreEqual(1, this.privateNotesRepository.SaveChanges());
            Assert.IsTrue(note.IsComplete);
        }

        [TestMethod]
        public void GetNotesWithExpiredDateShouldReturnEmptyCollection()
        {
            var result = this.service.GetNotesWithExpirationDate("101", 1);

            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public void GetNotesWithExpiredDateShouldPass()
        {
            var note = this.service.GetPrivateNoteById("10");
            note.IsExpired = true;
            var result = this.service.GetNotesWithExpirationDate("expired", 1);

            Assert.AreEqual(10, result.Count());
            Assert.IsTrue(result.Any(a => a.UserId == "expired"));
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

            var result = this.service.GetCompletedNotes("1", 1);

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

            var result = this.service.GetCompletedNotes("complete", 1);

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

            this.service.ChangeNote(dbNote, newTitle, newContent, newExpirationDate);

            Assert.AreEqual(0, this.privateNotesRepository.SaveChanges());
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

            this.service.ChangeNote(dbNote, newTitle, newContent, newExpirationDate);

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

            this.service.ChangeNote(dbNote, newTitle, newContent, newExpirationDate);

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

            this.service.ChangeNote(dbNote, newTitle, newContent, newExpirationDate);

            Assert.AreEqual(1, this.privateNotesRepository.SaveChanges());
        }

        [TestMethod]
        public void GetNoteByIdShouldPass()
        {
            var result = this.service.GetPrivateNoteById("4");

            var dbNote = new PrivateNote()
            {
                Id = "5",
                Title = "Title " + 5,
                Content = "Content " + 5,
                CreatedOn = DateTime.Now.AddDays(5),
                ExpiredOn = DateTime.Now.AddDays(5 + 5),
                UserId = 5.ToString()
            };

            Assert.AreEqual(result.Id, dbNote.Id);
            Assert.AreEqual(result.Title, dbNote.Title);
            Assert.AreEqual(result.Content, dbNote.Content);
            Assert.AreEqual(result.CreatedOn.Date, dbNote.CreatedOn.Date);
            Assert.AreEqual(result.ExpiredOn.Value.Date, dbNote.ExpiredOn.Value.Date);
            Assert.AreEqual(result.UserId, dbNote.UserId);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GetNoteByIdShouldReturnNull()
        {
            this.service.GetPrivateNoteById(string.Empty);
            this.service.GetPrivateNoteById("356");
        }
    }
}
