namespace ToDo.Tests.Api
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Data.Models;
    using Data.Models.Account;
    using Moq;
    using Services.Data.Contracts;

    public static class DependencyObjectFactory
    {
        private const int Count = 50;

        public static INotesService MockNotesService()
        {
            var notesService = new Mock<INotesService>();

            notesService.Setup(a => a.AddPrivateNote(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime?>()));
            notesService.Setup(a => a.All()).Returns(GenerateNotes);
            notesService.Setup(a => a.GetNotes(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(GenerateNotes);
            notesService.Setup(a => a.GetNotesFromToday(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(GenerateTodayNotes);
            notesService.Setup(a => a.GetNotesWithExpirationDate(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(GenerateNotes);
            notesService.Setup(a => a.GetCompletedNotes(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(GenerateCompleteNotes);
            notesService.Setup(a => a.RemovePrivateNoteById(It.IsAny<PrivateNote>()));
            notesService.Setup(a => a.GetPrivateNoteById("1"))
                .Returns(GenerateNotes().First());

            return notesService.Object;
        }

        public static IAccountService MockAccountService()
        {
            var accountService = new Mock<IAccountService>();

            accountService.Setup(a => a.GetBackground(It.IsAny<string>())).Returns("image");
            accountService
                .Setup(a => a.ProfileDetails(It.IsAny<string>()))
                .Returns(new ProfileDetails());
            accountService.Setup(a => a.EditAccountSettings(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<int?>(),
                It.IsAny<GenderType>(),
                It.IsAny<string>(),
                It.IsAny<string>()));

            return accountService.Object;
        }

        private static IQueryable<PrivateNote> GenerateNotes()
        {
            var result = new List<PrivateNote>();
            for (int i = 1; i <= Count; i++)
            {
                result.Add(new PrivateNote()
                {
                    Id = i.ToString(),
                    Title = "TestTitle",
                    Content = "TestContent",
                    CreatedOn = DateTime.Now,
                    ExpiredOn = DateTime.Now.AddDays(i),
                    UserId = "User"
                });
            }

            return result.AsQueryable();
        }

        private static IQueryable<PrivateNote> GenerateCompleteNotes()
        {
            var result = new List<PrivateNote>();
            for (int i = 1; i <= Count; i++)
            {
                result.Add(new PrivateNote()
                {
                    Id = i.ToString(),
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

        private static IQueryable<PrivateNote> GenerateTodayNotes()
        {
            var result = new List<PrivateNote>();
            for (int i = 1; i <= Count; i++)
            {
                result.Add(new PrivateNote()
                {
                    Id = i.ToString(),
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
