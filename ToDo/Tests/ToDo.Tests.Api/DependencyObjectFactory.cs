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
        private static IQueryable<Note> Notes()
        {
            var result = new List<Note>();
            result.Add(new Note()
            {
                Id = 1,
                Title = "TestTitle",
                Content = "TestContent",
                CreatedOn = DateTime.Now,
                ExpiredOn = DateTime.Now.AddDays(1),
                UserId = "User"
            });

            return result.AsQueryable();
        }

        public static INotesService GetNotesService()
        {
            var notesService = new Mock<INotesService>();

            notesService.Setup(a => a.AddNote(It.IsAny<string>(),It.IsAny<string>(),It.IsAny<string>(),It.IsAny<DateTime?>()));
            notesService.Setup(a => a.All()).Returns(Notes);
            notesService.Setup(a => a.ChangeNoteContent(It.IsAny<Note>(), It.IsAny<string>()));
            notesService.Setup(a => a.ChangeNoteExpireDate(It.IsAny<Note>(), It.IsAny<DateTime>()));
            notesService.Setup(a => a.ChangeNoteTitle(It.IsAny<Note>(), It.IsAny<string>()));
            notesService.Setup(a => a.GetNotes(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(Notes);
            notesService.Setup(a => a.GetNotesWithExpiredDate(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(Notes);
            notesService.Setup(a => a.RemoveNoteById(It.IsAny<Note>()));

            return notesService.Object;
        }
    }
}
