namespace ToDo.Tests.Services.Data
{
    using System;
    using ToDo.Data.Models;

    public static class DepedencyObjectFactory
    {
        public static InMemoryRepository<Note> GetNoteRepository(int notesInRepository)
        {
            var repository = new InMemoryRepository<Note>();
            GenerateNotes(repository, notesInRepository);

            return repository;
        }

        private static void GenerateNotes(InMemoryRepository<Note> noteRepository, int count)
        {
            for (int i = 1; i <= count; i++)
            {
                noteRepository.Add(new Note()
                {
                    Id = i,
                    Title = "Title " + i,
                    Content = "Content " + i,
                    CreatedOn = DateTime.Now.AddDays(i),
                    ExpiredOn = DateTime.Now.AddDays(i + i),
                    UserId = i.ToString()
                });
            }
        }
    }
}
