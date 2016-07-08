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
            notesService.Setup(a => a.ChangeNoteContent(It.IsAny<Note>(), It.IsAny<string>()));
            notesService.Setup(a => a.ChangeNoteExpireDate(It.IsAny<Note>(), It.IsAny<DateTime>()));
            notesService.Setup(a => a.ChangeNoteTitle(It.IsAny<Note>(), It.IsAny<string>()));
            notesService.Setup(a => a.GetNotes(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(GenerateNotes);
            notesService.Setup(a => a.GetNotesWithExpiredDate(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(GenerateNotes);
            notesService.Setup(a => a.RemoveNoteById(It.IsAny<Note>()));

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
    }
}
