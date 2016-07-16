namespace ToDo.Services.Data.Contracts
{
    using System;
    using System.Linq;
    using Server.Common.Constants;
    using ToDo.Data.Models;

    public interface INotesService
    {
        Note GetNoteById(int id);

        IQueryable<Note> GetCompletedNotes(string user, int page, int pageSize = ValidationConstants.DefaultPageSize);

        IQueryable<Note> GetNotesFromToday(string user, int page, int pageSize = ValidationConstants.DefaultPageSize);

        IQueryable<Note> GetNotes(string user, int page, int pageSize = ValidationConstants.DefaultPageSize);

        IQueryable<Note> GetNotesWithExpirationDate(string user, int page, int pageSize = ValidationConstants.DefaultPageSize);

        void AddNote(string user, string title, string content, DateTime? expireDate = null);

        void EditNote(Note dbNote, string newTitle, string newContent, DateTime? newExpiredOn);

        IQueryable<Note> All();

        void RemoveNoteById(Note note);

        void SetExpired(Note note);

        void SetComplete(Note note);
    }
}
