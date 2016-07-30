namespace ToDo.Tests.Services.Data
{
    using System;
    using ToDo.Data.Models;
    using ToDo.Data.Models.Account;

    public static class DepedencyObjectFactory
    {
        public static InMemoryRepository<PrivateNote> GetNoteRepository(int notesCount)
        {
            var repository = new InMemoryRepository<PrivateNote>();
            GenerateNotes(repository, notesCount);

            return repository;
        }

        public static InMemoryRepository<ProfileDetails> GetProfileDetailsRepository()
        {
            var repository = new InMemoryRepository<ProfileDetails>();

            return repository;
        }

        private static void GenerateNotes(InMemoryRepository<PrivateNote> noteRepository, int count)
        {
            for (int i = 1; i <= count * 2; i++)
            {
                if (i <= count)
                {
                    noteRepository.Add(new PrivateNote()
                    {
                        Id = i,
                        Title = "Title " + i,
                        Content = "Content " + i,
                        CreatedOn = DateTime.Now.AddDays(i),
                        ExpiredOn = DateTime.Now.AddDays(i + i),
                        UserId = i.ToString()
                    });
                }
                else if (i > count)
                {
                    noteRepository.Add(new PrivateNote()
                    {
                        Id = i,
                        Title = "Expired Title " + i,
                        Content = "Expired Content " + i,
                        IsExpired = true,
                        UserId = "expired"
                    });
                }
            }
        }
    }
}
