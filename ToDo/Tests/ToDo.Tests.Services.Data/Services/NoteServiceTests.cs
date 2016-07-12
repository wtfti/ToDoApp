namespace ToDo.Tests.Services.Data.Services
{
    using System;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using ToDo.Data.Models;
    using ToDo.Services.Data;
    using ToDo.Services.Data.Contracts;

    [TestClass]
    public class NoteServiceTests
    {
        private static int notesCount = 100;
        private INotesService service;
        private InMemoryRepository<Note> noteRepository;

        [TestInitialize]
        public void Initialize()
        {
            this.noteRepository = DepedencyObjectFactory.GetNoteRepository(notesCount);
            this.service = new NotesService(this.noteRepository);
        }

        [TestMethod]
        public void AllShouldReturnCorrectResult()
        {
            var result = this.service.All();

            Assert.AreEqual(notesCount, result.Count());
            Assert.IsNotNull(result.ToList()[1]);
        }

        [TestMethod]
        public void RemoveByIdShouldReturnCorrectResult()
        {
            var note = new Note()
            {
                Id = 1,
                Title = "Title " + 1,
                Content = "Content " + 1,
                CreatedOn = DateTime.Now.AddDays(1),
                ExpiredOn = DateTime.Now.AddDays(1 + 1),
                UserId = 1.ToString()
            };

            this.service.RemoveNoteById(note);

            Assert.AreEqual(notesCount - 1, this.service.All().ToList().Count);
            Assert.AreEqual(2, this.service.All().ToList()[0].Id);
            Assert.AreEqual(1, this.noteRepository.SaveChanges());
        }

        [TestMethod]
        public void ChangeNoteTitleShouldReturnCorrectResult()
        {
            Note note = this.service.All().First();

            this.service.ChangeNoteTitle(note, "value");

            Assert.AreEqual("value", this.service.All().ToList()[0].Title);
            Assert.AreEqual(1, this.noteRepository.NumberOfSaves);
        }

        [TestMethod]
        public void ChangeNoteContentShouldReturnCorrectResult()
        {
            Note note = this.service.All().First();

            this.service.ChangeNoteContent(note, "value");

            Assert.AreEqual("value", this.service.All().ToList()[0].Content);
            Assert.AreEqual(1, this.noteRepository.NumberOfSaves);
        }

        [TestMethod]
        public void ChangeNoteExpireDateShouldReturnCorrectResult()
        {
            Note note = this.service.All().First();

            var date = new DateTime(2015, 3, 14);
            this.service.ChangeNoteExpireDate(note, date);

            Assert.AreEqual(date, this.service.All().ToList()[0].ExpiredOn.Value);
            Assert.AreEqual(1, this.noteRepository.NumberOfSaves);
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
                this.service.AddNote(user, string.Empty, string.Empty);
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
                this.service.AddNote(user, string.Empty, string.Empty, date);
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

            this.service.AddNote(user, title, content);

            Assert.AreEqual(notesCount + 1, this.service.All().Count());
            Assert.AreEqual(user, this.noteRepository.All().Last().UserId);
            Assert.AreEqual(1, this.noteRepository.NumberOfSaves);
        }

        [TestMethod]
        public void AddNoteWithExpireDateShouldReturnCorrectResult()
        {
            string user = "username";
            string title = "username title";
            string content = "some test content";
            DateTime expire = new DateTime(2014, 3, 3);

            this.service.AddNote(user, title, content, expire);

            Assert.AreEqual(notesCount + 1, this.service.All().Count());
            Assert.AreEqual(user, this.noteRepository.All().Last().UserId);
            Assert.AreEqual(1, this.noteRepository.NumberOfSaves);
            Assert.AreEqual(expire, this.service.All().Last().ExpiredOn);
        }

        [TestMethod]
        public void SetExpiredShouldReturnCorrectResult()
        {
            var note = new Note()
            {
                IsExpired = false
            };
            this.service.SetExpired(note);

            Assert.AreEqual(1, this.noteRepository.SaveChanges());
            Assert.IsTrue(note.IsExpired);
        }

        [TestMethod]
        public void SetCompleteShouldReturnCorrectResult()
        {
            var note = new Note()
            {
                IsComplete = false
            };
            this.service.SetComplete(note);

            Assert.AreEqual(1, this.noteRepository.SaveChanges());
            Assert.IsTrue(note.IsComplete);
        }

        [TestMethod]
        public void GetNotesWithExpiredDateShouldReturnEmptyCollection()
        {
            var result = this.service.GetNotesWithExpiredDate("101", 1);

            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public void GetNotesWithExpiredDateShouldPass()
        {
            var result = this.service.GetNotesWithExpiredDate("10", 1);

            Assert.AreEqual(1, result.Count());
            Assert.AreEqual("10", result.First().UserId);
        }

        [TestMethod]
        public void GetCompletedNotesShouldReturnEmptyCollection()
        {
            var result = this.service.GetCompletedNotes("1", 1);

            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public void GetCompleteNotesShouldPass()
        {
            var result = this.service.GetCompletedNotes("complete", 1, notesCount);

            Assert.AreEqual(notesCount, result.Count());
        }
    }
}
