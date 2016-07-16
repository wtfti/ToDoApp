namespace ToDo.Tests.Api
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Data.Models;
    using Moq;
    using Services.Data.Contracts;

    public static class DependencyObjectFactory
    {
        private const int Count = 50;

        public static INotesService GetNotesService()
        {
            var notesService = new Mock<INotesService>();

            notesService.Setup(a => a.AddNote(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime?>()));
            notesService.Setup(a => a.All()).Returns(GenerateNotes);
            notesService.Setup(a => a.GetNotes(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(GenerateNotes);
            notesService.Setup(a => a.GetNotesFromToday(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(GenerateTodayNotes);
            notesService.Setup(a => a.GetNotesWithExpiredDate(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(GenerateNotes);
            notesService.Setup(a => a.GetCompletedNotes(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(GenerateCompleteNotes);
            notesService.Setup(a => a.RemoveNoteById(It.IsAny<Note>()));
            notesService.Setup(a => a.GetNoteById(1))
                .Returns(GenerateNotes().First());

            return notesService.Object;
        }

        private static IQueryable<Note> GenerateNotes()
        {
            var result = new List<Note>();
            for (int i = 1; i <= Count; i++)
            {
                result.Add(new Note()
                {
                    Id = i,
                    Title = "TestTitle",
                    Content = "TestContent",
                    CreatedOn = DateTime.Now,
                    ExpiredOn = DateTime.Now.AddDays(i),
                    UserId = "User"
                });
            }

            return result.AsQueryable();
        }

        private static IQueryable<Note> GenerateCompleteNotes()
        {
            var result = new List<Note>();
            for (int i = 1; i <= Count; i++)
            {
                result.Add(new Note()
                {
                    Id = i,
                    Title = "TestTitle",
                    Content = "TestContent",
                    CreatedOn = DateTime.Now,
                    ExpiredOn = DateTime.Now.AddDays(i),
                    UserId = "User",
                    IsComplete = true
                });
            }

            return result.AsQueryable();
        }

        private static IQueryable<Note> GenerateTodayNotes()
        {
            var result = new List<Note>();
            for (int i = 1; i <= Count; i++)
            {
                result.Add(new Note()
                {
                    Id = i,
                    CreatedOn = DateTime.Now,
                    UserId = "User",
                    IsComplete = false,
                    IsExpired = false
                });
            }

            return result.AsQueryable();
        }
    }
}
