namespace ToDo.Api.Models.Note
{
    using System;
    using Data.Models;
    using Infrastructure.Ninject.Mapping;

    public class NoteDataModel : IMapFrom<Note>
    {
        public string Id { get; set; }

        public DateTime? ExpiredOn { get; set; }

        public bool IsExpired { get; set; }

        public bool IsComplete { get; set; }
    }
}